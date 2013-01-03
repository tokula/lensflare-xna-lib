float4x4 World;
float4x4 View;
float4x4 Projection;
Texture ColorTexture;
Texture BumpMapTexture;
bool HasBumpMapTexture;
float3 LightPos;
float LightPower;
float Ambient;

sampler TextureSampler = sampler_state { texture = <ColorTexture> ; magfilter = LINEAR; minfilter = LINEAR; mipfilter=LINEAR; AddressU = mirror; AddressV = mirror;};
sampler BumpMapTextureSampler = sampler_state { texture = <BumpMapTexture> ; magfilter = LINEAR; minfilter = LINEAR; mipfilter=LINEAR; AddressU = mirror; AddressV = mirror;};

struct VertexToPixel
{
    float4 Position     : POSITION;    
    float2 TexCoords    : TEXCOORD0;
    float3 Normal        : TEXCOORD1;
    float3 Position3D    : TEXCOORD2;
	float3 Tangent : TEXCOORD3;
	float3 CoTangent : TEXCOORD4;
};

struct PixelToFrame
{
    float4 Color        : COLOR0;
};

float DotProduct(float3 lightPos, float3 pos3D, float3 normal)
{
    float3 lightDir = normalize(pos3D - lightPos);
    return dot(-lightDir, normal);    
}

VertexToPixel SimplestVertexShader( float4 inPos : POSITION0, float3 inNormal: NORMAL0, float2 inTexCoords : TEXCOORD0 )
{
    VertexToPixel Output = (VertexToPixel)0;
    
    Output.Position = mul(inPos, mul(World, mul(View, Projection)));
    Output.TexCoords = inTexCoords;
    Output.Normal = normalize(mul(inNormal, (float3x3)World));
	Output.Tangent = normalize(cross(inNormal, float3(1,0,1)));
	Output.CoTangent = normalize(cross(Output.Normal, Output.Tangent));
    Output.Position3D = mul(inPos, World);

    return Output;
}
 
PixelToFrame OurFirstPixelShader(VertexToPixel PSIn)
{
    PixelToFrame Output = (PixelToFrame)0;    

    float diffuseLightingFactor = DotProduct(LightPos, PSIn.Position3D, PSIn.Normal);
    diffuseLightingFactor = saturate(diffuseLightingFactor);
    diffuseLightingFactor *= LightPower;

    PSIn.TexCoords.y--;
    float4 baseColor = tex2D(TextureSampler, PSIn.TexCoords);

	float bumpFactor = 1;
	if(HasBumpMapTexture) {
		float4x4 mat;
		mat[0] = float4(1,0,0,0); mat[1] = float4(0,1,0,0); mat[2] = float4(0,0,1,0); mat[3] = float4(0,0,0,1);
		//mat = mul(PSIn.Normal, mat);

		float3 bumpNormalAsColor = tex2D(BumpMapTextureSampler, PSIn.TexCoords);
		bumpNormalAsColor[0] = (bumpNormalAsColor[0]-0.5)*2;
		bumpNormalAsColor[1] = bumpNormalAsColor[1];
		bumpNormalAsColor[2] = (bumpNormalAsColor[2]-0.5)*2;
		//bumpNormalAsColor[3] = 0;//(bumpNormalAsColor[3]-0.5)*2;
		bumpNormalAsColor = mul(bumpNormalAsColor, World);
		//bumpNormalAsColor = mul(mat, bumpNormalAsColor);
		bumpNormalAsColor = normalize(bumpNormalAsColor);
		
		//float3x3 objToTangent = transpose(float3x3(PSIn.Tangent, cross(PSIn.Normal, PSIn.Tangent), PSIn.Normal));
		float3x3 objToTangent = transpose(float3x3(PSIn.Tangent, PSIn.CoTangent, PSIn.Normal));
		bumpNormalAsColor = mul(objToTangent, bumpNormalAsColor);

		//bumpFactor = mul(bumpNormalAsColor, PSIn.Normal);

		float3 lightDir = normalize(LightPos - PSIn.Position3D);
		bumpFactor = mul(bumpNormalAsColor, lightDir);
		bumpFactor = max(0, bumpFactor);
	}

    Output.Color = baseColor*(diffuseLightingFactor * bumpFactor + Ambient);

    return Output;
}

technique Technique1
{
    pass Pass1
    {
        VertexShader = compile vs_2_0 SimplestVertexShader();
        PixelShader = compile ps_2_0 OurFirstPixelShader();
    }
}

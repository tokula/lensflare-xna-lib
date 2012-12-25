float4x4 World;
float4x4 View;
float4x4 Projection;
Texture Texture;

sampler TextureSampler = sampler_state { texture = <Texture> ; magfilter = LINEAR; minfilter = LINEAR; mipfilter=LINEAR; AddressU = mirror; AddressV = mirror;};
struct VertexToPixel
{
    float4 Position     : POSITION;    
    float2 TexCoords    : TEXCOORD0;
};

struct PixelToFrame
{
    float4 Color        : COLOR0;
};

VertexToPixel SimplestVertexShader( float4 inPos : POSITION, float2 inTexCoords : TEXCOORD0)
{
	VertexToPixel Output = (VertexToPixel)0;
     
	Output.Position = mul(inPos, mul(World, mul(View, Projection)));
	Output.TexCoords = inTexCoords;
 
	return Output;
}
 
PixelToFrame OurFirstPixelShader(VertexToPixel PSIn)
{
    PixelToFrame Output = (PixelToFrame)0;    
 
    Output.Color = tex2D(TextureSampler, PSIn.TexCoords);


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

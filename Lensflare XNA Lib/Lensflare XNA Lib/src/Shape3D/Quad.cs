using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Shape3D;

using VertexFormat = Microsoft.Xna.Framework.Graphics.VertexPositionNormalTexture;

namespace Shape3D {
    public class Quad : Shape {
        VertexBuffer vertexBuffer;
        IndexBuffer indexBuffer;

        public override int NumVertices { get { return 4; } }
        public override int NumPrimitives { get { return 2; } }

        public Quad(Game game, Vector3 size) {
			this.game = game;
            VertexFormat[] vertices = new VertexFormat[NumVertices];

			Vector3 topLeft =     new Vector3(-1.0f,  1.0f, 0.0f) * size;
			Vector3 bottomLeft =  new Vector3(-1.0f, -1.0f, 0.0f) * size;
			Vector3 topRight =    new Vector3( 1.0f,  1.0f, 0.0f) * size;
			Vector3 bottomRight = new Vector3( 1.0f, -1.0f, 0.0f) * size;

			Vector3 frontNormal = new Vector3(0.0f, 0.0f, -1.0f);

            Vector2 textureTopLeft = new Vector2(1.0f, 0.0f);
			Vector2 textureTopRight = new Vector2(0.0f, 0.0f);
			Vector2 textureBottomLeft = new Vector2(1.0f, 1.0f);
			Vector2 textureBottomRight = new Vector2(0.0f, 1.0f);

			// front face
            vertices[0] = new VertexFormat(topLeft, frontNormal, textureTopLeft);
            vertices[1] = new VertexFormat(bottomLeft, frontNormal, textureBottomLeft);
            vertices[2] = new VertexFormat(topRight, frontNormal, textureTopRight);
            vertices[3] = new VertexFormat(bottomRight, frontNormal, textureBottomRight);

            int[] indices = new int[6];

            int i = 0;

            indices[i++] = 0;
            indices[i++] = 3;
            indices[i++] = 2;

            indices[i++] = 0;
            indices[i++] = 1;
            indices[i++] = 3;

            vertexBuffer = new VertexBuffer(game.GraphicsDevice, typeof(VertexFormat), vertices.Length, BufferUsage.WriteOnly);
            vertexBuffer.SetData(vertices);
            indexBuffer = new IndexBuffer(game.GraphicsDevice, typeof(int), indices.Length, BufferUsage.WriteOnly);
            indexBuffer.SetData(indices);
        }

        public override void Draw() {
            game.GraphicsDevice.Indices = indexBuffer;
            game.GraphicsDevice.SetVertexBuffer(vertexBuffer);
            game.GraphicsDevice.DrawIndexedPrimitives(PrimitiveType.TriangleList, 0, 0, NumVertices, 0, NumPrimitives);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System.Diagnostics;

using VertexFormat = Microsoft.Xna.Framework.Graphics.VertexPositionNormalTexture;

namespace Shape3D {
    public class Tetrahedron : Shape {
        VertexBuffer vertexBuffer;

        public override int NumVertices { get { return 12; } }
        public override int NumPrimitives { get { return 4; } }

        public Tetrahedron(Game game, Vector3 size) {
            this.game = game;

            VertexFormat[] vertices = new VertexFormat[NumVertices];

            Vector3 topLeftFront = new Vector3(-1.0f, 1.0f, -1.0f) * size;
            Vector3 bottomRightFront = new Vector3(1.0f, -1.0f, -1.0f) * size;
            Vector3 topRightBack = new Vector3(1.0f, 1.0f, 1.0f) * size;
            Vector3 bottomLeftBack = new Vector3(-1.0f, -1.0f, 1.0f) * size;

            Vector3 bottomLeftFrontNormal = new Vector3(-1.0f, -1.0f, -1.0f);
            Vector3 topRightFrontNormal = new Vector3(1.0f, 1.0f, -1.0f);
            Vector3 bottomRightBackNormal = new Vector3(1.0f, -1.0f, 1.0f);
            Vector3 topLeftBackNormal = new Vector3(-1.0f, 1.0f, 1.0f);
            bottomLeftFrontNormal.Normalize();
            topRightFrontNormal.Normalize();
            bottomRightBackNormal.Normalize();
            topLeftBackNormal.Normalize();

            Vector2 textureTop = new Vector2(0.5f, 0.0f);
            Vector2 textureBottomLeft = new Vector2(1.0f, 1.0f);
            Vector2 textureBottomRight = new Vector2(0.0f, 1.0f);

            int i = 0;

            // bottom left front face
            vertices[i++] = new VertexFormat(bottomRightFront, bottomLeftFrontNormal, textureBottomRight);
            vertices[i++] = new VertexFormat(topLeftFront, bottomLeftFrontNormal, textureTop);
            vertices[i++] = new VertexFormat(bottomLeftBack, bottomLeftFrontNormal, textureBottomLeft);

            // top right front face
            vertices[i++] = new VertexFormat(topLeftFront, topRightFrontNormal, textureBottomRight);
            vertices[i++] = new VertexFormat(bottomRightFront, topRightFrontNormal, textureTop);
            vertices[i++] = new VertexFormat(topRightBack, topRightFrontNormal, textureBottomLeft);

            // bottom right back face
            vertices[i++] = new VertexFormat(bottomLeftBack, bottomRightBackNormal, textureBottomLeft);
            vertices[i++] = new VertexFormat(topRightBack, bottomRightBackNormal, textureTop);
            vertices[i++] = new VertexFormat(bottomRightFront, bottomRightBackNormal, textureBottomRight);

            // top left back face
            vertices[i++] = new VertexFormat(topRightBack, topLeftBackNormal, textureBottomLeft);
            vertices[i++] = new VertexFormat(bottomLeftBack, topLeftBackNormal, textureTop);
            vertices[i++] = new VertexFormat(topLeftFront, topLeftBackNormal, textureBottomRight);

            Debug.Assert(i == NumVertices, "wrong vertex count");

            vertexBuffer = new VertexBuffer(game.GraphicsDevice, typeof(VertexFormat), vertices.Length, BufferUsage.WriteOnly);
            vertexBuffer.SetData(vertices);
        }

        public override void Draw() {
            game.GraphicsDevice.SetVertexBuffer(vertexBuffer);
            game.GraphicsDevice.DrawPrimitives(PrimitiveType.TriangleList, 0, NumPrimitives);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System.Diagnostics;

using VertexFormat = Microsoft.Xna.Framework.Graphics.VertexPositionNormalTexture;

namespace Shape3D {
	public class Box : Shape {
        VertexBuffer vertexBuffer;

        public override int NumVertices { get { return 36; } }
        public override int NumPrimitives { get { return 12; } }

		public Box(Game game, Vector3 size) {
			this.game = game;
            VertexFormat[] vertices = new VertexFormat[NumVertices];

			Vector3 topLeftFront =     new Vector3(-1.0f,  1.0f, -1.0f) * size;
			Vector3 bottomLeftFront =  new Vector3(-1.0f, -1.0f, -1.0f) * size;
			Vector3 topRightFront =    new Vector3( 1.0f,  1.0f, -1.0f) * size;
			Vector3 bottomRightFront = new Vector3( 1.0f, -1.0f, -1.0f) * size;
			Vector3 topLeftBack =      new Vector3(-1.0f,  1.0f,  1.0f) * size;
			Vector3 topRightBack =     new Vector3( 1.0f,  1.0f,  1.0f) * size;
			Vector3 bottomLeftBack =   new Vector3(-1.0f, -1.0f,  1.0f) * size;
			Vector3 bottomRightBack =  new Vector3( 1.0f, -1.0f,  1.0f) * size;

			Vector3 frontNormal = new Vector3(0.0f, 0.0f, -1.0f);
			Vector3 backNormal = new Vector3(0.0f, 0.0f, 1.0f);
			Vector3 topNormal = new Vector3(0.0f, 1.0f, 0.0f);
			Vector3 bottomNormal = new Vector3(0.0f, -1.0f, 0.0f);
			Vector3 leftNormal = new Vector3(-1.0f, 0.0f, 0.0f);
			Vector3 rightNormal = new Vector3(1.0f, 0.0f, 0.0f);

			Vector2 textureTopLeft = new Vector2(1.0f, 0.0f);
			Vector2 textureTopRight = new Vector2(0.0f, 0.0f);
			Vector2 textureBottomLeft = new Vector2(1.0f, 1.0f);
			Vector2 textureBottomRight = new Vector2(0.0f, 1.0f);

            int i = 0;

			// front face
            vertices[i++] = new VertexFormat(topLeftFront, frontNormal, textureTopLeft);
            vertices[i++] = new VertexFormat(bottomLeftFront, frontNormal, textureBottomLeft);
            vertices[i++] = new VertexFormat(topRightFront, frontNormal, textureTopRight);
            vertices[i++] = new VertexFormat(bottomLeftFront, frontNormal, textureBottomLeft);
            vertices[i++] = new VertexFormat(bottomRightFront, frontNormal, textureBottomRight);
            vertices[i++] = new VertexFormat(topRightFront, frontNormal, textureTopRight);

			// back face
            vertices[i++] = new VertexFormat(topLeftBack, backNormal, textureTopRight);
            vertices[i++] = new VertexFormat(topRightBack, backNormal, textureTopLeft);
            vertices[i++] = new VertexFormat(bottomLeftBack, backNormal, textureBottomRight);
            vertices[i++] = new VertexFormat(bottomLeftBack, backNormal, textureBottomRight);
            vertices[i++] = new VertexFormat(topRightBack, backNormal, textureTopLeft);
            vertices[i++] = new VertexFormat(bottomRightBack, backNormal, textureBottomLeft);

			// top face
            vertices[i++] = new VertexFormat(topLeftFront, topNormal, textureTopRight);
            vertices[i++] = new VertexFormat(topRightBack, topNormal, textureBottomLeft);
            vertices[i++] = new VertexFormat(topLeftBack, topNormal, textureBottomRight);
            vertices[i++] = new VertexFormat(topLeftFront, topNormal, textureTopRight);
            vertices[i++] = new VertexFormat(topRightFront, topNormal, textureTopLeft);
            vertices[i++] = new VertexFormat(topRightBack, topNormal, textureBottomLeft);

			// bottom face
            vertices[i++] = new VertexFormat(bottomLeftFront, bottomNormal, textureTopLeft);
            vertices[i++] = new VertexFormat(bottomLeftBack, bottomNormal, textureBottomLeft);
            vertices[i++] = new VertexFormat(bottomRightBack, bottomNormal, textureBottomRight);
            vertices[i++] = new VertexFormat(bottomLeftFront, bottomNormal, textureTopLeft);
            vertices[i++] = new VertexFormat(bottomRightBack, bottomNormal, textureBottomRight);
            vertices[i++] = new VertexFormat(bottomRightFront, bottomNormal, textureTopRight);

			// left face
            vertices[i++] = new VertexFormat(topLeftFront, leftNormal, textureTopRight);
            vertices[i++] = new VertexFormat(bottomLeftBack, leftNormal, textureBottomLeft);
            vertices[i++] = new VertexFormat(bottomLeftFront, leftNormal, textureBottomRight);
            vertices[i++] = new VertexFormat(topLeftBack, leftNormal, textureTopLeft);
            vertices[i++] = new VertexFormat(bottomLeftBack, leftNormal, textureBottomLeft);
            vertices[i++] = new VertexFormat(topLeftFront, leftNormal, textureTopRight);

			// right face
            vertices[i++] = new VertexFormat(topRightFront, rightNormal, textureTopLeft);
            vertices[i++] = new VertexFormat(bottomRightFront, rightNormal, textureBottomLeft);
            vertices[i++] = new VertexFormat(bottomRightBack, rightNormal, textureBottomRight);
            vertices[i++] = new VertexFormat(topRightBack, rightNormal, textureTopRight);
            vertices[i++] = new VertexFormat(topRightFront, rightNormal, textureTopLeft);
            vertices[i++] = new VertexFormat(bottomRightBack, rightNormal, textureBottomRight);

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

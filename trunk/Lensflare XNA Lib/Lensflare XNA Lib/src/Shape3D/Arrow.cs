using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;

using VertexFormat = Microsoft.Xna.Framework.Graphics.VertexPositionNormalTexture; //TODO: ändern
using Microsoft.Xna.Framework;

namespace Shape3D {
    public class Arrow : Shape {
        VertexBuffer vertexBuffer;

        public override int NumVertices { get { return 2; } }
        public override int NumPrimitives { get { return 1; } }

        public Arrow(Game game, Vector3 size) {
			this.game = game;
            VertexFormat[] vertices = new VertexFormat[NumVertices];

			Vector3 top = new Vector3( 0.0f,  1.0f,  0.0f) * size;
            Vector3 bottom = new Vector3(0.0f, 0.0f, 0.0f) * size;

            int i = 0;

            vertices[i++] = new VertexFormat(bottom, Vector3.Up, new Vector2(0, 0));
            vertices[i++] = new VertexFormat(top, Vector3.Up, new Vector2(0, 0));

			vertexBuffer = new VertexBuffer(game.GraphicsDevice, typeof(VertexFormat), vertices.Length, BufferUsage.WriteOnly);
			vertexBuffer.SetData(vertices);
		}

		public override void Draw() {
            game.GraphicsDevice.SetVertexBuffer(vertexBuffer);
			game.GraphicsDevice.DrawPrimitives(PrimitiveType.LineList, 0, NumPrimitives);
		}
    }
}

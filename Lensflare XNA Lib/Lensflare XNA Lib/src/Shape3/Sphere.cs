using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using VertexFormat = Microsoft.Xna.Framework.Graphics.VertexPositionNormalTexture;

namespace Shape3 {
    public class Sphere : Shape { //TODO: level of detail
		VertexBuffer vertexBuffer;
        IndexBuffer indexBuffer;

        protected readonly int numVertices;
        protected readonly int numPrimitives;

        public override int NumVertices { get { return numVertices; } }
        public override int NumPrimitives { get { return numPrimitives; } }

        public Sphere(Game game, float radius, int slices, int stacks) {
			this.game = game;
            
            numVertices = (slices + 1) * (stacks + 1);
            VertexFormat[] vertices = new VertexFormat[numVertices];

            float phi, theta;
            float dphi = MathHelper.Pi / stacks;
            float dtheta = MathHelper.TwoPi / slices;
            float x, y, z, sc;
            int index = 0;

            for (int stack = 0; stack <= stacks; ++stack) {
                phi = MathHelper.PiOver2 - stack * dphi;
                y = radius * (float)Math.Sin(phi);
                sc = -radius * (float)Math.Cos(phi);

                for (int slice = 0; slice <= slices; ++slice) {
                    theta = slice * dtheta;
                    x = sc * (float)Math.Sin(theta);
                    z = sc * (float)Math.Cos(theta);
                    vertices[index++] = new VertexFormat(new Vector3(x, y, z), new Vector3(x, y, z), new Vector2((float)slice / (float)slices, (float)stack / (float)stacks));
                }
            }

            int numIndices = slices * stacks * 6;
            numPrimitives = numIndices / 3;
            int[] indices = new int[numIndices];
            index = 0;
            int k = slices + 1;

            for (int stack = 0; stack < stacks; ++stack) {
                for (int slice = 0; slice < slices; ++slice) {
                    indices[(numIndices - 1) - index++] = (stack + 0) * k + slice;
                    indices[(numIndices - 1) - index++] = (stack + 1) * k + slice;
                    indices[(numIndices - 1) - index++] = (stack + 0) * k + slice + 1;

                    indices[(numIndices - 1) - index++] = (stack + 0) * k + slice + 1;
                    indices[(numIndices - 1) - index++] = (stack + 1) * k + slice;
                    indices[(numIndices - 1) - index++] = (stack + 1) * k + slice + 1;
                }
            }

            vertexBuffer = new VertexBuffer(game.GraphicsDevice, typeof(VertexFormat), vertices.Length, BufferUsage.WriteOnly);
            vertexBuffer.SetData(vertices);
            indexBuffer = new IndexBuffer(game.GraphicsDevice, typeof(int), indices.Length, BufferUsage.WriteOnly);
            indexBuffer.SetData(indices);
		}

        public override void Draw() {
            game.GraphicsDevice.Indices = indexBuffer;
            game.GraphicsDevice.SetVertexBuffer(vertexBuffer);
            game.GraphicsDevice.DrawIndexedPrimitives(PrimitiveType.TriangleList, 0, 0, numVertices, 0, numPrimitives);
		}
    }
}

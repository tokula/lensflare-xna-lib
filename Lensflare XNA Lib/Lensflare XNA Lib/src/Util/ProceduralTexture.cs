using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System.Diagnostics;

namespace Util {
    public class ProceduralTextureBuilder {
        protected GraphicsDevice g;

        public ProceduralTextureBuilder(GraphicsDevice g) {
            this.g = g;
        }

        public Texture2D TextureFromColor(Color color) {
            Texture2D texture = new Texture2D(g, 1, 1);
            texture.SetData<Color>(new Color[] { color });
            return texture;
        }

        public Texture2D TextureFromBitmap(ref Color[,] colors) {
            int width = colors.GetLength(0);
            int height = colors.GetLength(1);
            Texture2D texture = new Texture2D(g, width, height);
            Color[] colors1D = new Color[width*height];
            int i = 0;
            for (int y = 0; y < height; ++y) {
                for (int x = 0; x < width; ++x) {
                    colors1D[i++] = colors[x,y];
                }
            }
            Debug.Assert(i == colors1D.Length);
            texture.SetData(colors1D);
            return texture;
        }

        public Texture2D Test(int width, int height) { //TODO: ...
            Texture2D texture = new Texture2D(g, width, height);
            Color[] colors = new Color[width * height];
            for (int y = 0; y < height; ++y) {
                for (int x = 0; x < width; ++x) {
                    float fx = x/(float)(Math.PI*2)/16;
                    float fy = y/(float)(Math.PI*2)/16;
                    float red = 0.5f*((float)Math.Sin(fx*fy)+1.0f);
                    float green = 0.5f * ((float)-Math.Sin(fx * fy) + 1.0f);
                    float blue = 0;
                    colors[x + y*width] = new Color(red, green, blue);
                }
            }
            texture.SetData(colors);
            return texture;
        }
    }
}

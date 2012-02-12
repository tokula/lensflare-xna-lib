using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System.Diagnostics;

namespace Util {
    public class TextureBuilder {
        protected GraphicsDevice g;

        public TextureBuilder(GraphicsDevice g) {
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

        public Texture2D EuclideanDistance(int width, int height, Color color) {
            Texture2D texture = new Texture2D(g, width, height);
            Color[] colors1D = new Color[width * height];
            int i = 0;
            int cx;
            int cy;
            for (int y = 0; y < height; ++y) {
                for (int x = 0; x < width; ++x) {
                    cx = x - width / 2;
                    cy = y - height / 2;
                    float alphaFactor = 1.0f - ((float)Math.Sqrt(cx * cx + cy * cy) / width * 2.0f);
                    colors1D[i++] = color * alphaFactor; 
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

        public Texture2D[,] Split(Texture2D texture, int count) {
            int width = texture.Width;
            int height = texture.Height;

            Debug.Assert(count >= 2 && count < width && count < height);

            Texture2D[,] textures = new Texture2D[count,count];
            for (int y = 0; y < count; ++y) {
                for (int x = 0; x < count; ++x) {
                    int splittedWidth = (width / count);
                    int splittedHeight = (height / count);
                    Color[] colorData = new Color[width * height];
                    texture.GetData<Color>(colorData);

                    Color[] splittedColorData = new Color[splittedWidth * splittedHeight];
                    for (int sy = 0; sy < splittedHeight; ++sy) {
                        for (int sx = 0; sx < splittedWidth; ++sx) {
                            int gx = x*splittedWidth+sx;
                            int gy = y*splittedHeight+sy;
                            splittedColorData[sy * splittedWidth + sx] = colorData[gy * width + gx];
                        }
                    }

                    Texture2D splittedTexture = new Texture2D(g, splittedWidth, splittedHeight);
                    splittedTexture.SetData<Color>(splittedColorData);

                    textures[x, y] = splittedTexture;
                }
            }
            return textures;
        }
    }
}

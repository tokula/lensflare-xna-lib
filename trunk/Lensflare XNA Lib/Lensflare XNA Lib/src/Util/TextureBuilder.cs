using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System.Diagnostics;

namespace Util {
    public class TextureColorMapper {
        public delegate Color MappingFunction(int x, int y);

        public IntVector2 Size { get; protected set; }
        Color[] colors1D;
        MappingFunction mappingFunction;

        public TextureColorMapper(IntVector2 size, MappingFunction mappingFunction) {
            Size = size;
            this.mappingFunction = mappingFunction;
            colors1D = new Color[size.X * size.Y];
        }

        public Texture2D BuildTexture(GraphicsDevice g) {
            Texture2D texture = new Texture2D(g, Size.X, Size.Y);

            int i = 0;
            for (int y = 0; y < Size.Y; ++y) {
                for (int x = 0; x < Size.X; ++x) {
                    colors1D[i++] = mappingFunction(x, y);
                }
            }
            Debug.Assert(i == colors1D.Length);
            texture.SetData(colors1D);

            return texture;
        }
    }

    public class TextureBuilder {
        protected GraphicsDevice g;

        public TextureBuilder(GraphicsDevice g) {
            this.g = g;
        }

        protected Color BlendColors(Color color1, Color color2, float blend) {
            return (color1 * blend).Sum(color2 * (1.0f-blend));
        }

        protected float HillFunction(float x, float bottomInside, float top, float bottomOutside, float increaseLength, float topLength, float decreaseLength) {
            float bottomLength = 1.0f - increaseLength - decreaseLength - topLength;

            float y;
            if (x < bottomLength) {
                y = bottomInside;
            } else if (x < bottomLength + increaseLength) {
                y = (x - bottomLength) * (top - bottomInside) / increaseLength + bottomInside;
            } else if (x < bottomLength + increaseLength + topLength) {
                y = top;
            } else if (x < bottomLength + increaseLength + topLength + decreaseLength) {
                y = (1.0f - (x - bottomLength - increaseLength - topLength) / decreaseLength) * (top - bottomOutside) + bottomOutside;
            } else {
                y = bottomOutside;
            }
            return y;
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
            return new TextureColorMapper(new IntVector2(width, height), (x, y) => {
                int cx = x - width / 2;
                int cy = y - height / 2;
                float alphaFactor = 1.0f - ((float)Math.Sqrt(cx * cx + cy * cy) / width * 2.0f);
                return color * alphaFactor; 
            }).BuildTexture(g);
        }

        public Texture2D Ring(int size, Color color, HillFunctionParameters fp) {
            float radius = size * 0.5f;
            return new TextureColorMapper(new IntVector2(size, size), (x, y) => {
                float distX = x - radius;
                float distY = y - radius;
                float dist = (float)Math.Sqrt(distX * distX + distY * distY) + 1;
                float alphaFactor = HillFunction(dist / radius, fp.bottomInside, fp.top, fp.bottomOutside, fp.increaseLength, fp.topLength, fp.decreaseLength);
                return color * alphaFactor;
            }).BuildTexture(g);
        }

        public Texture2D Sphere(int size, Color color) {
            float radius = size * 0.5f;
            float distTop = radius - 1;
            float distDecrease = 1.0f;

            return new TextureColorMapper(new IntVector2(size, size), (x, y) => {
                float distX = x - radius;
                float distY = y - radius;
                float dist = (float)Math.Sqrt(distX * distX + distY * distY) + 1;
                float normalizedDist = dist / radius;
                float brightness = (float)(Math.Acos(normalizedDist) / (Math.PI * 0.5));
                Color shadedColor = new Color(color.ToVector3() * brightness);

                float alphaFactor;
                if (dist <= distTop) {
                    alphaFactor = 1.0f;
                } else if (dist > distTop && dist <= distTop + distDecrease) {
                    alphaFactor = 1.0f - (dist - distTop) / distDecrease;
                } else {
                    alphaFactor = 0.0f;
                }

                return shadedColor * alphaFactor;
            }).BuildTexture(g);
        }

        public Texture2D VerticalGradient(int width, int height, Color colorTop, Color colorBottom) {
            return new TextureColorMapper(new IntVector2(width, height), (x, y) => {
                return BlendColors(colorTop, colorBottom, 1.0f - (float)y / (float)(height - 1));
            }).BuildTexture(g);
        }

        public Texture2D Bricks(int width, int height) {
            Random random = new Random();

            float brickW = 0.20f;
            float brickH = 0.08f;
            float mortarThickness = 0.02f;
            float bmW = brickW + mortarThickness;
            float bmH = brickH + mortarThickness;
            float mwf = mortarThickness * 0.5f / bmW;
            float mhf = mortarThickness * 0.5f / bmH;

            Color brickColor = new Color(0.5f, 0.15f, 0.14f);
            Color mortarColor = new Color(0.5f, 0.5f, 0.5f);
            Color mortarColor2 = new Color(0.3f, 0.15f, 0.15f);

            return new TextureColorMapper(new IntVector2(width, height), (x, y) => {
                float s = (float)x / (float)width;
                float t = (float)y / (float)height;
                //float scoord = s;
                //float tcoord = t;
                //Vector2 Nf = normalize(faceforward(N, I));
                float ss = s / bmW;
                float tt = t / bmH;
                if (MathProcedural.Mod(tt * 0.5f, 1.0f) > 0.5f) {
                    ss += 0.5f;
                }
                float sbrick = (float)Math.Floor(ss); /* which brick? */
                float tbrick = (float)Math.Floor(tt); /* which brick? */
                ss -= sbrick;
                tt -= tbrick;

                float w = MathProcedural.Step(mwf, ss) - MathProcedural.Step(1 - mwf, ss);
                float h = MathProcedural.Step(mhf, tt) - MathProcedural.Step(1 - mhf, tt);


                /* compute bump-mapping function for mortar grooves */
                float sbump = MathProcedural.SmoothStep(0, mwf, ss) - MathProcedural.SmoothStep(1 - mwf, 1, ss);
                float tbump = MathProcedural.SmoothStep(0, mhf, tt) - MathProcedural.SmoothStep(1 - mhf, 1, tt);
                float stbump = sbump * tbump;

                /* diffuse reflection model */
                //Oi = Os;
                //Ci = Os * Ct * (Ka * ambient() + Kd * diffuse(Nf));

                Color shadedMortarColor = mortarColor.Mix(mortarColor2, stbump);
                Color Ct = shadedMortarColor.Mix(brickColor, w * h);

                return Ct;
            }).BuildTexture(g);
        }

        public Texture2D Test(int width, int height) { //TODO: ...
            Random random = new Random();

            return new TextureColorMapper(new IntVector2(width, height), (x, y) => {
                float fx = x / (float)(Math.PI * 2) / 16;
                float fy = y / (float)(Math.PI * 2) / 16;
                float red = 0.5f * ((float)Math.Sin(fx * fy) + 1.0f);
                float green = 0.5f * ((float)-Math.Sin(fx * fy) + 1.0f);
                float blue = 0;
                return new Color(red, green, blue);
            }).BuildTexture(g);
        }

        public Texture2D[,] Split(Texture2D texture, int count) {
            int width = texture.Width;
            int height = texture.Height;

            Debug.Assert(count >= 2 && count < width && count < height);

            int splittedWidth = width / count;
            int splittedHeight = height / count;

            Texture2D[,] splittedTextures = new Texture2D[count, count];
            for (int y = 0; y < count; ++y) {
                for (int x = 0; x < count; ++x) {
                    Color[] colorData = new Color[width * height];
                    texture.GetData<Color>(colorData);

                    Color[] splittedColorData = new Color[splittedWidth * splittedHeight];
                    for (int splittedY = 0; splittedY < splittedHeight; ++splittedY) {
                        for (int splittedX = 0; splittedX < splittedWidth; ++splittedX) {
                            int textureX = x * splittedWidth + splittedX;
                            int textureY = y * splittedHeight + splittedY;
                            splittedColorData[splittedY * splittedWidth + splittedX] = colorData[textureY * width + textureX];
                        }
                    }

                    Texture2D splittedTexture = new Texture2D(g, splittedWidth, splittedHeight);
                    splittedTexture.SetData<Color>(splittedColorData);

                    splittedTextures[x, y] = splittedTexture;
                }
            }
            return splittedTextures;
        }
    }

    public struct HillFunctionParameters {
        public float bottomInside, top, bottomOutside, increaseLength, topLength, decreaseLength;
    }
}

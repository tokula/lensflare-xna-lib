using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Util {
    public static class Primitive2 {
        private static Texture2D pixel;

        public static void Init(GraphicsDevice graphicsDevice) {
            pixel = new Texture2D(graphicsDevice, 1, 1);
            UInt32[] colorData = new UInt32[1] { 0xFFFFFFFF };
            pixel.SetData<UInt32>(colorData);
        }

        public static void DrawPixel(SpriteBatch spriteBatch, Vector2 position, Color color, float layerDepth) {
            spriteBatch.Draw(pixel, position, null, color, 0, Vector2.Zero, 1, SpriteEffects.None, layerDepth);
        }

        public static void DrawHorizontalLine(SpriteBatch spriteBatch, Vector2 position, float length, Color color, float layerDepth) {
            spriteBatch.Draw(pixel, new Rectangle((int)position.X, (int)position.Y, (int)length, 1), null, color, 0, Vector2.Zero, SpriteEffects.None, layerDepth);
        }

        public static void DrawVerticalLine(SpriteBatch spriteBatch, Vector2 position, float length, Color color, float layerDepth) {
            spriteBatch.Draw(pixel, new Rectangle((int)position.X, (int)position.Y, 1, (int)length), null, color, 0, Vector2.Zero, SpriteEffects.None, layerDepth);
        }

        public static void DrawLine(SpriteBatch spriteBatch, Vector2 start, Vector2 end, Color color, float layerDepth) {
            int distance = (int)Vector2.Distance(start, end);
            float rotationAngle = (float)Math.Atan2(end.Y - start.Y, end.X - start.X);
            spriteBatch.Draw(pixel, new Rectangle((int)start.X, (int)start.Y, distance, 1), null, color, rotationAngle, new Vector2(0, 0), SpriteEffects.None, layerDepth);
        }

        public static void DrawTextureLine(SpriteBatch spriteBatch, Texture2D texture, Vector2 start, Vector2 end, float thickness, Color color, float layerDepth) {
            float distance = Vector2.Distance(start, end);
            float rotationAngle = (float)Math.Atan2(end.Y - start.Y, end.X - start.X);
            spriteBatch.Draw(texture, start, null, color, rotationAngle, new Vector2(0, 0.5f * texture.Height), new Vector2(distance / texture.Width, thickness / texture.Height), SpriteEffects.None, layerDepth);
            //spriteBatch.Draw(texture, new Rectangle((int)start.X, (int)start.Y, distance, (int)thickness), null, color, rotationAngle, new Vector2(0, 0.5f * texture.Height), SpriteEffects.None, layerDepth);
        }

        public static void DrawRect(SpriteBatch spriteBatch, Vector2 position, Vector2 size, Color color, bool filled, float layerDepth) {
            if (filled == false) {
                DrawHorizontalLine(spriteBatch, position, size.X, color, layerDepth);
                DrawHorizontalLine(spriteBatch, new Vector2(position.X, position.Y + size.Y - 1), size.X, color, layerDepth);
                DrawVerticalLine(spriteBatch, new Vector2(position.X, position.Y + 1), size.Y - 2, color, layerDepth);
                DrawVerticalLine(spriteBatch, new Vector2(position.X + size.X - 1, position.Y + 1), size.Y - 2, color, layerDepth);
            } else {
                spriteBatch.Draw(pixel, new Rectangle((int)position.X, (int)position.Y, (int)size.X, (int)size.Y), null, color, 0, Vector2.Zero, SpriteEffects.None, layerDepth);
            }
        }

        public static void DrawCircle(SpriteBatch spriteBatch, Vector2 position, float r, Color color, bool filled, float layerDepth) {
            if (filled == false) {
                int a = (int)Math.Sqrt(r * r / 2) + 1;
                for (int p = 0; p < a; ++p) {
                    int e = (int)Math.Sqrt(r * r - p * p);

                    spriteBatch.Draw(pixel, position + new Vector2( p,  e), null, color, 0, Vector2.Zero, 1, SpriteEffects.None, layerDepth);
                    spriteBatch.Draw(pixel, position + new Vector2( p, -e), null, color, 0, Vector2.Zero, 1, SpriteEffects.None, layerDepth);
                    spriteBatch.Draw(pixel, position + new Vector2( e,  p), null, color, 0, Vector2.Zero, 1, SpriteEffects.None, layerDepth);
                    spriteBatch.Draw(pixel, position + new Vector2(-e,  p), null, color, 0, Vector2.Zero, 1, SpriteEffects.None, layerDepth);

                    spriteBatch.Draw(pixel, position + new Vector2(-p,  e), null, color, 0, Vector2.Zero, 1, SpriteEffects.None, layerDepth);
                    spriteBatch.Draw(pixel, position + new Vector2(-p, -e), null, color, 0, Vector2.Zero, 1, SpriteEffects.None, layerDepth);
                    spriteBatch.Draw(pixel, position + new Vector2( e, -p), null, color, 0, Vector2.Zero, 1, SpriteEffects.None, layerDepth);
                    spriteBatch.Draw(pixel, position + new Vector2(-e, -p), null, color, 0, Vector2.Zero, 1, SpriteEffects.None, layerDepth);
                }
            } else {
                int a = (int)r + 1;
                for (int p = 0; p < a; ++p) {
                    int e = (int)Math.Sqrt(r * r - p * p);
                    if (p > 0) {
                        spriteBatch.Draw(pixel, new Rectangle((int)position.X + p, (int)position.Y - e, 1, e * 2 + 1), null, color, 0, Vector2.Zero, SpriteEffects.None, layerDepth);
                    }
                    spriteBatch.Draw(pixel, new Rectangle((int)position.X - p, (int)position.Y - e, 1, e * 2 + 1), null, color, 0, Vector2.Zero, SpriteEffects.None, layerDepth);
                }
            }
        }
    }
}

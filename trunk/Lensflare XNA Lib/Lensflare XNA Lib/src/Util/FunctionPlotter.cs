using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Util {
    public class ColoredFunction {
        public delegate float Function(float x);
        public Function F { get; set; }
        public Color Color { get; set; }

        public ColoredFunction(Color color, Function f) {
            F = f;
            Color = color;
        }
    }

    public class FunctionPlotter {
        public SpriteBatch SpriteBatch { get; set; }
        public Vector2 Size { get; set; }
        public Vector2 Origin { get; set; }
        public Vector2 PosOnScreen { get; set; }
        public float Scale { get; set; }
        public Color BorderColor { get; set; }
        public Color AxesColor { get; set; }

        public ColoredFunction[] Functions { get; set; }

        public FunctionPlotter(SpriteBatch spriteBatch, ColoredFunction[] functions) {
            SpriteBatch = spriteBatch;
            Functions = functions;

            Size = new Vector2(4, 3);
            Scale = 200.0f;
            BorderColor = Color.Black * 0.5f;
            AxesColor = Color.White * 0.8f;

            Origin = Vector2.One * 1.0f;
        }

        public void Draw() {
            const float layerDepth = 1.0f;
            Vector2 hswap = new Vector2(1, -1);

            Vector2 OriginP = PosOnScreen + (new Vector2(0, Size.Y) + Origin * hswap) * Scale;

            //border
            Primitive2.DrawRect(SpriteBatch, PosOnScreen, Size * Scale + Vector2.One, BorderColor, true, layerDepth);

            //axes
            Primitive2.DrawHorizontalLine(SpriteBatch, PosOnScreen + new Vector2(0, Size.Y) * Scale + new Vector2(0, -Origin.Y) * Scale, Size.X * Scale, AxesColor, layerDepth);
            Primitive2.DrawVerticalLine(SpriteBatch, PosOnScreen + new Vector2(Origin.X, 0) * Scale, Size.Y * Scale, AxesColor, layerDepth);

            //unit markers
            Primitive2.DrawHorizontalLine(SpriteBatch, OriginP + new Vector2(0, -1) * Scale + new Vector2(-2, 0), 5, AxesColor, layerDepth);
            Primitive2.DrawVerticalLine(SpriteBatch, OriginP + new Vector2(1, 0) * Scale + new Vector2(0, -2), 5, AxesColor, layerDepth);

            //the function graph
            if (Functions != null) {
                foreach (ColoredFunction f in Functions) {
                    Vector2? pLast = null;
                    for (int xs = 0; xs < Size.X * Scale; ++xs) {
                        float x = xs / Scale - Origin.X;
                        float y = f.F(x);

                        Vector2 p = OriginP + new Vector2(x, y) * Scale * hswap;
                        if (pLast.HasValue) {
                            Vector2 delta = p - pLast.Value;
                            float length = delta.Length() + 0.001f;
                            delta.Normalize();
                            delta = delta * length;
                            Vector2 pOverlap = p + delta;

                            Primitive2.DrawLine(SpriteBatch, pLast.Value, pOverlap, f.Color, layerDepth);
                        }
                        pLast = p;
                    }
                }
            }
        }
    }
}

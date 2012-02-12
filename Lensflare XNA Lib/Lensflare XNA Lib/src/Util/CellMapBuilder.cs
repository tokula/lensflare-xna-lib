using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Util {
    public static class Vector2Extension {
        public static void Set(this Vector2 v, float x, float y) {
            v.X = x;
            v.Y = y;
        }
    }

    public class CellMapBuilder {

        Random random;

        public enum BitmapGenerationMode {
            Random,
        }

        public CellMapBuilder(Random random) {
            this.random = random;
        }

        public bool[,] MakeBitmap(int width, int height, BitmapGenerationMode mode) {
            bool[,] bitmap = new bool[width, height];
            for (int y = 0; y < height; ++y) {
                for (int x = 0; x < width; ++x) {
                    bitmap[x, y] = random.NextDouble() < 0.5;
                }
            }
            return bitmap;
        }

        public float[] MakeBorders(bool[,] bitmap, float scale) {
            int width = bitmap.GetLength(0);
            int height = bitmap.GetLength(1);

            LinkedList<float> a = new LinkedList<float>();

            for (int y = 0; y < height; ++y) {
                for (int x = 0; x < width; ++x) {
                    if (bitmap[x, y]) {
                        if (x == 0 || !bitmap[x - 1, y]) {
                            a.AddLast(x);
                            a.AddLast(y);
                            a.AddLast(x);
                            a.AddLast(y + 1);
                        }
                        if (x == width - 1 || !bitmap[x + 1, y]) {
                            a.AddLast(x + 1);
                            a.AddLast(y);
                            a.AddLast(x + 1);
                            a.AddLast(y + 1);
                        }
                        if (y == 0 || !bitmap[x, y - 1]) {
                            a.AddLast(x);
                            a.AddLast(y);
                            a.AddLast(x + 1);
                            a.AddLast(y);
                        }
                        if (y == height - 1 || !bitmap[x, y + 1]) {
                            a.AddLast(x);
                            a.AddLast(y + 1);
                            a.AddLast(x + 1);
                            a.AddLast(y + 1);
                        }
                    }
                }
            }

            float[] aScaled = new float[a.Count];

            int i = 0;
            foreach(float value in a) {
                aScaled[i++] = value * scale;
            }

            return aScaled;
        }

        /**
         * Returns an array of Vector2 objects where the first dimension is the pair index and the second dimension is the Vector2 index in the pair, which is 0 or 1. 
         */
        public Vector2[,] VectorPairsArray(float[] values) {
            Vector2[,] vectorPairs = new Vector2[values.Length/4,2];
            for (int i = 0; i < values.Length; i += 4) {
                float x0 = values[i];
                float y0 = values[i + 1];
                float x1 = values[i + 2];
                float y1 = values[i + 3];
                vectorPairs[i / 4, 0].Set(x0, y0);
                vectorPairs[i / 4, 1].Set(x1, y1);
            }
            return vectorPairs;
        }

        /**
         * Returns a linked list of Vector2 arrays where each array represents a Vector2 pair and contains exactly 2 Vector2 objects. 
         */
        public LinkedList<Vector2[]> VectorPairsList(float[] values) {
            LinkedList<Vector2[]> vectorPairs = new LinkedList<Vector2[]>();
            for (int i = 0; i < values.Length; i += 4) {
                float x0 = values[i];
                float y0 = values[i + 1];
                float x1 = values[i + 2];
                float y1 = values[i + 3];
                vectorPairs.AddLast(new Vector2[]{new Vector2(x0, y0), new Vector2(x1, y1)});
            }
            return vectorPairs;
        }
    }
}

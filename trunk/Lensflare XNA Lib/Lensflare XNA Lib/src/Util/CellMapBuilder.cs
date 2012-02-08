using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Util {
    public class CellMapBuilder {

        Random random;

        public CellMapBuilder(Random random) {
            this.random = random;
        }

        public float[] Test(int width, int height, float scale) {
            bool[,] bitmap = new bool[width,height];
            for (int y = 0; y < height; ++y) {
                for (int x = 0; x < width; ++x) {
                    bitmap[x, y] = random.NextDouble() < 0.5;
                }
            }

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
    }
}

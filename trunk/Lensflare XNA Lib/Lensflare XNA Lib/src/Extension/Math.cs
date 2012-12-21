using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Util {
    public static class MathProcedural {
        public static float Step(float a, float x)  {
            return x >= a ? 1.0f : 0.0f;
        }

        public static float Pulse(float a, float b, float x) {
            return Step(a, x) - Step(b, x);
        }

        public static float SmoothStep(float a, float b, float x) {
            float xs = (x-a)/(b-a);
            return x < a ? 0.0f : x >= b ? 1.0f : (xs*xs*(3.0f-2.0f*xs));
        }

        public static float Mod(float x, float a) {
            int n = (int)(x/a);
            x -= n*a;
            if(x < 0) {
                x += a;
            }
            return x; 
        }

        public static float GammaCorrect(float gamma, float x) {
            return (float)Math.Pow(x, 1 / gamma);
        }

        public static float Bias(float bias, float x) {
            return (float)Math.Pow(x, Math.Log(bias)/Math.Log(0.5));
        }

        public static float Gain(float gain, float x) {
            return x < 0.5f ? Bias(1.0f-gain, 2.0f*x)*0.5f : 1.0f - Bias(1.0f-gain, 2.0f-2.0f*x)*0.5f;
        }
    }
}

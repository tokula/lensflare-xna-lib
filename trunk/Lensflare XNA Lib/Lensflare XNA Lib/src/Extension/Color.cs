using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Util {
    public static class ColorExtension {
        public static Color Sum(this Color color0, Color color1) {
            return new Color(color0.ToVector4() + color1.ToVector4());
        }

        public static Color Mix(this Color color0, Color color1, float mix) {
            return Sum(color0 * (1.0f - mix), color1 * mix);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Util {
    public static class ColorExtension {
        public static Color Sum(this Color color1, Color color2) {
            return new Color(color1.ToVector4() + color2.ToVector4());
        }
    }
}

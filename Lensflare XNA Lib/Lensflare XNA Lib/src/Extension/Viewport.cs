using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Util {
    public static class ViewportExtension {
        public static IntVector2 GetSize(this Viewport viewport) {
            return new IntVector2(viewport.Width, viewport.Height);
        }

        public static Vector2 GetCenter(this Viewport viewport) {
            return new Vector2(viewport.Width * 0.5f, viewport.Height * 0.5f);
        }
    }
}

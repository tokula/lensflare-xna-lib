using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Util {
    public static class Texture2DExtension {
        public static IntVector2 GetSize(this Texture2D texture) {
            return new IntVector2(texture.Width, texture.Height);
        }

        public static Vector2 GetCenter(this Texture2D texture) {
            return new Vector2(texture.Width * 0.5f, texture.Height * 0.5f);
        }
    }
}

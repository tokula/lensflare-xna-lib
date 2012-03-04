using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace LensflareGameFramework {
    public static class GraphicsDeviceManagerExtension {
        public static void ApplyResolution(this GraphicsDeviceManager gdm, int width, int height, bool fullscreen) {
            gdm.IsFullScreen = fullscreen;
            gdm.PreferredBackBufferWidth = width;
            gdm.PreferredBackBufferHeight = height;
            gdm.ApplyChanges();
        }
    }
}

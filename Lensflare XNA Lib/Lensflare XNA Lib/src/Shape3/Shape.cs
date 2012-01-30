using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Shape3 {
    public abstract class Shape {
        protected Game game;

        public abstract int NumVertices { get; }
        public abstract int NumPrimitives { get; }

        public abstract void Draw();
    }
}

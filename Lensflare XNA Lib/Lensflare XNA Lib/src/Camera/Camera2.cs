using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Camera {
    public class Camera2 {
        protected Vector2 position;
        protected Vector2 size;

        public Vector2 Position { get { return position; } set { position = value; } }
        public Vector2 Size { get { return size; } set { size = value; } }

        public Camera2() {
        }

        public virtual void Update(GameTime gameTime) {

        }
    }
}

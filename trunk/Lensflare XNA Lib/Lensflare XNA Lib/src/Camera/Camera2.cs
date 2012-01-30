using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Camera {
    public class Camera2 {
        protected Vector2 positionWorld = Vector2.Zero;
        protected Vector2 positionScreen = Vector2.Zero;
        protected Vector2 size = new Vector2(100, 100);

        public Vector2 PositionWorld { get { return positionWorld; } set { positionWorld = value; } }
        public Vector2 PositionScreen { get { return positionScreen; } set { positionScreen = value; } }
        public Vector2 Size { get { return size; } set { size = value; } }

        public Camera2() {
        }

        public virtual void Update(GameTime gameTime) {

        }
    }
}

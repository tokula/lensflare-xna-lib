using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Camera {
    class SmoothCamera2 : Camera2 {
        protected Vector2 velocity = Vector2.Zero;
        protected float dampingFactor = 0.9f;

        public Vector2 Velocity { get { return velocity; } set { velocity = value; } }
        public float DampingFactor { get { return dampingFactor; } set { dampingFactor = value; } }

        public SmoothCamera2() : base() {
        }

        public override void Update(GameTime gameTime) {
            float elapsedSeconds = (float)gameTime.ElapsedGameTime.TotalSeconds;
            velocity *= (float)Math.Pow(dampingFactor * 0.01f, elapsedSeconds);
            position += velocity * elapsedSeconds;
            base.Update(gameTime);
        }
    }
}

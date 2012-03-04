using Microsoft.Xna.Framework;
using System;

namespace Camera {
    public class SmoothCamera3 : Camera3 {
        protected Vector3 velocity = Vector3.Zero;
        protected float dampingFactor = 0.9f;

        public Vector3 Velocity { get { return velocity; } set { velocity = value; } }

        public float DampingFactor {
            get { return dampingFactor; }
            set { dampingFactor = value; }
        }

        public SmoothCamera3() : base() {
        }

        public override void Update(GameTime gameTime) {
            base.Update(gameTime);
            float elapsedSeconds = (float)gameTime.ElapsedGameTime.TotalSeconds;
            velocity *= (float)Math.Pow(dampingFactor*0.01f, elapsedSeconds);
            position += velocity * elapsedSeconds;
        }
    }
}

using Microsoft.Xna.Framework;

namespace Util {
    public class SmoothCamera : Camera {
        protected Vector3 velocity = Vector3.Zero;
        protected float dampingFactor = 0.85f;

        public Vector3 Velocity { get { return velocity; } set { velocity = value; } }

        public float DampingFactor {
            get { return dampingFactor; }
            set { dampingFactor = value; }
        }

        public SmoothCamera()
            : base() {
        }

        public override void Update(GameTime gameTime) {
            velocity *= dampingFactor;
            position += velocity;
            base.Update(gameTime);
        }
    }
}

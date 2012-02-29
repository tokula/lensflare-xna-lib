using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Camera {
    public class SmoothCamera2 : Camera2 {
        public Vector2 Velocity { get; set; }
        public float DampingFactor { get; set; }

        public SmoothCamera2() : base() {
            DampingFactor = 0.9f;
        }

        public override void Update(GameTime gameTime) {
            float elapsedSeconds = (float)gameTime.ElapsedGameTime.TotalSeconds;
            Velocity *= (float)Math.Pow(DampingFactor * 0.01f, elapsedSeconds);
            PositionWorld += Velocity * elapsedSeconds;
            base.Update(gameTime);
        }
    }
}

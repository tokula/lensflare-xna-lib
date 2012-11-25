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

        //TODO: zoom velocity and damping for smooth zooming

        public override void Update(GameTime gameTime) {
            base.Update(gameTime);
            float elapsedSeconds = (float)gameTime.ElapsedGameTime.TotalSeconds;
            Velocity *= (float)Math.Pow(DampingFactor * 0.01f, elapsedSeconds);
            PositionWorld += Velocity * elapsedSeconds;
        }
    }
}

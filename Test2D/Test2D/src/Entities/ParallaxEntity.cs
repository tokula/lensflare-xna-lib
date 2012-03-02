using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Collision.Shapes;
using Microsoft.Xna.Framework;
using Util;
using Microsoft.Xna.Framework.Graphics;
using Camera;

namespace Test2D {
    class ParallaxEntity : GameEntity {
        public Texture2D texture1 = null;
        public Texture2D texture2 = null;

        public ParallaxEntity(Game2D game) : base(game) {

        }

        protected override void Dispose() {
            base.Dispose();
        }

        public override void Update(GameTime gameTime) {

        }

        public override void Draw() {
            Camera2 camera = Game.camera;
            Game.spriteBatch.Draw(texture1, camera.ScreenPointFromWorldPoint(Vector2.Zero, 0.5f), Color.White);
            Game.spriteBatch.Draw(texture2, camera.ScreenPointFromWorldPoint(Vector2.Zero, 0.75f), Color.White);
        }
    }
}

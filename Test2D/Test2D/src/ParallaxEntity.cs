using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Collision.Shapes;
using Microsoft.Xna.Framework;
using Util;
using Microsoft.Xna.Framework.Graphics;

namespace Test2D {
    class ParallaxEntity : GameEntity {
        public Texture2D texture1;
        public Texture2D texture2;

        public ParallaxEntity(Game2D game) : base(game) {

        }

        protected override void Dispose() {
            base.Dispose();
        }

        public override void Update(GameTime gameTime) {

        }

        public override void Draw() {
            Game.spriteBatch.Draw(texture1, Game.camera.PositionScreen - Game.camera.PositionWorld * 0.5f, Color.White);
            Game.spriteBatch.Draw(texture2, Game.camera.PositionScreen - Game.camera.PositionWorld * 1.0f, Color.White);
        }
    }
}

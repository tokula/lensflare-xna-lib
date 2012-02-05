using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Util;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Collision.Shapes;

namespace Test2D {
    class GroundEntity : GameEntity {
        Body body;
        EdgeShape shape;
        Fixture fixture;

        public GroundEntity(Game2D game) : base(game) {
            body = new Body(game.world);
            body.BodyType = BodyType.Static;

            shape = new EdgeShape(new Vector2(-200, 100), new Vector2(300, 150));
            fixture = new Fixture(body, shape);
        }

        protected override void Dispose() {
            base.Dispose();
        }

        public override void Update(GameTime gameTime) {

        }

        public override void Draw() {
            Vector2 screenPosition = Game.camera.PositionScreen - Game.camera.PositionWorld;
            Primitive2.DrawLine(Game.spriteBatch, screenPosition + shape.Vertex1, screenPosition + shape.Vertex2, Color.DarkGreen);
        }
    }
}

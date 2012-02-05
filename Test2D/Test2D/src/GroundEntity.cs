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

            shape = new EdgeShape(new Vector2(10, 300), new Vector2(500, 350));
            fixture = new Fixture(body, shape);
        }

        protected override void Dispose() {
            base.Dispose();
        }

        public override void Update(GameTime gameTime) {

        }

        public override void Draw() {
            Primitive2.DrawLine(Game.spriteBatch, shape.Vertex1, shape.Vertex2, Color.DarkGreen);
        }
    }
}

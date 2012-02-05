using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using LensflareGameFramework;
using Util;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Collision.Shapes;

namespace Test2D {
    class TestEntity : GameEntity {
        Body body;
        CircleShape shape;
        Fixture fixture;

        public TestEntity(Game2D game, Vector2 position, float radius, float density) : base(game) {
            body = new Body(game.world);
            body.BodyType = BodyType.Dynamic;
            body.Position = position;
            //body.SleepingAllowed = false;
            body.AngularDamping = 0;
            body.LinearDamping = 0;

            shape = new CircleShape(radius, density);

            fixture = new Fixture(body, shape);
            fixture.Restitution = 0.5f;
        }

        protected override void Dispose() {
            base.Dispose();
        }

        public override void Update(GameTime gameTime) {

        }

        public override void Draw() {
            Primitive2.DrawCircle(Game.spriteBatch, body.Position, shape.Radius, new Color(1.0f, 0.5f, 0.0f, 0.5f), true);
        }
    }
}

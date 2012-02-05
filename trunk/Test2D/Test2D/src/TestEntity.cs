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

            shape = new CircleShape(radius, density);
            fixture = new Fixture(body, shape);

            //physicsEntities.Add(new BEPUphysics.Entities.Prefabs.Sphere(position, radius, mass));
            //game.space.Add(physicsEntities[0]);
        }

        protected override void Dispose() {
            base.Dispose();
        }

        public override void Update(GameTime gameTime) {
            /*float elapsedSeconds = (float)gameTime.ElapsedGameTime.TotalSeconds;
            tempVariable += elapsedSeconds*5;
            tempVariableB += elapsedSeconds*2;
            position.X += 3.0f * (float)Math.Sin((double)tempVariable);
            position.Y += 1.0f * (float)Math.Sin((double)tempVariableB);
            radius += 0.7f * (float)Math.Sin((double)tempVariableB);*/
        }

        public override void Draw() {
            Primitive2.DrawCircle(Game.spriteBatch, body.Position, shape.Radius, new Color(1.0f, 0.5f, 0.0f, 0.5f), false);
        }
    }
}

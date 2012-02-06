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
        public Texture2D texture;

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
            Vector2 screenPosition = Game.camera.PositionScreen - Game.camera.PositionWorld;
            //Primitive2.DrawCircle(Game.spriteBatch, screenPosition + body.Position, shape.Radius, new Color(1.0f, 0.5f, 0.0f, 0.5f), true);
            Vector2 position = body.Position + screenPosition;
            Rectangle rect = texture.Bounds;
            Vector2 origin = new Vector2(texture.Bounds.Center.X, texture.Bounds.Center.Y);
            float scale = 2.0f * shape.Radius / texture.Bounds.Height;
            Game.spriteBatch.Draw(texture, position, rect, Color.White, body.Rotation, origin, scale, SpriteEffects.None, 1.0f);
        }
    }
}

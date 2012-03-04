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
using FarseerPhysics.Collision;
using FarseerPhysics.Common;
using Camera;

namespace Test2D {
    class TestEntity : GameEntity {
        Body body;
        CircleShape shape;
        Fixture fixture;
        public Texture2D texture;

        public TestEntity(Game2D game, Vector2 position, float radius, float density) : base(game) {
            body = new Body(game.World);
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
            Camera2 camera = Game.Camera;
            Vector2 position = camera.Project(body.Position);
            Vector2 origin = texture.GetCenter();
            float scale = 2.0f * shape.Radius / texture.Bounds.Height;

            if (camera.OverlapsWorldShape(shape, body)) {
                Game.SpriteBatch.Draw(texture, position, null, Color.White, body.Rotation, origin, scale * camera.Zoom, SpriteEffects.None, Game.LayerManager.Depth((int)MainLayer.DynamicEntity));
            }
        }
    }
}

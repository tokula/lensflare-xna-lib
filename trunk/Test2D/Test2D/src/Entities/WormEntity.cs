using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Collision.Shapes;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Camera;
using Util;
using LensflareGameFramework;
using FarseerPhysics.Dynamics.Joints;

namespace Test2D {
    public class WormEntity : GameEntity {
        Body body;
        CircleShape shape;
        Fixture fixture;
        public Texture2D texture;
        FixedDistanceJoint ropeJoint;

        public WormEntity(Game2D game, Vector2 position) : base(game) {
            body = new Body(game.World);
            body.BodyType = BodyType.Dynamic;
            body.Position = position;
            //body.SleepingAllowed = false;
            body.AngularDamping = 0;
            body.LinearDamping = 0;

            shape = new CircleShape(15.0f, 1.0f);

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

            if (ropeJoint != null) {
                if (camera.OverlapsWorldLine(ropeJoint.WorldAnchorA, ropeJoint.WorldAnchorB)) {
                    Primitive2.DrawTextureLine(Game.SpriteBatch, Game.lineTexture, camera.Project(ropeJoint.WorldAnchorA), camera.Project(ropeJoint.WorldAnchorB), 10.0f, Color.White, Game.LayerManager.Depth((int)MainLayer.DynamicEntity));
                }
            }
        }

        public void AccelerateBy(Vector2 v) {
            body.ApplyForce(v);
        }

        public void ToggleRope() {
            Vector2? hitPosition = null;
            Fixture hitFixture = null;
            float minFrac = float.MaxValue;
            Game.World.RayCast((f, p, n, fr) => {
                    bool fixtureShouldBeIgnored = false;
                    if (!fixtureShouldBeIgnored) {
                        if (fr < minFrac) {
                            minFrac = fr;
                            hitPosition = p;
                            hitFixture = f;
                        }
                        return 1.0f;
                    } else {
                        return -1.0f;
                    }
                },                 
                body.Position,
                Game.Camera.Unproject(Input.MousePosition));

            if (ropeJoint == null) {
                if (hitPosition != null) {
                    //TestEntity testEntity = new TestEntity(Game, hitPosition.Value, 5.0f, 1.0f);
                    //testEntity.texture = texture;
                    //Entity.Add(testEntity);

                    ropeJoint = new FixedDistanceJoint(body, Vector2.Zero, hitPosition.Value);
                    //ropeJoint = new FixedLineJoint(body, hitPosition.Value, hitPosition.Value - body.Position);
                    Game.World.AddJoint(ropeJoint);
                }
            } else {
                Game.World.RemoveJoint(ropeJoint);
                ropeJoint = null;
            }
        }
    }
}

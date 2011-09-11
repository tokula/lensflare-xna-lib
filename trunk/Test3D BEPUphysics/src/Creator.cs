using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using LensflareGameFramework;

namespace EngineTest {
    class Creator : GameEntity {
        protected Shape3D.Sphere visualSphere;
        protected BEPUphysics.Entities.Prefabs.Sphere physicsSphere;

        public Vector3 Position { get { return physicsSphere.Position; } }
        public Vector3 Velocity { get { return physicsSphere.LinearVelocity; } set { physicsSphere.LinearVelocity = value; } }
        public Vector3 AngularVelocity { get { return physicsSphere.AngularVelocity; } set { physicsSphere.AngularVelocity = value; } }

        public Creator(Game1 game, Vector3 position, float radius, float mass, Texture2D texture) : base(game, texture) {
            visualSphere = new Shape3D.Sphere(game, radius, 128, 128);
			if (mass < 0) {
                physicsSphere = new BEPUphysics.Entities.Prefabs.Sphere(position, radius);
			} else {
                physicsSphere = new BEPUphysics.Entities.Prefabs.Sphere(position, radius, mass);
			}
            physicsSphere.LinearDamping = 0.5f;
            physicsSphere.CollisionInformation.Events.InitialCollisionDetected += game.HandleCollision;
            game.space.Add(physicsSphere);
		}

        protected override void Dispose() {
            physicsSphere.CollisionInformation.Events.RemoveAllEvents();
            game.space.Remove(physicsSphere);
            visualSphere = null;
            physicsSphere = null;
            base.Dispose();
        }

		public override void Update(GameTime gameTime) {

        }

		public override void Draw() {
            DrawShape(physicsSphere.WorldTransform, visualSphere);
		}
    }
}

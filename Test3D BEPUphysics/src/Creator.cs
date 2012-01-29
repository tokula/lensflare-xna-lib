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

        public Vector3 Position { get { return physicsEntities[0].Position; } }
        public Vector3 Velocity { get { return physicsEntities[0].LinearVelocity; } set { physicsEntities[0].LinearVelocity = value; } }
        public Vector3 AngularVelocity { get { return physicsEntities[0].AngularVelocity; } set { physicsEntities[0].AngularVelocity = value; } }

        public Creator(Game3D game, Vector3 position, float radius, float mass, Texture2D texture) : base(game, texture) {
            visualSphere = new Shape3D.Sphere(game, radius, 128, 128);
			if (mass < 0) {
                physicsEntities.Add(new BEPUphysics.Entities.Prefabs.Sphere(position, radius));
			} else {
                physicsEntities.Add(new BEPUphysics.Entities.Prefabs.Sphere(position, radius, mass));
			}
            //physicsEntities[0].LinearDamping = 0.5f;
            physicsEntities[0].LinearDamping = 0;
            physicsEntities[0].AngularDamping = 0;
            physicsEntities[0].CollisionInformation.Events.InitialCollisionDetected += game.HandleCollision;
            game.space.Add(physicsEntities[0]);
		}

        protected override void Dispose() {
            visualSphere = null;
            base.Dispose();
        }

		public override void Update(GameTime gameTime) {

        }

		public override void Draw() {
            DrawShape(physicsEntities[0].WorldTransform, visualSphere);
		}
    }
}

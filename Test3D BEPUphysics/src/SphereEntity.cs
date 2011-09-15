using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using LensflareGameFramework;

namespace EngineTest {
    class SphereEntity : GameEntity {
		Shape3D.Sphere visualSphere;

        public SphereEntity(Game1 game, Vector3 position, float radius, float mass, Texture2D texture) : base(game, texture) {
            visualSphere = new Shape3D.Sphere(game, radius, 128, 128);
			if (mass < 0) {
                physicsEntities.Add(new BEPUphysics.Entities.Prefabs.Sphere(position, radius));
			} else {
                physicsEntities.Add(new BEPUphysics.Entities.Prefabs.Sphere(position, radius, mass));
			}
            physicsEntities[0].CollisionInformation.Events.InitialCollisionDetected += game.HandleCollision;
            physicsEntities[0].LinearDamping = 0;
            physicsEntities[0].AngularDamping = 0;
            game.space.Add(physicsEntities[0]);
		}

        protected override void Dispose() {
            visualSphere = null;
            base.Dispose();
        }

		public override void Update(GameTime gameTime) {
            foreach (var e in physicsEntities) {
                if (e.Position.Y < -1000) {
                    Entity.Remove(this);
                    return;
                }
            }
		}

		public override void Draw() {
            DrawShape(physicsEntities[0].WorldTransform, visualSphere);
		}
    }
}

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
        public BEPUphysics.Entities.Prefabs.Sphere physicsSphere;

        public SphereEntity(Game1 game, Vector3 position, float radius, float mass, Texture2D texture) : base(game, texture) {
            visualSphere = new Shape3D.Sphere(game, radius, 128, 128);
			if (mass < 0) {
                physicsSphere = new BEPUphysics.Entities.Prefabs.Sphere(position, radius);
			} else {
                physicsSphere = new BEPUphysics.Entities.Prefabs.Sphere(position, radius, mass);
			}
            physicsSphere.CollisionInformation.Events.InitialCollisionDetected += game.HandleCollision;
            game.space.Add(physicsSphere);
		}

        protected override void Dispose() {
            physicsSphere.CollisionInformation.Events.RemoveAllEvents();
            Game.space.Remove(physicsSphere);
            visualSphere = null;
            physicsSphere = null;
            base.Dispose();
        }

		public override void Update(GameTime gameTime) {
            if (physicsSphere.Position.Y < -100) {
                Entity.Remove(this);
            }
		}

		public override void Draw() {
            DrawShape(physicsSphere.WorldTransform, visualSphere);
		}
    }
}

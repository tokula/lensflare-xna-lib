using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using LensflareGameFramework;

namespace EngineTest {
	class BoxEntity : GameEntity {
        protected Shape3D.Box visualBox;

        public BoxEntity(Game3D game, Vector3 position, Vector3 size, float mass, Texture2D texture) : base(game, texture) {
            visualBox = new Shape3D.Box(game, size);
            Vector3 physicalSize = size * 2;
			if (mass < 0) {
				physicsEntities.Add(new BEPUphysics.Entities.Prefabs.Box(position, physicalSize.X, physicalSize.Y, physicalSize.Z));
			} else {
                physicsEntities.Add(new BEPUphysics.Entities.Prefabs.Box(position, physicalSize.X, physicalSize.Y, physicalSize.Z, mass));
			}
            physicsEntities[0].CollisionInformation.Events.InitialCollisionDetected += game.HandleCollision;
            physicsEntities[0].LinearDamping = 0;
            physicsEntities[0].AngularDamping = 0;
            game.space.Add(physicsEntities[0]);
		}

        protected override void Dispose() {
            visualBox = null;
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
            DrawShape(physicsEntities[0].WorldTransform, visualBox);
		}
	}
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using LensflareGameFramework;

namespace EngineTest {
    class ForceEntity : GameEntity {
		protected Shape3D.Sphere visualSphere;
        public float Force { get; set; }

        public ForceEntity(Game1 game, Vector3 position, float radius, Texture2D texture, float force) : base(game, texture) {
            visualSphere = new Shape3D.Sphere(game, radius, 64, 64);
            Force = force;
            physicsEntities.Add(new BEPUphysics.Entities.Prefabs.Sphere(position, radius));
            physicsEntities[0].CollisionInformation.Events.InitialCollisionDetected += game.HandleCollision;
            game.space.Add(physicsEntities[0]);
		}

        protected override void Dispose() {
            visualSphere = null;
            base.Dispose();
        }

		public override void Update(GameTime gameTime) {
            foreach (var entity in Entity.All) {
                if (entity != this) {
                    if (entity is Creator) {
                        var e = (Creator)entity;
                        foreach(var physicsEntity in e.PhysicsEntities) {
                            Vector3 posDiff = this.physicsEntities[0].Position - physicsEntity.Position;
                            float distanceSquared = posDiff.LengthSquared();
                            posDiff.Normalize();
                            physicsEntity.LinearVelocity += posDiff / (distanceSquared * Force);
                        }
                    }
                }
            }
		}

		public override void Draw() {
            DrawShape(physicsEntities[0].WorldTransform, visualSphere);
		}
    }
}

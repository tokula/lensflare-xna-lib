using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using LensflareGameFramework;

namespace EngineTest {
    class ForceEntity : GameEntity {
		protected Shape3.Sphere visualSphere;
        public float Force { get; set; }
        public Vector3 Position { get { return physicsEntities[0].Position; } }
        public float Radius { get { return ((BEPUphysics.Entities.Prefabs.Sphere)physicsEntities[0]).Radius; } }

        public ForceEntity(Game3D game, Vector3 position, float radius, float mass, Texture2D texture, float force) : base(game, texture) {
            visualSphere = new Shape3.Sphere(game, radius, 64, 64);
            Force = force;
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
            foreach (var entity in Entity.All) {
                if (entity != this) {
                    if (entity is GameEntity) {
                        var e = (GameEntity)entity;
                        foreach(var physicsEntity in e.PhysicsEntities) {
                            if (physicsEntity.IsDynamic) {
                                Vector3 posDiff = this.physicsEntities[0].Position - physicsEntity.Position;
                                float distanceSquared = posDiff.LengthSquared();
                                posDiff.Normalize();
                                physicsEntity.LinearVelocity += Force * (posDiff / distanceSquared);
                            }
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

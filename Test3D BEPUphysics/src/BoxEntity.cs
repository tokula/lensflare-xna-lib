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
        public BEPUphysics.Entities.Prefabs.Box physicsBox;

        public BoxEntity(Game1 game, Vector3 position, Vector3 size, float mass, Texture2D texture) : base(game, texture) {
            visualBox = new Shape3D.Box(game, size);
            Vector3 physicalSize = size * 2;
			if (mass < 0) {
				physicsBox = new BEPUphysics.Entities.Prefabs.Box(position, physicalSize.X, physicalSize.Y, physicalSize.Z);
			} else {
				physicsBox = new BEPUphysics.Entities.Prefabs.Box(position, physicalSize.X, physicalSize.Y, physicalSize.Z, mass);
			}
			physicsBox.CollisionInformation.Events.InitialCollisionDetected += game.HandleCollision;
			game.space.Add(physicsBox);
		}

        protected override void Dispose() {
            physicsBox.CollisionInformation.Events.RemoveAllEvents();
            Game.space.Remove(physicsBox);
            visualBox = null;
            physicsBox = null;
            base.Dispose();
        }

		public override void Update(GameTime gameTime) {
            if (physicsBox.Position.Y < -100) {
                Entity.Remove(this);
            }
		}

		public override void Draw() {
            DrawShape(physicsBox.WorldTransform, visualBox);
		}
	}
}

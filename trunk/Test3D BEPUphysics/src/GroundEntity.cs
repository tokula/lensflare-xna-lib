using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LensflareGameFramework;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace EngineTest {
    class GroundEntity : GameEntity {
        Shape3D.Quad quad;
        public BEPUphysics.Entities.Prefabs.Box physicsBox;

        public GroundEntity(Game1 game, Vector3 position, Vector3 size, Texture2D texture) : base(game, texture) {
            quad = new Shape3D.Quad(game, size);
            Vector3 physicalSize = size * 2;
		    physicsBox = new BEPUphysics.Entities.Prefabs.Box(position, physicalSize.X, physicalSize.Y, physicalSize.Z);
			physicsBox.CollisionInformation.Events.InitialCollisionDetected += game.HandleCollision;
			game.space.Add(physicsBox);
		}

        protected override void Dispose() {
            physicsBox.CollisionInformation.Events.RemoveAllEvents();
            Game.space.Remove(physicsBox);
            quad = null;
            physicsBox = null;
            base.Dispose();
        }

		public override void Update(GameTime gameTime) {

		}

		public override void Draw() {
            DrawShape(physicsBox.WorldTransform, quad);
		}
    }
}

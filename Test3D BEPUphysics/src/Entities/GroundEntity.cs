﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LensflareGameFramework;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace EngineTest {
    class GroundEntity : GameEntity {
        Shape3.Quad quad;

        public GroundEntity(Game3D game, Vector3 position, Vector3 size, Texture2D texture) : base(game, texture) {
            quad = new Shape3.Quad(game, size);
            Vector3 physicalSize = size * 2;
		    physicsEntities.Add(new BEPUphysics.Entities.Prefabs.Box(position, physicalSize.X, physicalSize.Y, physicalSize.Z)); //TODO: Plane instaead of Box
			physicsEntities[0].CollisionInformation.Events.InitialCollisionDetected += game.HandleCollision;
			game.space.Add(physicsEntities[0]);
		}

        protected override void Dispose() {
            quad = null;
            base.Dispose();
        }

		public override void Update(GameTime gameTime) {

		}

		public override void Draw() {
            DrawShape(physicsEntities[0].WorldTransform, quad);
		}
    }
}

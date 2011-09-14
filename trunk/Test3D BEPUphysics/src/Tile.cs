using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LensflareGameFramework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;

namespace EngineTest {
    class Tile : GameEntity {
        protected Shape3D.Box visualBox;
        protected SoundEffect sound;
        protected static readonly Vector3 size = new Vector3(0.5f, 0.05f, 0.5f);

        public static Vector3 Size { get { return size; } }

        public Tile(Game1 game, Vector3 position, Texture2D texture, SoundEffect sound) : base(game, texture) {
            this.sound = sound;
            visualBox = new Shape3D.Box(game, size*0.95f);
            Vector3 physicalSize = size * 2;
			physicsEntities.Add(new BEPUphysics.Entities.Prefabs.Box(position, physicalSize.X, physicalSize.Y, physicalSize.Z));
            physicsEntities[0].CollisionInformation.Events.InitialCollisionDetected += game.HandleCollision;
            game.space.Add(physicsEntities[0]);
		}

        protected override void Dispose() {
            visualBox = null;
            base.Dispose();
        }

		public override void Update(GameTime gameTime) {

		}

		public override void Draw() {
            DrawShape(physicsEntities[0].WorldTransform, visualBox);
		}
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace EngineTest {
    class TestBoxEntity : BoxEntity {
        override public Texture2D Texture { 
            get { 
                return Game.ProceduralTexture;
            } 
            set { }
        }

        override public Texture2D BumpMapTexture {
            get {
                return Game.ProceduralBumpMapTexture;
            }
            set { }
        }

        public TestBoxEntity(Game3D game, Vector3 position, Vector3 size, float mass) : base(game, position, size, mass, null) {

		}

		public override void Draw() {
            DrawShape(physicsEntities[0].WorldTransform, visualBox);
		}
    }
}

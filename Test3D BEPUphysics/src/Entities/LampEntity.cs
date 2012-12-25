using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Shape3;

namespace EngineTest {
    class LampEntity : GameEntity {
        public float Radius { get; protected set; }
        public Vector3 Position { get; set; }

        Sphere sphereShape;

        public LampEntity(Game3D game, Vector3 position, float radius) : base(game, game.textureBuilder.TextureFromColor(Color.White)) {
            Radius = radius;
            Position = position;
            sphereShape = new Sphere(game, radius, 16, 16);
        }

        protected override void Dispose() {
            sphereShape = null;
            base.Dispose();
        }

        public override void Update(GameTime gameTime) {

        }

        public override void Draw() {
            DrawShape(Matrix.CreateTranslation(Position), sphereShape);
        }
    }
}

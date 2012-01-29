using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using LensflareGameFramework;
using Util;

namespace Test2D {
    class TestEntity : GameEntity {
        Vector2 position;
        float radius;
        float tempVariable;
        float tempVariableB;

        public TestEntity(Game2D game, Vector2 position, float radius, float mass) : base(game) {
            this.position = position;
            this.radius = radius;

            //physicsEntities.Add(new BEPUphysics.Entities.Prefabs.Sphere(position, radius, mass));
            //game.space.Add(physicsEntities[0]);
        }

        protected override void Dispose() {
            base.Dispose();
        }

        public override void Update(GameTime gameTime) {
            float elapsedSeconds = (float)gameTime.ElapsedGameTime.TotalSeconds;
            tempVariable += elapsedSeconds*5;
            tempVariableB += elapsedSeconds*2;
            //Console.WriteLine("elapsed seconds: " + elapsedSeconds);
            position.X += 3.0f * (float)Math.Sin((double)tempVariable);
            position.Y += 1.0f * (float)Math.Sin((double)tempVariableB);
        }

        public override void Draw() {
            Primitive2D.DrawCircle(Game.spriteBatch, position, radius, Color.Yellow, false);
        }
    }
}

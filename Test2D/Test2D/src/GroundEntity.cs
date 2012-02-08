using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Util;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Collision.Shapes;

namespace Test2D {
    class GroundEntity : GameEntity {
        Body body;
        //EdgeShape shape;
        LinkedList<EdgeShape> lineShapes = new LinkedList<EdgeShape>();

        public GroundEntity(Game2D game) : base(game) {
            body = new Body(game.world);
            body.BodyType = BodyType.Static;

            //shape = new EdgeShape(new Vector2(-200, 100), new Vector2(300, 150));
            //new Fixture(body, shape);

            CellMapBuilder cmb = new CellMapBuilder(Game.random);
            float[] components = cmb.Test(16, 16, 200);
            for (int i = 0; i < components.Length; i += 4) {
                float x1 = components[i];
                float y1 = components[i + 1];
                float x2 = components[i + 2];
                float y2 = components[i + 3];
                EdgeShape lineShape = new EdgeShape(new Vector2(x1, y1), new Vector2(x2, y2));
                new Fixture(body, lineShape);
                lineShapes.AddLast(lineShape);
            }
        }

        protected override void Dispose() {
            base.Dispose();
        }

        public override void Update(GameTime gameTime) {

        }

        public override void Draw() {
            Vector2 screenPosition = Game.camera.PositionScreen - Game.camera.PositionWorld;
            foreach(EdgeShape shape in lineShapes) {
                Primitive2.DrawLine(Game.spriteBatch, screenPosition + shape.Vertex1, screenPosition + shape.Vertex2, Color.Yellow);
            }
            //Primitive2.DrawLine(Game.spriteBatch, screenPosition + shape.Vertex1, screenPosition + shape.Vertex2, Color.DarkGreen);
        }
    }
}

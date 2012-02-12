using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Util;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Collision.Shapes;
using Microsoft.Xna.Framework.Graphics;

namespace Test2D {
    class GroundEntity : GameEntity {
        Body body;
        LinkedList<EdgeShape> lineShapes = new LinkedList<EdgeShape>();
        bool[,] bitmap;
        float cellScale;
        Texture2D texture;

        public GroundEntity(Game2D game, Texture2D texture) : base(game) {
            this.texture = texture;
            body = new Body(game.world);
            body.BodyType = BodyType.Static;

            this.cellScale = 512;
            CellMapBuilder cmb = new CellMapBuilder(Game.random);
            bitmap = cmb.MakeBitmap(16, 16, CellMapBuilder.BitmapGenerationMode.Random);
            foreach (var vectorPair in cmb.VectorPairsList(cmb.MakeBorders(bitmap, cellScale))) {
                EdgeShape lineShape = new EdgeShape(vectorPair[0], vectorPair[1]);
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

            int cellXCount = bitmap.GetLength(0);
            int cellYCount = bitmap.GetLength(1);
            for (int y = 0; y < cellYCount; ++y) {
                for (int x = 0; x < cellXCount; ++x) {
                    if (bitmap[x, y]) {
                        Vector2 cellOffset = new Vector2(x, y) * cellScale;
                        Game.spriteBatch.Draw(texture, screenPosition + cellOffset, Color.White);
                    }
                }
            }

            foreach (EdgeShape shape in lineShapes) {
                Primitive2.DrawLine(Game.spriteBatch, screenPosition + shape.Vertex1, screenPosition + shape.Vertex2, Color.Yellow);
            }
        }
    }
}

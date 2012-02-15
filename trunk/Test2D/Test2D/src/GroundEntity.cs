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
        Texture2D[,] textures;

        const int splitCount = 8;

        public GroundEntity(Game2D game, Texture2D texture) : base(game) {
            body = new Body(game.world);
            body.BodyType = BodyType.Static;

            this.textures = game.proceduralTextureBuilder.Split(texture, splitCount);

            this.cellScale = texture.Width / splitCount;
            CellMapBuilder cmb = new CellMapBuilder(Game.random);
            bitmap = cmb.MakeBitmap(128, 128, CellMapBuilder.BitmapGenerationMode.Random);
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
                        //Vector2 cellPosition = screenPosition + new Vector2(x, y) * cellScale;
                        Texture2D texture = textures[x%splitCount, y%splitCount];
                        /*
                        if (Game.camera.IsTextureVisible(texture, cellPosition)) {
                            Game.spriteBatch.Draw(texture, cellPosition, Color.White);
                        }*/


                        for (int i = 0; i < 15; ++i) {
                            float dist = i * 0.015f;
                            float s = 1.0f - dist;
                            Vector2 cellPosition = (Game.camera.PositionScreen - Game.camera.PositionWorld*s) + new Vector2(x, y) * s * cellScale;
                            float layerCellScale = (1.0f-s)*0.5f;

                            if (Game.camera.IsTextureVisible(texture, cellPosition)) {
                                Game.spriteBatch.Draw(texture, cellPosition, null, Color.White * s, 0.0f, Vector2.Zero, 1.0f * s, SpriteEffects.None, layerCellScale);
                            }
                        }
                    }
                }
            }

            /*
            foreach (EdgeShape shape in lineShapes) {
                Vector2 point1 = screenPosition + shape.Vertex1;
                Vector2 point2 = screenPosition + shape.Vertex2;
                if(Game.camera.IsLineVisible(point1, point2)) {
                    Primitive2.DrawLine(Game.spriteBatch, point1, point2, Color.Yellow);
                }
            }*/
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Util;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Collision.Shapes;
using Microsoft.Xna.Framework.Graphics;
using Camera;

namespace Test2D {
    class GroundEntity : GameEntity {
        Body body;
        LinkedList<EdgeShape> lineShapes = new LinkedList<EdgeShape>();
        bool[,] bitmap;
        float cellScale;
        Texture2D[,] textures;
        Texture2D testLineTexture;

        const int splitCount = 8;

        public GroundEntity(Game2D game, Texture2D texture) : base(game) {
            body = new Body(game.World);
            body.BodyType = BodyType.Static;

            this.textures = game.TextureBuilder.Split(texture, splitCount);
            this.testLineTexture = Game.Content.Load<Texture2D>("line_c");

            this.cellScale = texture.Width / splitCount;
            CellMapBuilder cmb = new CellMapBuilder(Game.Random);
            bitmap = cmb.MakeBitmap(64, 64, CellMapBuilder.BitmapGenerationMode.Random);
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
            Camera2 camera = Game.Camera;
            SpriteBatch spriteBatch = Game.SpriteBatch;

            int cellXCount = bitmap.GetLength(0);
            int cellYCount = bitmap.GetLength(1);
            for (int y = 0; y < cellYCount; ++y) {
                for (int x = 0; x < cellXCount; ++x) {
                    if (bitmap[x, y]) {
                        Texture2D texture = textures[x%splitCount, y%splitCount];

                        for (int i = 0; i < 10; ++i) {
                            float dist = i * 0.015f;
                            float s = 1.0f - dist;
                            Vector2 cellPosition = camera.Project(new Vector2(x, y) * s * cellScale, s);
                            float layerCellScale = (1.0f-s)*0.5f;

                            float textureScale = s * camera.Zoom;
                            if (camera.OverlapsRect(cellPosition, texture.GetSize() * textureScale)) {
                                spriteBatch.Draw(texture, cellPosition, null, Color.White * s, 0.0f, Vector2.Zero, textureScale, SpriteEffects.None, Game.LayerManager.Depth((int)MainLayer.Ground, layerCellScale));
                            }
                        }
                    }
                }
            }

            foreach (EdgeShape shape in lineShapes) {
                Vector2 point1 = camera.Project(shape.Vertex1);
                Vector2 point2 = camera.Project(shape.Vertex2);

                if(camera.OverlapsLine(point1, point2)) {
                    Primitive2.DrawTextureLine(spriteBatch, testLineTexture, point1, point2, testLineTexture.Height * 0.25f * camera.Zoom, Color.White, Game.LayerManager.Depth((int)MainLayer.Walls));
                }
            }
        }
    }
}

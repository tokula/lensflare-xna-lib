using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Collision.Shapes;
using Microsoft.Xna.Framework;
using Util;

namespace Test2D {
    class WorldFrameEntity : GameEntity {
        Vector2 size;
        Body body;

        public WorldFrameEntity(Game2D game, Vector2 position, Vector2 size) : base(game) {
            this.size = size;

            body = new Body(game.World);
            body.BodyType = BodyType.Static;

            float wHalf = size.X * 0.5f;
            float hHalf = size.Y * 0.5f;

            EdgeShape leftShape =  new EdgeShape(new Vector2(0 - wHalf, 0 - hHalf), new Vector2(0 - wHalf, 0 + hHalf));
            EdgeShape rightShape = new EdgeShape(new Vector2(0 + wHalf, 0 - hHalf), new Vector2(0 + wHalf, 0 + hHalf));
            EdgeShape topShape =   new EdgeShape(new Vector2(0 - wHalf, 0 - hHalf), new Vector2(0 + wHalf, 0 - hHalf));
            EdgeShape downShape =  new EdgeShape(new Vector2(0 - wHalf, 0 + hHalf), new Vector2(0 + wHalf, 0 + hHalf));

            new Fixture(body, leftShape);
            new Fixture(body, rightShape);
            new Fixture(body, topShape);
            new Fixture(body, downShape);
        }

        protected override void Dispose() {
            base.Dispose();
        }

        public override void Update(GameTime gameTime) {

        }

        public override void Draw() {
            //Vector2 screenPosition = Game.camera.PositionScreen - Game.camera.PositionWorld;
            Vector2 screenPosition = Game.Camera.ScreenPointFromWorldPoint(Vector2.Zero);
            Vector2 zoomedSize = size * Game.Camera.Zoom;
            Primitive2.DrawRect(Game.SpriteBatch, screenPosition + body.Position - zoomedSize * 0.5f, zoomedSize, Color.DarkGreen, false, Game.LayerManager.Depth((int)MainLayer.Hud));
        }
    }
}

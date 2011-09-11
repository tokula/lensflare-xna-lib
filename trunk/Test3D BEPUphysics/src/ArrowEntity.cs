using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LensflareGameFramework;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Shape3D;

namespace EngineTest {
    class ArrowEntity : GameEntity {
        Shape tip;
        Shape bar;

        public Vector3 position = Vector3.Zero;
        public Vector3 direction = Vector3.Up;

        protected float length;
        protected bool tipIsOrigin;
        protected const float tipSizeFactor = 0.1f;

        public ArrowEntity(Game1 game, Vector3 position, Vector3 size, bool tipIsOrigin, Texture2D texture)
            : base(game, texture) {
            this.position = position;
            this.tipIsOrigin = tipIsOrigin;
            tip = new Tetrahedron(game, size * tipSizeFactor);
            bar = new Box(game, new Vector3(0.05f, 0.05f, 0.5f) * size);
            length = size.Z;
        }

        protected override void Dispose() {
            tip = null;
            bar = null;
            base.Dispose();
        }

        public override void Update(GameTime gameTime) {

        }

        public override void Draw() {
            Matrix rotMatrix;

            //arrow bar:
            Engine.DirectionToRotationMatrix(ref direction, out rotMatrix);
            DrawShape(Matrix.CreateTranslation(0, 0, tipIsOrigin ? length * 0.5f + length * tipSizeFactor * 4 : length * -0.5f) * rotMatrix * Matrix.CreateTranslation(position), bar);

            //arrow tip:
            Matrix tipRotation;
            float a = (float)Math.PI / 4.0f;
            Vector3 axis = new Vector3(-1, -1, 0);
            Matrix.CreateFromAxisAngle(ref axis, a, out tipRotation);
            Engine.DirectionToRotationMatrix(ref direction, out rotMatrix);
            DrawShape(tipRotation * Matrix.CreateTranslation(0, 0, tipIsOrigin ? length * tipSizeFactor * 4 : -length) * rotMatrix * Matrix.CreateTranslation(position), tip);


            /* TODO: remove after test
            Matrix worldMatrix;
            Matrix rotMatrix;

            //arrow bar:
            game.engine.RotationMatrixFromDirection(ref direction, out rotMatrix);
            worldMatrix = Matrix.CreateTranslation(0, 0, tipIsOrigin ? length * 0.5f + length * tipSizeFactor*4 : length * -0.5f) * rotMatrix * Matrix.CreateTranslation(position);
            effect.Parameters["xWorldViewProjection"].SetValue(worldMatrix * game.camera.view * game.camera.projection);
            effect.Parameters["xTexture"].SetValue(texture);
            effect.Parameters["xWorld"].SetValue(worldMatrix);
            effect.Parameters["xLightsWorldViewProjection"].SetValue(worldMatrix * game.engine.lightsViewProjectionMatrix);

            foreach (EffectPass pass in effect.CurrentTechnique.Passes) {
                pass.Apply();
                bar.Draw();
            }

            //arrow tip:
            Matrix tipRotation;
            float a = (float)Math.PI / 4.0f;
            Vector3 axis = new Vector3(-1, -1, 0);
            Matrix.CreateFromAxisAngle(ref axis, a, out tipRotation);

            game.engine.RotationMatrixFromDirection(ref direction, out rotMatrix);
            worldMatrix = tipRotation * Matrix.CreateTranslation(0, 0, tipIsOrigin ? length * tipSizeFactor*4 : -length) * rotMatrix * Matrix.CreateTranslation(position);
            effect.Parameters["xWorldViewProjection"].SetValue(worldMatrix * game.camera.view * game.camera.projection);
            effect.Parameters["xTexture"].SetValue(texture);
            effect.Parameters["xWorld"].SetValue(worldMatrix);
            effect.Parameters["xLightsWorldViewProjection"].SetValue(worldMatrix * game.engine.lightsViewProjectionMatrix);

            foreach (EffectPass pass in effect.CurrentTechnique.Passes) {
                pass.Apply();
                tip.Draw();
            }
            */
        }
    }
}

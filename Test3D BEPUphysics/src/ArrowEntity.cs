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
        readonly bool wireframe; //TODO: test

        Shape tip;
        Shape bar;

        Arrow lineArrow; //TODO: test

        public Vector3 position = Vector3.Zero;
        public Vector3 direction = Vector3.Up;

        protected float length;
        protected bool tipIsOrigin;
        protected const float tipSizeFactor = 0.1f;

        public ArrowEntity(Game1 game, Vector3 position, Vector3 size, bool tipIsOrigin, Texture2D texture) : base(game, texture) {
            this.position = position;
            this.tipIsOrigin = tipIsOrigin;
            wireframe = false;
            if (wireframe) {
                lineArrow = new Arrow(game, size);
            } else {
                tip = new Tetrahedron(game, size * tipSizeFactor);
                bar = new Box(game, new Vector3(0.05f, 0.05f, 0.5f) * size);
            }
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
            if (wireframe) {
                Engine.DirectionToRotationMatrix(ref direction, out rotMatrix);
                DrawShape(rotMatrix * Matrix.CreateTranslation(position), lineArrow);
            } else {
                //arrow bar:
                Engine.DirectionToRotationMatrix(ref direction, out rotMatrix);
                DrawShape(Matrix.CreateTranslation(0, 0, tipIsOrigin ? length * 0.5f + length * tipSizeFactor * 2.1f : length * -0.5f) * rotMatrix * Matrix.CreateTranslation(position), bar);

                //arrow tip:
                Matrix tipRotation;
                float a = (float)Math.PI / 4.0f;
                Vector3 axis = new Vector3(-1, -1, 0);
                Matrix.CreateFromAxisAngle(ref axis, a, out tipRotation);
                Engine.DirectionToRotationMatrix(ref direction, out rotMatrix);
                DrawShape(tipRotation * Matrix.CreateTranslation(0, 0, tipIsOrigin ? length * tipSizeFactor * 2.1f : -length) * rotMatrix * Matrix.CreateTranslation(position), tip);
            }
        }
    }
}

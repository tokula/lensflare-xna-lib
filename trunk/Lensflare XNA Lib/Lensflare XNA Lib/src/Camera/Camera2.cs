using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Camera {
    public class Camera2 {
        protected Vector2 positionWorld = Vector2.Zero;
        protected Vector2 positionScreen = Vector2.Zero;
        protected Vector2 size = new Vector2(100, 100);

        public Vector2 PositionWorld { get { return positionWorld; } set { positionWorld = value; } }
        public Vector2 PositionScreen { get { return positionScreen; } set { positionScreen = value; } }
        public Vector2 Size { get { return size; } set { size = value; } }

        public Camera2() {
        }

        public virtual void Update(GameTime gameTime) {

        }

        public bool IsTextureVisible(Texture2D texture, Vector2 position) {
            Vector2 v = positionScreen - size * 0.5f;
            float cx1 = v.X;
            float cy1 = v.Y;
            float cx2 = cx1 + size.X;
            float cy2 = cy1 + size.Y;
            
            float ex1 = position.X;
            float ey1 = position.Y;
            float ex2 = ex1 + texture.Width;
            float ey2 = ey1 + texture.Height;

            return !(ex2 < cx1 || ex1 > cx2 || ey2 < cy1 || ey1 > cy2);
        }

        public bool IsLineVisible(Vector2 point1, Vector2 point2) {
            Vector2 v = positionScreen - size * 0.5f;
            float cx1 = v.X;
            float cy1 = v.Y;
            float cx2 = cx1 + size.X;
            float cy2 = cy1 + size.Y;

            float ex1 = point1.X;
            float ey1 = point1.Y;
            float ex2 = point2.X;
            float ey2 = point2.Y;

            return (ex1 >= cx1 && ex1 <= cx2 && ey1 >= cy1 && ey1 <= cy2) || (ex2 >= cx1 && ex2 <= cx2 && ey2 >= cy1 && ey2 <= cy2);
        }
    }
}

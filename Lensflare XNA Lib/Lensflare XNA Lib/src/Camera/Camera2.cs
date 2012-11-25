using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Camera {
    public class Camera2 {
        public Vector2 PositionWorld { get; set; }
        public Vector2 PositionScreen { get; set; }
        public Vector2 ViewSize { get; set; }
        public float Zoom { get; set; }

        public Camera2() {
            ViewSize = new Vector2(100, 100);
            Zoom = 1.0f;
        }

        public virtual void Update(GameTime gameTime) {

        }

        public Vector2 Project(Vector2 worldPoint) {
            return PositionScreen - PositionWorld * Zoom + worldPoint * Zoom;
        }

        public Vector2 Project(Vector2 worldPoint, float depthScale) {
            float z = (float)Math.Pow(Zoom, depthScale);
            //return PositionScreen - PositionWorld * Zoom * depthScale + worldPoint * Zoom;
            return PositionScreen - PositionWorld * z * depthScale + worldPoint * z; //TODO: does not work correctly when zoomed out very far.
        }

        public Vector2 Unproject(Vector2 screenPoint) {
            return screenPoint / Zoom - PositionScreen / Zoom + PositionWorld;
        }

        public bool OverlapsRect(Vector2 position, Vector2 size) {
            Vector2 v = PositionScreen - ViewSize * 0.5f;

            float cx1 = v.X;
            float cy1 = v.Y;
            float cx2 = cx1 + ViewSize.X;
            float cy2 = cy1 + ViewSize.Y;
            
            float ex1 = position.X;
            float ey1 = position.Y;
            float ex2 = ex1 + size.X;
            float ey2 = ey1 + size.Y;

            return !(ex2 < cx1 || ex1 > cx2 || ey2 < cy1 || ey1 > cy2);
        }

        public bool OverlapsLine(Vector2 point1, Vector2 point2) {
            Vector2 v = PositionScreen - ViewSize * 0.5f;

            float cx1 = v.X;
            float cy1 = v.Y;
            float cx2 = cx1 + ViewSize.X;
            float cy2 = cy1 + ViewSize.Y;

            float ex1 = point1.X;
            float ey1 = point1.Y;
            float ex2 = point2.X;
            float ey2 = point2.Y;

            return (ex1 >= cx1 && ex1 <= cx2 && ey1 >= cy1 && ey1 <= cy2) || (ex2 >= cx1 && ex2 <= cx2 && ey2 >= cy1 && ey2 <= cy2);
        }

        public bool OverlapsWorldRect(Vector2 position, Vector2 size) {
            return OverlapsRect(Project(position), size*Zoom);
        }

        public bool OverlapsWorldLine(Vector2 point1, Vector2 point2) {
            return OverlapsLine(Project(point1), Project(point2));
        }
    }
}

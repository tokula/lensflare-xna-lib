using System;
using Microsoft.Xna.Framework;

namespace Util {
    public struct IntVector2 {
        public int X, Y;

        public Vector2 Vector2 { get { return new Vector2(X, Y); } }
        public IntVector2 Direction9State { get { return new IntVector2(X > 0 ? 1 : X < 0 ? -1 : 0, Y > 0 ? 1 : Y < 0 ? -1 : 0); } }

        public IntVector2(int x, int y) {
            this.X = x;
            this.Y = y;
        }

        public IntVector2(Vector2 v) {
            this.X = (int)v.X;
            this.Y = (int)v.Y;
        }

        public override bool Equals(Object obj) {
            if (obj is IntVector2) {
                return this == (IntVector2)obj;
            } else {
                return false;
            }
        }

        public override int GetHashCode() {
            return base.GetHashCode(); //TODO: recherchieren
        }

        public static IntVector2 operator -(IntVector2 v) {
            return new IntVector2(-v.X, -v.Y);
        }

        public static IntVector2 operator +(IntVector2 v1, IntVector2 v2) {
            return new IntVector2(v1.X + v2.X, v1.Y + v2.Y);
        }

        public static IntVector2 operator -(IntVector2 v1, IntVector2 v2) {
            return new IntVector2(v1.X - v2.X, v1.Y - v2.Y);
        }

        public static Vector2 operator +(IntVector2 v1, Vector2 v2) {
            return new Vector2(v1.X + v2.X, v1.Y + v2.Y);
        }

        public static Vector2 operator -(IntVector2 v1, Vector2 v2) {
            return new Vector2(v1.X - v2.X, v1.Y - v2.Y);
        }

        public static bool operator ==(IntVector2 v1, IntVector2 v2) {
            return v1.X == v2.X && v1.Y == v2.Y;
        }

        public static bool operator !=(IntVector2 v1, IntVector2 v2) {
            return v1.X != v2.X || v1.Y != v2.Y;
        }

        public static Vector2 operator *(IntVector2 v1, Vector2 v2) {
            return new Vector2(v1.X * v2.X, v1.Y * v2.Y);
        }

        public static IntVector2 operator *(IntVector2 v, int scale) {
            return new IntVector2(v.X * scale, v.Y * scale);
        }

        public static Vector2 operator *(IntVector2 v, float scale) {
            return new Vector2(v.X * scale, v.Y * scale);
        }

        public static float Distance(IntVector2 v1, IntVector2 v2) {
            int dx = v1.X - v2.X;
            int dy = v1.Y - v2.Y;
            return (float)Math.Sqrt(dx * dx + dy * dy);
        }

        public static void Distance(ref IntVector2 v1, ref IntVector2 v2, out float result) {
            int dx = v1.X - v2.X;
            int dy = v1.Y - v2.Y;
            result = (float)Math.Sqrt(dx * dx + dy * dy);
        }
    }
}

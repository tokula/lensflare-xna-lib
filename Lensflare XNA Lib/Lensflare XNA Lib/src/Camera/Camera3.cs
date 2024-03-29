﻿using Microsoft.Xna.Framework;

namespace Camera {
    public class Camera3 {
        protected float aspectRatio = 1.0f;
        protected Vector3 position = Vector3.Zero;
        protected Vector3 rotation = Vector3.Zero;
        protected Matrix view = Matrix.Identity;
        protected Matrix projection = Matrix.Identity;

        public float AspectRatio { get { return aspectRatio; } set { aspectRatio = value; } }
        public Vector3 Position { get { return position; } set { position = value; } }
        public Vector3 Rotation { get { return rotation; } set { rotation = value; } }
        public Matrix ViewMatrix { get { return view; } set { view = value; } }
        public Matrix ProjectionMatrix { get { return projection; } set { projection = value; } }
        public Vector3 UpVector { get { return Vector3.Up; } }

        public Camera3() {
        }

        public Matrix CalcRotationMatrix() {
            return Matrix.CreateRotationX(rotation.X) * Matrix.CreateRotationY(rotation.Y);
        }

        public virtual void Update(GameTime gameTime) {
            Matrix rotationMatrix = CalcRotationMatrix();

            Vector3 originalTarget = new Vector3(0, 0, -1);
            Vector3 rotatedTarget = Vector3.Transform(originalTarget, rotationMatrix);
            Vector3 finalTarget = position + rotatedTarget;

            Vector3 rotatedUpVector = Vector3.Transform(UpVector, rotationMatrix);

            view = Matrix.CreateLookAt(position, finalTarget, rotatedUpVector);
            projection = Matrix.CreatePerspectiveFieldOfView(MathHelper.ToRadians(45.0f), aspectRatio, 0.001f, 1000000.0f); //TODO: device.Viewport.AspectRatio
        }
    }
}

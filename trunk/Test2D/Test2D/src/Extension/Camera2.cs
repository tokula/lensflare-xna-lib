using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Camera;
using Microsoft.Xna.Framework;
using FarseerPhysics.Collision.Shapes;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Collision;
using FarseerPhysics.Common;

namespace Test2D {
    public static class Camera2Extension {
        public static bool OverlapsWorldShape(this Camera2 camera, Shape shape, Body body) {
            AABB aabb;
            Transform transform;
            body.GetTransform(out transform);
            shape.ComputeAABB(out aabb, ref transform, 0);
            Vector2 extents = aabb.Extents;

            return camera.OverlapsRect(camera.Project(aabb.Center - extents), extents * 2.0f * camera.Zoom);
        }
    }
}

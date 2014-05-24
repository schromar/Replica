using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Replica
{
    class Camera
    {
        Vector2 resolution;

        Transform transform;

        Matrix view;
        Matrix projection;

        public Camera(Vector2 resolution)
        {
            this.resolution = resolution;

            transform = new Transform();

            UpdateView();
            projection = Matrix.CreatePerspectiveFieldOfView(MathHelper.ToRadians(90), resolution.X / resolution.Y, 0.5f, 1000.0f);
        }

        public void SetTransform(Transform transform)
        {
            this.transform = transform;
            UpdateView();
        }

        public Matrix GetView()
        {
            return view;
        }

        public Matrix GetProjection()
        {
            return projection;
        }

        void UpdateView()
        {
            view = Matrix.CreateLookAt(transform.position, transform.position + transform.forward, transform.up);
        }
    }
}

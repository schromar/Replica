using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Replica
{
    class Camera
    {
        //TODO 1: Do we even need resolution/transform attributes?
        Vector2 resolution;

        Transform transform;

        Matrix view;
        Matrix projection;

        //TODO: Add fov attribute?

        public Camera(Vector2 resolution)
        {
            this.resolution = resolution;

            transform = new Transform();

            UpdateView();
            projection = Matrix.CreatePerspectiveFieldOfView(MathHelper.ToRadians(90), resolution.X / resolution.Y, 0.5f, 1000.0f);
        }

        /// <summary>
        /// Used by Replica.Entities.Player to update the camera.
        /// </summary>
        /// <param name="transform"></param>
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
            view = Matrix.CreateLookAt(transform.position, transform.position + transform.Forward, transform.Up);
        }
    }
}
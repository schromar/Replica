using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Replica
{
    public class Camera
    {
        //TODO 2: Do we even need resolution/transform attributes?
        Vector2 resolution;

        Transform transform = new Transform();

        Matrix view;
        public Matrix View { get { return view; } }
        Matrix projection;
        public Matrix Projection { get { return projection; } }

        //TODO: Add fov attribute?

        public Camera(Vector2 resolution)
        {
            this.resolution = resolution;

            UpdateView();
            projection = Matrix.CreatePerspectiveFieldOfView(MathHelper.ToRadians(90), resolution.X / resolution.Y, 0.5f, 1000.0f);
        }

        /// <summary>
        /// Used by Entities.Player to update the camera.
        /// </summary>
        /// <param name="transform"></param>
        public void SetTransform(Transform transform)
        {
            this.transform = transform;
            UpdateView();
        }

        void UpdateView()
        {
            view = Matrix.CreateLookAt(transform.position, transform.position + transform.Forward, transform.Up);
        }
    }
}
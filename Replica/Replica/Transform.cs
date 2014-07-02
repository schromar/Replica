using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Replica
{
    //class Transform instead?
    public struct Transform
    {
        //What if an entity returns the transform and it is changed? (reference?)
        public Vector3 position;

        private Vector2 rotation;
        public Vector2 Rotation
        {
            get
            {
                return rotation;
            }
            set
            {
                rotation = value;
                //Math. It happens. Here.
                forward = new Vector3((float)Math.Cos(rotation.Y) * (float)Math.Sin(rotation.X),
                                                (float)Math.Sin(rotation.Y),
                                                (float)Math.Cos(rotation.Y) * (float)Math.Cos(rotation.X));
                right = new Vector3((float)Math.Sin(rotation.X - Math.PI / 2.0f),
                                            0,
                                            (float)Math.Cos(rotation.X - Math.PI / 2.0f));
                up = Vector3.Cross(right, forward);
            }
        }

        private Vector3 forward;
        private Vector3 right;
        private Vector3 up;
        public Vector3 Forward { get { return forward; } }
        public Vector3 Right { get { return right; } }
        public Vector3 Up { get { return up; } }
        //TODO: Add size attribute?

        /*public Transform()
        {
            position = Vector3.Zero;

            rotation = Vector2.Zero;

            forward = Vector3.Forward;
            right = Vector3.Right;
            up = Vector3.Up;
        }*/
    }
}

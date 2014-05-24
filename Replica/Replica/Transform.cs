using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Replica
{
    class Transform
    {
        //What if an entity returns the transform and it is changed? (reference?)
        public Vector3 position;

        public Vector3 forward;
        public Vector3 right;
        public Vector3 up;

        public Transform()
        {
            position = Vector3.Zero;

            forward = Vector3.Forward;
            right = Vector3.Right;
            up = Vector3.Up;
        }
    }
}

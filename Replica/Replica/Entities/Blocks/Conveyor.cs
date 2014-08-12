using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Replica.Entities.Blocks
{
    class Conveyor : Block
    {
        /// <summary>
        /// The direction the Conveyor pushes object in. Y-Axis should have no effect.
        /// </summary>
        Vector3 direction;
        public Vector3 Direction { get { return direction; } }
        float speed=7;
        public float Speed { get { return speed; } }

        public Conveyor(List<Entity> entities, Level lvl, Transform transform, Vector3 boundsSize, Vector3 direction)
            : base(entities, lvl, transform, boundsSize, EntityType.Conveyor)
        {
            this.direction = direction;
        }
    }
}

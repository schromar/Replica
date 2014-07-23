using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Replica.Entities.Blocks
{
    class JumpPad : Block
    {
        float velocity = 20;
        public float Velocity { get { return velocity; } }

        public JumpPad(List<Entity> entities, Level lvl, Transform transform, Vector3 boundsSize)
            : base(entities, lvl, transform, boundsSize, EntityType.JumpPad)
        {

        }
    }
}

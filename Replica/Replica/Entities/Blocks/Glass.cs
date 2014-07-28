using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Replica.Entities.Blocks
{
    class Glass : Block
    {
        public Glass(List<Entity> entities, Level lvl, Transform transform, Vector3 boundsSize)
            : base(entities, lvl, transform, boundsSize, EntityType.Glass)
        {
            drawBounds = true;
            solid = true;

            draw = false;
        }
    }
}

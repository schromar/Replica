using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Replica.Entities
{
    class Block : Entity
    {

        public Block(List<Entity> entities, Level lvl, Transform transform, Vector3 boundsSize, EntityType type = EntityType.Block)
            : base(entities, lvl, type, transform, boundsSize)
        {
            this.transform = transform;
        }
    }
}

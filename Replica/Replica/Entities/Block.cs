using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Replica.Entities
{
    class Block : Entity
    {

        public Block(Transform transform, List<Entity> entities,Level lvl,  EntityType type=EntityType.Block)
            : base(entities, lvl, type)
        {
            this.transform = transform;
            Vector3 boundSize = new Vector3(2, 2, 2); //Every block currently is 2x2x2 big, also transform.position is the middle point
            bounds.Min = transform.position - boundSize / 2.0f;
            bounds.Max = transform.position + boundSize / 2.0f;
        }
    }
}

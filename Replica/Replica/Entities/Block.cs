using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Replica.Entities
{
    class Block : Entity
    {
        

        public Block(Transform transform, Vector3 boundSize, List<Entity> entities, Level lvl,  EntityType type=EntityType.Block)
            : base(entities, lvl, type)
        {
            this.transform = transform;
            bounds.Min = transform.position - boundSize / 2.0f;
            bounds.Max = transform.position + boundSize / 2.0f;

            solid = true;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Replica
{
    class Replicant : Entity
    {
        public Replicant(List<Entity> entities, Transform transform)
            : base(entities)
        {
            this.transform = transform;
        }
    }
}

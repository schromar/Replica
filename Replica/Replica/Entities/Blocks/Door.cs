using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Replica.Entities.Blocks
{
    class Door : Block
    {
        List<Switch> requirements;

        public Door(List<Entity> entities, Transform transform, List<Switch> requirements)
            : base(entities, transform)
        {
            this.requirements = requirements;
        }

        //TODO 3: check requirements
    }
}

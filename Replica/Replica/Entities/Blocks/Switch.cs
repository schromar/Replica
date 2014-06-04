using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Replica.Entities.Blocks
{
    class Switch : Block
    {
        bool activated;

        public Switch(List<Entity> entities, Transform transform)
            : base(entities, transform)
        {
            activated = false;
        }

        public override void OnCollision(Entity entity)
        {
            //TODO 2: activate
            if (entity.GetType() == typeof(Player)) //RTTI=bad?
            {
                Console.WriteLine("HAAAAAALP");
            }
        }
    }
}

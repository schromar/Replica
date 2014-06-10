using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Replica.Entities.Blocks
{
    class Door : Block
    {
        List<Switch> requirements;
        bool open;

        public Door(List<Entity> entities, Transform transform, List<Switch> requirements)
            : base(entities, transform, EntityType.Door)
        {
            this.requirements = requirements;
            open = false;

            boundsColor = Color.Yellow;
        }

        public override void Update(GameTime gameTime)
        {
            open = true;
            foreach(Switch requirement in requirements)
            {
                if (!requirement.isActivated())
                {
                    open = false;
                    break;
                }
            }
        }

        public override void OnCollision(Entity entity)
        {
            if (entity.GetEntityType() == EntityType.Player) //TODO: Testing
            {
                if (open)
                {
                    Console.WriteLine("You win, yay! bowOwls");
                }
            }
        }
    }
}

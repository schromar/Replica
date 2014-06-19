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
        string color;
        public static bool done;

        public Door(List<Entity> entities, Level lvl, Transform transform, Vector3 boundsSize, String color)
            : base(entities, lvl, transform, boundsSize, EntityType.Door)
        {
            requirements = lvl.getSwitches(color);
            this.color = color;
            open = false;

            boundsColor = Color.Yellow;
        }

        public override void Update(GameTime gameTime)
        {
            open = true;

           

            foreach(Switch requirement in requirements)
            {
                if (!requirement.IsActivated())
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
                done = false;
                if (open)
                {
                    done = true;
                }
            }
        }
    }
}

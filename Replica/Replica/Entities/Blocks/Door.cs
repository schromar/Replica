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
        string color;

        public Door(Transform transform, Vector3 boundSize, String color, List<Entity> entities, Level lvl)
            : base(transform, boundSize, entities, lvl, EntityType.Door)
        {
            solid = true;
            requirements = lvl.getSwitches(color);
            this.color = color;

            boundsColor = Color.Yellow;
        }

        public override void Update(GameTime gameTime)
        {
            solid = false;

            foreach(Switch requirement in requirements)
            {
                if (!requirement.isActivated())
                {
                    solid = true;
                    break;
                }
            }
        }

        public override void OnCollision(Entity entity)
        {
        }
    }
}

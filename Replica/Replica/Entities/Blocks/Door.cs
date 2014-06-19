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



        public Door(List<Entity> entities, Level lvl, Transform transform, Vector3 boundsSize, String color)
            : base(entities, lvl, transform, boundsSize, EntityType.Door)
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
                if (!requirement.IsActivated())
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

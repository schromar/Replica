using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Replica.Entities.Blocks
{
    /// <summary>
    /// Block which stops being solid once all switches of the same color are activated.
    /// </summary>
    class Door : Block
    {
        /// <summary>
        /// The switches that need to be activated for the door to be open.
        /// </summary>
        List<Switch> requirements;

        string color;

        public Door(List<Entity> entities, Level lvl, Transform transform, Vector3 boundsSize, String color)
            : base(entities, lvl, transform, boundsSize, EntityType.Door)
        {
            boundsColor = Color.Yellow;
            solid = true;

            requirements = lvl.getSwitches(color);
            this.color = color;
        }

        public override void Update(GameTime gameTime)
        {
            solid = false;
            foreach(Switch requirement in requirements)
            {
                if (!requirement.IsActivated()) //If only one switch in requirements is not activated the door is closed
                {
                    solid = true;
                    break;
                }
            }
        }
    }
}

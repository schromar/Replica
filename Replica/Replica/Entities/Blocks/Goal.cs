using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Replica.Statics;

namespace Replica.Entities.Blocks
{
    /// <summary>
    /// Block that causes the Player to win once he enters it.
    /// </summary>
    class Goal : Block
    {
        public Goal(Transform transform, Vector3 boundSize, List<Entity> entities, Level lvl)
            : base(entities, lvl, transform, boundSize, EntityType.Goal)
        {
            boundsColor = Color.DarkOrange;
            drawBounds = true;
            solid = false;

            draw = false;
        }

        public override void OnCollision(Entity entity)
        {
            if (entity.Type == EntityType.Player)
            {
                Globals.reachedGoal = true;
            }
        }
    }
}

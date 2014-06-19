using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Replica.Statics;

namespace Replica.Entities.Blocks
{
    class Goal : Block
    {
        

        public Goal(Transform transform, Vector3 boundSize, List<Entity> entities, Level lvl)
            : base(entities, lvl, transform, boundSize, EntityType.Goal)
        {
            boundsColor = Color.DarkOrange;
            solid = false;
        }

        public override void OnCollision(Entity entity)
        {
            if (entity.GetEntityType() == EntityType.Player) //TODO: Testing
            {
                Globals.reachedGoal = true;
            }
        }
    }
}

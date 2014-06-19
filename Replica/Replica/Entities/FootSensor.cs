using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Replica.Entities
{
    class FootSensor : Entity
    {
        bool collided;
        bool activated;
   
        public FootSensor(List<Entity> entities, Level lvl, Transform transform, Vector3 boundsSize)
            :   base(entities, lvl, EntityType.FootSensor, transform, boundsSize)
        {
            collided = false;
            activated = false;
        }

        public override void Update(GameTime gameTime)
        {
            activated = collided;
            collided = false;
        }

        public override void OnCollision(Entity entity)
        {
            if (entity.GetEntityType() == EntityType.Block || entity.GetEntityType() == EntityType.Replicant)
            {
                collided = true;
            }
        }

        public bool IsActivated()
        {
            return activated;
        }
    }
}

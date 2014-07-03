using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Replica.Entities
{
    //TODO 2: Resolve similarities between Trigger and Switch
    class Trigger : Entity
    {
        /// <summary>
        /// Can exclude entities from registering collision.
        /// </summary>
        public List<Entity> excluded = new List<Entity>();

        bool collided;
        bool activated;
        /// <summary>
        /// Holds all of the objects that have collided since last time GetColliders() was called.
        /// </summary>
        List<Entity> colliders = new List<Entity>();
   
        public Trigger(List<Entity> entities, Level lvl, Transform transform, Vector3 boundsSize)
            :   base(entities, lvl, EntityType.Trigger, transform, boundsSize)
        {

        }

        public override void Update(GameTime gameTime)
        {
            activated = collided;
            collided = false;
        }

        public override void OnCollision(Entity entity)
        {
            if (entity.isSolid() && !excluded.Contains(entity)) //TODO 1: Let users handle solid check
            {
                collided = true;
                colliders.Add(entity);
            }
        }

        public bool IsActivated()
        {
            return activated;
        }

        public List<Entity> GetColliders()
        {
            List<Entity> result = new List<Entity>(colliders);
            colliders.Clear();
            return result;
        }
    }
}

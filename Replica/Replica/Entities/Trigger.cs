using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Audio;

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
        public bool Activated { get { return activated; } }
        /// <summary>
        /// Holds all of the objects that have collided since last time GetColliders() was called.
        /// </summary>
        List<Entity> colliders = new List<Entity>();
   
        public Trigger(List<Entity> entities, Level lvl, Transform transform, Vector3 boundsSize)
            :   base(entities, lvl, EntityType.Trigger, transform, boundsSize)
        {
            drawBounds = false;
        }

        public override void Update(GameTime gameTime, AudioListener listener)
        {
            activated = collided;
            collided = false;
        }

        public override void OnCollision(Entity entity)
        {
            if (entity.Solid && !excluded.Contains(entity))
            {
                collided = true;
                colliders.Add(entity);
            }
        }

        public List<Entity> GetColliders()
        {
            List<Entity> result = new List<Entity>(colliders);
            colliders.Clear();
            return result;
        }
    }
}

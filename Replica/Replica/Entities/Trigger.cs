using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Replica.Entities
{
    class Trigger : Entity
    {
        bool collided;
        bool activated;
        List<Entity> colliders;
   
        public Trigger(List<Entity> entities, Level lvl, Transform transform, Vector3 boundsSize)
            :   base(entities, lvl, EntityType.Trigger, transform, boundsSize)
        {
            collided = false;
            activated = false;
            colliders = new List<Entity>();
        }

        public override void Update(GameTime gameTime)
        {
            activated = collided;
            collided = false;
        }

        public override void OnCollision(Entity entity)
        {
            if (entity.isSolid())
            {
                collided = true;
                colliders.Add(entity);
            }
        }

        public bool IsActivated()
        {
            return activated;
        }

        public List<Entity> GetCollider()
        {
            List<Entity> result = new List<Entity>(colliders);
            colliders.Clear();
            return result;
        }
    }
}

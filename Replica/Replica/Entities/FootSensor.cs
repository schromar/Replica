﻿using Microsoft.Xna.Framework;
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
        Entity collider;
   
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
            if (entity.isSolid())
            {
                collided = true;
                collider = entity;
            }
        }

        public bool IsActivated()
        {
            return activated;
        }

        public Entity GetCollider()
        {
            return collider;
        }
    }
}

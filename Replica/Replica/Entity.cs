﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Replica.Statics;

namespace Replica
{
    class Entity
    {
        public enum EntityType
        {
            Block,
            Player,
            Trigger,
            Replicant,
            ImitatingReplicant,
            Door,
            Switch,
            Goal 
        }
        
        /// <summary>
        /// Each Entity needs a List of entities to be able to modify other entities
        /// </summary>
        protected List<Entity> entities;
        protected Level lvl;
        protected EntityType type;

        protected Transform transform;

        protected Vector3 boundsSize;
        /// <summary>
        /// Used for testing purposes.
        /// </summary>
        protected Color boundsColor = Color.White;

        /// <summary>
        /// The BoundingBox for this Entity. The position of the transform is its midpoint.
        /// </summary>
        protected BoundingBox bounds;
        protected bool drawBounds = true;

        protected bool solid;
        
        public Entity(List<Entity> entities, Level lvl, EntityType type, Transform transform, Vector3 boundsSize)
        {
            this.entities = entities;
            this.lvl = lvl;
            this.type = type;

            this.transform = transform;

            this.boundsSize = boundsSize;
            bounds=Globals.GenerateBounds(transform, boundsSize);
        }

        public virtual void Update(GameTime gameTime)
        {

        }

        //Thinking about removing the GraphicsDevice/BasicEffect again?
        /// <summary>
        /// To be defined by children. At base level it just draws the bounds of the Entity
        /// </summary>
        /// <param name="graphics"></param>
        /// <param name="gameTime"></param>
        /// <param name="effect"></param>
        /// <param name="camera"></param>
        public virtual void Draw(GraphicsDevice graphics, GameTime gameTime, BasicEffect effect, Camera camera)
        {
            if (drawBounds)
            {
                Globals.DrawBounds(bounds, boundsColor, graphics, effect);
            }
        }

        public virtual void OnCollision(Entity entity)
        {

        }

        //TODO 2: Create properties instead of getter/setters (not only in Entity.cs, in general)
        public EntityType GetEntityType()
        {
            return type;
        }

        public Transform GetTransform()
        {
            return transform;
        }

        public Vector3 GetBoundsSize()
        {
            return boundsSize;
        }

        public BoundingBox GetBounds()
        {
            return bounds;
        }

        /// <summary>
        /// Universal Move Method that moves all of an Entity's components at once.
        /// </summary>
        /// <param name="velocity"></param>
        public virtual void Move(Vector3 velocity)
        {
            transform.position+=velocity;
            bounds = Globals.GenerateBounds(transform, boundsSize);
        }

        /// <summary>
        /// Workaround for a design error (having just a List of entities without encapsulation).
        /// </summary>
        public virtual void Destroy()
        {

        }

        public bool isSolid()
        {
            return solid;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Replica
{
    class Entity
    {
        //BoundingBox, Model, Texture, Duration
        protected List<Entity> entities;

        protected Transform transform;
        protected BoundingBox bounds;

        public Entity(List<Entity> entities)
        {
            this.entities = entities;

            transform = new Transform();
            bounds = new BoundingBox();
        }

        public virtual void Update(GameTime gameTime)
        {

        }

        //Thinking about removing the BasicEffect again?
        public virtual void Draw(GameTime gameTime, BasicEffect effect, Camera camera)
        {

        }

        public virtual void OnCollision(Entity entity)
        {

        }

        public Transform GetTransform()
        {
            return transform;
        }

        public BoundingBox GetBounds()
        {
            return bounds;
        }
    }
}

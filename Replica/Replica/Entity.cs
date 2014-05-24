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

        public Entity(List<Entity> entities)
        {
            this.entities = entities;

            transform = new Transform();
        }

        public virtual void Update(GameTime gameTime)
        {

        }

        //Thinking about removing the BasicEffect again?
        public virtual void Draw(GameTime gameTime, BasicEffect effect, Camera camera)
        {

        }

        public Transform GetTransform()
        {
            return transform;
        }
    }
}

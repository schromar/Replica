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

        protected Color boundsColor; //For testing purposes

        public Entity(List<Entity> entities)
        {
            this.entities = entities;

            transform = new Transform();
            bounds = new BoundingBox();

            boundsColor = Color.White;
        }

        public virtual void Update(GameTime gameTime)
        {

        }

        //Thinking about removing the GraphicsDevice/BasicEffect again?
        public virtual void Draw(GraphicsDevice graphics, GameTime gameTime, BasicEffect effect, Camera camera)
        {
            //Copypasta is strong on this one
            short[] bBoxIndices = {
                0, 1, 1, 2, 2, 3, 3, 0, // Front edges
                4, 5, 5, 6, 6, 7, 7, 4, // Back edges
                0, 4, 1, 5, 2, 6, 3, 7 // Side edges connecting front and back
                                  };

            Vector3[] corners = bounds.GetCorners();
            VertexPositionColor[] primitiveList = new VertexPositionColor[corners.Length];

            // Assign the 8 box vertices
            for (int i = 0; i < corners.Length; i++)
            {
                primitiveList[i] = new VertexPositionColor(corners[i], boundsColor);
            }

            // Draw the box with a LineList
            foreach (EffectPass pass in effect.CurrentTechnique.Passes)
            {
                pass.Apply();
                graphics.DrawUserIndexedPrimitives(PrimitiveType.LineList, primitiveList, 0, 8, bBoxIndices, 0, 12);
            }
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

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
        public enum EntityType
        {
            Block,
            Player,
            FootSensor,
            Replicant,
            Door,
            Switch,
            Goal 
        }
        
        protected List<Entity> entities;
        protected Level lvl;
        protected EntityType type;

        protected Transform transform;
        protected Vector3 boundsSize;
        protected Color boundsColor; //For testing purposes
        protected BoundingBox bounds;

        public Entity(List<Entity> entities, Level lvl, EntityType type, Transform transform, Vector3 boundsSize)
        {
            this.entities = entities;
            this.lvl = lvl;
            this.type = type;


            this.transform = transform;
            this.boundsSize = boundsSize;
            boundsColor = Color.White;
            GenerateBounds();
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

        public EntityType GetEntityType()
        {
            return type;
        }

        public Transform GetTransform()
        {
            return transform;
        }

        public BoundingBox GetBounds()
        {
            return bounds;
        }

        public virtual void SetPosition(Vector3 position)
        {
            transform.position = position;
            GenerateBounds();
        }

        void GenerateBounds()
        {
            bounds = new BoundingBox();
            bounds.Min = transform.position - boundsSize / 2.0f;
            bounds.Max = transform.position + boundsSize / 2.0f;
        }
    }
}

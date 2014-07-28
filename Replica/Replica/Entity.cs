using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Replica.Statics;

namespace Replica
{
    public enum EntityType
    {
        Block,
            Conveyor,
            Door,
            Glass,
            Goal,
            JumpPad,
            Switch,
        ImitatingReplicant,
        Player,
        Replicant,
        Trigger
    }

    class Entity
    {
        
        
        /// <summary>
        /// Each Entity needs a List of entities to be able to modify other entities
        /// </summary>
        protected List<Entity> entities;
        protected Level lvl;
        protected EntityType type;
        public EntityType Type { get { return type; } }

        protected Transform t;
        public Transform T { get { return t; } }

        protected Vector3 boundsSize;
        public Vector3 BoundsSize { get { return boundsSize; } }
        /// <summary>
        /// Used for testing purposes.
        /// </summary>
        protected Color boundsColor = Color.White;

        /// <summary>
        /// The BoundingBox for this Entity. The position of the transform is its midpoint.
        /// </summary>
        protected BoundingBox bounds;
        public BoundingBox Bounds { get { return bounds; } }
        protected bool drawBounds = true;

        protected bool solid;
        public bool Solid { get { return solid; } }
        
        public Entity(List<Entity> entities, Level lvl, EntityType type, Transform transform, Vector3 boundsSize)
        {
            this.entities = entities;
            this.lvl = lvl;
            this.type = type;

            this.t = transform;

            this.boundsSize = boundsSize;
            bounds=Globals.GenerateBounds(transform, boundsSize);
        }

        public virtual void Update(GameTime gameTime, AudioListener listener)
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

        /// <summary>
        /// Universal Move Method that moves all of an Entity's components at once.
        /// </summary>
        /// <param name="velocity"></param>
        public virtual void Move(Vector3 velocity)
        {
            t.position+=velocity;
            bounds = Globals.GenerateBounds(t, boundsSize);
        }

        /// <summary>
        /// Workaround for a design error (having just a List of entities without encapsulation).
        /// </summary>
        public virtual void Destroy()
        {

        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Replica.Statics;

namespace Replica.Entities
{
    

    class Block : Entity
    {
        /// <summary>
        /// TODO 1: Workaround for now. Currently either both Block and Child classes or neither can draw a Model
        /// </summary>
        protected bool draw = true;

        public Block(List<Entity> entities, Level lvl, Transform transform, Vector3 boundsSize, EntityType type = EntityType.Block)
            : base(entities, lvl, type, transform, boundsSize)
        {
            drawBounds = false;
            solid = true;
            t.Rotation = new Vector2();
        }

        public override void Draw(GraphicsDevice graphics, GameTime gameTime, BasicEffect effect, Camera camera)
        {
            if (draw)
            {
                
                Globals.DrawModel(Assets.wallModel, t, 2, 1, camera);
            }

            base.Draw(graphics, gameTime, effect, camera);
        }
    }
}

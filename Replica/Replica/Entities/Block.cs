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

        //TODO 2: Is this class necessary?!
        public Block(List<Entity> entities, Level lvl, Transform transform, Vector3 boundsSize, EntityType type = EntityType.Block)
            : base(entities, lvl, type, transform, boundsSize)
        {
            solid = true;
        }


        public override void Draw(GraphicsDevice graphics, GameTime gameTime, BasicEffect effect, Camera camera)
        {
            transform.Rotation=new Vector2();
            Globals.DrawModel(Assets.wallModel, transform,0.005f, camera);

            base.Draw(graphics, gameTime, effect, camera);
        }
    }
}

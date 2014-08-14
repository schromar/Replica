using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Replica.Statics;

namespace Replica.Entities.Blocks
{
    class Antiblock : Block
    {

        public Antiblock(List<Entity> entities, Level lvl, Transform transform, Vector3 boundsSize)
            : base(entities, lvl, transform, boundsSize, EntityType.Antiblock)
        {
            
            drawBounds = false;
            solid = false;

            draw = false;
        }

        public override void Update(GameTime gameTime, AudioListener listener)
        {
            
        }

        public override void Draw(GraphicsDevice graphics, GameTime gameTime, BasicEffect effect, Camera camera)
        {
            base.Draw(graphics, gameTime, effect, camera);
        }
    }
}
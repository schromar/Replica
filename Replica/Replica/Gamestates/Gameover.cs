using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

using Replica.Entities;
using Replica.Entities.Blocks;
using Replica.Statics;

namespace Replica.Gamestates
{
    public class Gameover : Gamestate
    {
        public void Init(GraphicsDevice gDevice)
        {
        }

        public eGamestates Update(GameTime gameTime)
        {           
            return eGamestates.GameOver;
        }

        public void Draw(GameTime gameTime)
        {
        }
    }
}

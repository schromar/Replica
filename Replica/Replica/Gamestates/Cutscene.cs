﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Replica.Gamestates
{
    public class Cutscene : Gamestate
    {
        public void Init(GraphicsDevice gDevice)
        {
        }

        public eGamestates Update(GameTime gameTime)
        {
            return eGamestates.Cutscene;
        }

        public void Draw(GameTime gameTime)
        {
        }
    }
}

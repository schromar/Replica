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

namespace Replica
{
    public class Ingame : Gamestate
    {
        List<Entity> entities;

        Player player;
        Level lvl;

        public void init()
        {
          

        }

        public eGamestates update()
        {
            if(Input.isClicked(Microsoft.Xna.Framework.Input.Keys.Escape))
                return eGamestates.MainMenu;

            return eGamestates.InGame;
        }

        public void draw()
        {
        }
    }
}

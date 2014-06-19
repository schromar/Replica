using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Replica
{
    public class Ingame : Gamestate
    {
        public void init()
        {
        }

        public eGamestates update()
        {
            return eGamestates.InGame;
        }

        public void draw()
        {
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Replica
{
    public class Cutscene : Gamestate
    {
        public void init()
        {
        }

        public eGamestates update()
        {
            return eGamestates.Cutscene;
        }

        public void draw()
        {
        }
    }
}

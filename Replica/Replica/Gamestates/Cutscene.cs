using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Replica.Gamestates
{
    public class Cutscene : Gamestate
    {
        public void Init()
        {
        }

        public eGamestates Update()
        {
            return eGamestates.Cutscene;
        }

        public void Draw()
        {
        }
    }
}

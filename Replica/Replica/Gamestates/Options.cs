using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Replica.Gamestates
{
    public class Options : Gamestate
    {
        public void Init()
        {
        }

        public eGamestates Update()
        {
            return eGamestates.Options;
        }

        public void Draw()
        {
        }

    }
}

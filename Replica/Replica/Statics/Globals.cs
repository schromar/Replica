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

namespace Replica.Statics
{
    public class Globals
    {
        public static String currentLvl;
        public static eGamestates currentState = eGamestates.MainMenu;
        public static eGamestates prevState = eGamestates.MainMenu;
        public static bool reachedGoal = false;

        public static int windowheight = 480;
        public static int windowwidth = 800;
    }
}

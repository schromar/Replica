using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;


namespace Replica
{
    public static class Input
    {
        public static KeyboardState currentKeyboard;
        public static KeyboardState prevKeyboard;

        public static bool isClicked(Keys key)
        {
            return currentKeyboard.IsKeyDown(key) && prevKeyboard.IsKeyUp(key);
        }

        public static bool isPressed(Keys key)
        {
            return currentKeyboard.IsKeyDown(key);
        }
    }
}


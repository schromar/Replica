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
    public class Intro : Gamestate
    {
        String text;

        public void Init(GraphicsDevice gDevice)
        {
            text = "Ok, this is the situation: \n" + "Your boss, the professor has kidnapped you \n" + "and now you are locked in his basement, \n" + "a very strange device attached to your hand. \n" + "Until you find a way out, \n" + "You'll have to play his games. \n \n" +"Good luck!";
        }
            

        public eGamestates Update(GameTime gameTime)
        {

            if (Input.isClicked(Microsoft.Xna.Framework.Input.Keys.Space) || Input.isLeftClicked())
            {
                return eGamestates.InGame;
            }
            
            return eGamestates.Intro;
        }

        public void Draw(GameTime gameTime)
        {
            Game1.spriteBatch.Draw(Assets.pix, new Rectangle(0, 0, Globals.resolutionWidht, Globals.resolutionHeight), Color.Black);

            Game1.spriteBatch.DrawString(Assets.font1, text, new Vector2(10, 10), Color.White, 0f, new Vector2(0, 0), 2, SpriteEffects.None, 0f);
        }


    }
}

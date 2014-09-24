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
    public class Ending : Gamestate
    {
        String text;

        public void Init(GraphicsDevice gDevice)
        {
            text = "Unfortunately, the end to our exciting story  \n" + "didn't make it into the game. \n" + "Just imagine that you freed yourself, \n" + "saved the whole world and got the girl \n" +"(Who didn't make it into the game either). \n" + "You should be proud of yourself. \n" + "\n" + "- The End -";
        }


        public eGamestates Update(GameTime gameTime)
        {

            if (Input.isClicked(Microsoft.Xna.Framework.Input.Keys.Space) || Input.isLeftClicked())
            {
                return eGamestates.Credits;
            }

            return eGamestates.Ending;
        }

        public void Draw(GameTime gameTime)
        {
            Game1.spriteBatch.Draw(Assets.pix, new Rectangle(0, 0, Globals.resolutionWidht, Globals.resolutionHeight), Color.Black);

            Game1.spriteBatch.DrawString(Assets.font1, text, new Vector2(10, 10), Color.White, 0f, new Vector2(0, 0), 2, SpriteEffects.None, 0f);
        }


    }
}

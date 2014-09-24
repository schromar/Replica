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
    public class Mainmenu  : Gamestate 

    {
        Button playbutton;
        Button exitbutton;
        Button loadbutton;
        Button creditsbutton;
        float buttonX;
        float buttonY;

        public void Init(GraphicsDevice gDevice)
        {
            buttonX = gDevice.Viewport.Width * 0.44f;
            buttonY = gDevice.Viewport.Height * 0.13f;

            playbutton = new Button(Assets.lvl_clear, Game1.graphics.GraphicsDevice, 0, " START");
            playbutton.setPosition(new Vector2(buttonX, buttonY * 3));

            loadbutton = new Button(Assets.lvl_clear, Game1.graphics.GraphicsDevice, 0, " LEVEL");
            loadbutton.setPosition(new Vector2(buttonX, buttonY * 4));

            creditsbutton = new Button(Assets.lvl_clear, Game1.graphics.GraphicsDevice, 0, "CREDITS");
            creditsbutton.setPosition(new Vector2(buttonX, buttonY * 5));

            exitbutton = new Button(Assets.lvl_clear, Game1.graphics.GraphicsDevice, 0, " EXIT");
            exitbutton.setPosition(new Vector2(buttonX, buttonY * 6));

            if (!Globals.playmusic)
            {
                MediaPlayer.Play(Assets.song[Globals.random.Next(Assets.song.Length)]);
                MediaPlayer.IsRepeating = true;
                Globals.playmusic = true;
            }
        }

        public eGamestates Update(GameTime gameTime)
        {


            playbutton.Update(Input.currentMouse, Input.prevMouse);
            exitbutton.Update(Input.currentMouse, Input.prevMouse);
            loadbutton.Update(Input.currentMouse, Input.prevMouse);
            creditsbutton.Update(Input.currentMouse, Input.prevMouse);

            if (playbutton.IsClicked())
            {
                if (Globals.newGame)
                {
                    Globals.newGame = false;
                    return eGamestates.Intro;
                }
                return eGamestates.InGame;
            }
            if (exitbutton.IsClicked() || Input.isClicked(Keys.Escape) || GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
            {
                return eGamestates.LeaveGame;
            }
            if (loadbutton.IsClicked())
            {
                return eGamestates.Levelselection;
            }
            if (creditsbutton.IsClicked())
            {
                return eGamestates.Credits;
            }
            return eGamestates.MainMenu;
        }

        public void Draw(GameTime gameTime)
        {
            Game1.spriteBatch.Draw(Assets.mainmenu, new Rectangle(0, 0, Globals.resolutionWidht, Globals.resolutionHeight), Color.White);

            playbutton.Draw(Game1.spriteBatch);           
            exitbutton.Draw(Game1.spriteBatch);
            loadbutton.Draw(Game1.spriteBatch);
            creditsbutton.Draw(Game1.spriteBatch);
        }
         

    }
}

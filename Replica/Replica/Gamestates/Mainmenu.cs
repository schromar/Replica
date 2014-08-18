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
        float buttonX;
        float buttonY;

        public void Init(GraphicsDevice gDevice)
        {
            buttonX = gDevice.Viewport.Width * 0.44f;
            buttonY = gDevice.Viewport.Height * 0.11f;

            playbutton = new Button(Assets.play, Game1.graphics.GraphicsDevice);
            playbutton.setPosition(new Vector2(buttonX, buttonY * 2));

            exitbutton = new Button(Assets.exit, Game1.graphics.GraphicsDevice);
            exitbutton.setPosition(new Vector2(buttonX, buttonY * 3));

            loadbutton = new Button(Assets.levelselection, Game1.graphics.GraphicsDevice);
            loadbutton.setPosition(new Vector2(buttonX, buttonY * 4));
        }

        public eGamestates Update(GameTime gameTime)
        {


            playbutton.Update(Input.currentMouse, Input.prevMouse);
            exitbutton.Update(Input.currentMouse, Input.prevMouse);
            loadbutton.Update(Input.currentMouse, Input.prevMouse);

            if (playbutton.isClicked)
            {
                return eGamestates.InGame;
            }
            if (exitbutton.isClicked || Input.isClicked(Keys.Escape) || GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
            {
                return eGamestates.LeaveGame;
            }
            if (loadbutton.isClicked)
            {
                return eGamestates.Levelselection;
            }          
            return eGamestates.MainMenu;
        }

        public void Draw(GameTime gameTime)
        {
            Game1.spriteBatch.Draw(Assets.dna, new Rectangle(0, 0, Globals.resolutionWidht, Globals.resolutionHeight), Color.White);

            playbutton.Draw(Game1.spriteBatch);           
            exitbutton.Draw(Game1.spriteBatch);
            loadbutton.Draw(Game1.spriteBatch);
        }
         

    }
}

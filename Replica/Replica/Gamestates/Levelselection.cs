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
    public class Levelselection : Gamestate
    {
        Button lvl00button;
        Button lvl01button;
        Button lvl02button;
        Button lvl03button;
        Button lvl04button;

        int buttonX;
        int buttonY;

        public void Init(GraphicsDevice gDevice)
        {
            buttonX = (int)(Globals.resolutionWidht * 0.45f);
            buttonY = (int)(Globals.resolutionHeight * 0.11f);

            lvl00button = new Button(Assets.lvl00, Game1.graphics.GraphicsDevice);
            lvl00button.setPosition(new Vector2(buttonX, buttonY * 2));

            lvl01button = new Button(Assets.lvl01, Game1.graphics.GraphicsDevice);
            lvl01button.setPosition(new Vector2(buttonX, buttonY * 3));

            lvl02button = new Button(Assets.lvl02, Game1.graphics.GraphicsDevice);
            lvl02button.setPosition(new Vector2(buttonX, buttonY * 4));

            lvl03button = new Button(Assets.lvl02, Game1.graphics.GraphicsDevice);
            lvl03button.setPosition(new Vector2(buttonX, buttonY * 5));

            lvl04button = new Button(Assets.lvl02, Game1.graphics.GraphicsDevice);
            lvl04button.setPosition(new Vector2(buttonX, buttonY * 6));
        }

        public eGamestates Update(GameTime gameTime)
        {
            lvl00button.Update(Input.currentMouse, Input.prevMouse);
            lvl01button.Update(Input.currentMouse, Input.prevMouse);
            lvl02button.Update(Input.currentMouse, Input.prevMouse);
            lvl03button.Update(Input.currentMouse, Input.prevMouse);
            lvl04button.Update(Input.currentMouse, Input.prevMouse);

            if (lvl00button.isClicked)
            {
                Globals.levelnamecounter = 0;
                if (Globals.levelnamecounter >= Globals.highesstreachedlvl)
                {
                    Globals.levelnamecounter = Globals.highesstreachedlvl;
                }
                Globals.currentLvl = Globals.levelnames[Globals.levelnamecounter];
                return eGamestates.InGame;
            }
            if (lvl01button.isClicked)
            {
                Globals.levelnamecounter = 1;
                if (Globals.levelnamecounter >= Globals.highesstreachedlvl)
                {
                    Globals.levelnamecounter = Globals.highesstreachedlvl;
                }
                Globals.currentLvl = Globals.levelnames[Globals.levelnamecounter];
                return eGamestates.InGame;
            }
                if (lvl02button.isClicked)
                {
                    Globals.levelnamecounter = 2;
                    if (Globals.levelnamecounter >= Globals.highesstreachedlvl)
                    {
                        Globals.levelnamecounter = Globals.highesstreachedlvl;
                    }
                    Globals.currentLvl = Globals.levelnames[Globals.levelnamecounter];
                    return eGamestates.InGame;
                }

                if (lvl03button.isClicked)
                {
                    Globals.levelnamecounter = 3;
                    if (Globals.levelnamecounter >= Globals.highesstreachedlvl)
                    {
                        Globals.levelnamecounter = Globals.highesstreachedlvl;
                    }
                    Globals.currentLvl = Globals.levelnames[Globals.levelnamecounter];
                    return eGamestates.InGame;
                }

                if (lvl04button.isClicked)
                {
                    Globals.levelnamecounter = 4;
                    if (Globals.levelnamecounter >= Globals.highesstreachedlvl)
                    {
                        Globals.levelnamecounter = Globals.highesstreachedlvl;
                    }
                    Globals.currentLvl = Globals.levelnames[Globals.levelnamecounter];
                    return eGamestates.InGame;
                }

                if (Input.isClicked(Keys.Escape))
                    return eGamestates.MainMenu;

                return eGamestates.Levelselection;
            }
        

        public void Draw(GameTime gameTime)
        {
            Game1.spriteBatch.Draw(Assets.dna, new Rectangle(0, 0, Globals.resolutionWidht, Globals.resolutionHeight), Color.White);

            lvl00button.Draw(Game1.spriteBatch);
            lvl01button.Draw(Game1.spriteBatch);
            lvl02button.Draw(Game1.spriteBatch);
            lvl03button.Draw(Game1.spriteBatch);
            lvl04button.Draw(Game1.spriteBatch);
        }

    }
}

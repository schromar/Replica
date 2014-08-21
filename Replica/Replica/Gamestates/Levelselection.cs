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
        List<Button> buttons;

        int buttonX_1;
        int buttonX_2;
        int buttonX_3;
        int buttonY;

        int buttonsPerColumn = 6;

        public void Init(GraphicsDevice gDevice)
        {
            buttonX_1 = (int)(Globals.resolutionWidht * 0.2f);
            buttonX_2 = (int)(Globals.resolutionWidht * 0.4f);
            buttonX_3 = (int)(Globals.resolutionWidht * 0.6f);

            buttonY = (int)(Globals.resolutionHeight * 0.11f);

            buttons = new List<Button>();

            int curColumn = 1;
            int curLine = 1;

            for (int i = 0; i < Globals.levelnames.Length; i++)
            {
                String name = "LVL ";

                if (i < 9)
                    name += "0";

                name += i + 1;
                Button curButton = new Button(Assets.lvl_clear, Game1.graphics.GraphicsDevice, i, name);
                curButton.setPosition(new Vector2 (buttonX_1 * curColumn,buttonY *  (curLine+1)));

                buttons.Add(curButton);

                curLine++;

                if (curLine > buttonsPerColumn)
                {
                    curColumn++;
                    curLine = 1;
                }
            }
        }

        public eGamestates Update(GameTime gameTime)
        {
            

            for (int i = 0; i < buttons.Count; i++)
            {
                buttons[i].Update(Input.currentMouse, Input.prevMouse);

                if (buttons[i].IsClicked())
                {
                    if (buttons[i].GetIndex() <= Globals.highesstreachedlvl)
                    {
                        Globals.levelnamecounter = buttons[i].GetIndex();
                        Globals.currentLvl = Globals.levelnames[Globals.levelnamecounter];
                        return eGamestates.InGame;
                    }
                }

            }
            if (Input.isClicked(Keys.Escape))
                return eGamestates.MainMenu;

            return eGamestates.Levelselection;
         }
        

        public void Draw(GameTime gameTime)
        {
            Game1.spriteBatch.Draw(Assets.dna, new Rectangle(0, 0, Globals.resolutionWidht, Globals.resolutionHeight), Color.White);

            for (int i = 0; i < buttons.Count; i++)
            {
                buttons[i].Draw(Game1.spriteBatch);
            }
        }

    }
}

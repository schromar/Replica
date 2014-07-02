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

        public void Init()
        {
            lvl00button = new Button(Assets.lvl00, Game1.graphics.GraphicsDevice);
            lvl00button.setPosition(new Vector2(350, 100));

            lvl01button = new Button(Assets.lvl01, Game1.graphics.GraphicsDevice);
            lvl01button.setPosition(new Vector2(350, 200));

            lvl02button = new Button(Assets.lvl02, Game1.graphics.GraphicsDevice);
            lvl02button.setPosition(new Vector2(350, 400));
        }

        public eGamestates Update()
        {
            lvl00button.Update(Mouse.GetState());
            lvl01button.Update(Mouse.GetState());
            lvl02button.Update(Mouse.GetState());

            if (lvl00button.isClicked)
            {
                Globals.currentLvl = "01_OneButton";
                return eGamestates.InGame;
            }
            if (lvl01button.isClicked)
            {
                Globals.currentLvl = "02_TwoButtons";
                return eGamestates.InGame;
            }

            if (lvl02button.isClicked)
            {
                Globals.currentLvl = "03_Leapfrogging";
                return eGamestates.InGame;
            }

            if(Input.isClicked(Keys.Escape))
                return eGamestates.MainMenu;

            return eGamestates.Levelselection;
        }

        public void Draw()
        {
            Game1.spriteBatch.Draw(Assets.dna, new Rectangle(0, 0, Assets.dna.Width, Assets.dna.Height), Color.White);

            lvl00button.Draw(Game1.spriteBatch);
            lvl01button.Draw(Game1.spriteBatch);
            lvl02button.Draw(Game1.spriteBatch);
        }

    }
}

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

        public void Init(GraphicsDevice gDevice)
        {
            lvl00button = new Button(Assets.lvl00, Game1.graphics.GraphicsDevice);
            lvl00button.setPosition(new Vector2(350, 100));

            lvl01button = new Button(Assets.lvl01, Game1.graphics.GraphicsDevice);
            lvl01button.setPosition(new Vector2(350, 175));

            lvl02button = new Button(Assets.lvl02, Game1.graphics.GraphicsDevice);
            lvl02button.setPosition(new Vector2(350, 250));

            lvl03button = new Button(Assets.lvl02, Game1.graphics.GraphicsDevice);
            lvl03button.setPosition(new Vector2(350, 325));

            lvl04button = new Button(Assets.lvl02, Game1.graphics.GraphicsDevice);
            lvl04button.setPosition(new Vector2(350, 400));
        }

        public eGamestates Update(GameTime gameTime)
        {
            lvl00button.Update(Mouse.GetState());
            lvl01button.Update(Mouse.GetState());
            lvl02button.Update(Mouse.GetState());
            lvl03button.Update(Mouse.GetState());
            lvl04button.Update(Mouse.GetState());

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
                Globals.currentLvl = "05_LockedIn";
                return eGamestates.InGame;
            }

            if (lvl03button.isClicked)
            {
                Globals.currentLvl = "06_Bridge";
                return eGamestates.InGame;
            }

            if (lvl04button.isClicked)
            {
                Globals.currentLvl = "07_TwoReplicantsThreeButtons";
                return eGamestates.InGame;
            }

            if(Input.isClicked(Keys.Escape))
                return eGamestates.MainMenu;

            return eGamestates.Levelselection;
        }

        public void Draw(GameTime gameTime)
        {
            Game1.spriteBatch.Draw(Assets.dna, new Rectangle(0, 0, Assets.dna.Width, Assets.dna.Height), Color.White);

            lvl00button.Draw(Game1.spriteBatch);
            lvl01button.Draw(Game1.spriteBatch);
            lvl02button.Draw(Game1.spriteBatch);
            lvl03button.Draw(Game1.spriteBatch);
            lvl04button.Draw(Game1.spriteBatch);
        }

    }
}

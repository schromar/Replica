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
    public class Credits : Gamestate
    {
        public void Init(GraphicsDevice gDevice)
        {
        }

        public eGamestates Update(GameTime gameTime)
        {
            if (Input.isClicked(Keys.Escape) || GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                return eGamestates.MainMenu;

            return eGamestates.Credits;
        }

        public void Draw(GraphicsDevice graphicDevice, GameTime gameTime)
        {
            Game1.spriteBatch.Draw(Assets.happy, new Rectangle(0, 0, Assets.happy.Width, Assets.happy.Height), Color.White);

        }
    }
}

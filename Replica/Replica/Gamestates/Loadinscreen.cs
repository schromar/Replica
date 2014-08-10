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
    class Loadinscreen : Gamestate
    {
        TimeSpan TargetElapsedTime = new TimeSpan(0, 0, 5);
        GameTime loading = new GameTime();

        public void Init(GraphicsDevice gDevice)
        {
        }

        public eGamestates Update(GameTime gameTime)
        {
            if (Input.isClicked(Microsoft.Xna.Framework.Input.Keys.Space))
            {
                return eGamestates.InGame;
            }
            return eGamestates.Loadingscreen;

        }
        public void Draw(GameTime gameTime)
        {
            Game1.spriteBatch.Draw(Assets.loading, new Rectangle(0, 0, Assets.loading.Width, Assets.loading.Height), Color.White);
        }


    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Replica.Statics;

namespace Replica.Drawables
{
    class OnScreenText : Drawable
    {
        /// <summary>
        /// The text to be displayed. Will usually be passed in from the current lvl.
        /// </summary>
        string text;
        /// <summary>
        /// How long the text will be drawn.
        /// </summary>
        float existenceTime = 5;
        public float ExistenceTime { get { return existenceTime; } }

        public OnScreenText(string text)
        {
            this.text = text;
        }

        public override void Update(GameTime gameTime)
        {
            existenceTime -= (float)gameTime.ElapsedGameTime.TotalSeconds;
            base.Update(gameTime);
        }

        public override void Draw()
        {
            //The text will start to fade out once a certain amount of time is left
            if (existenceTime < 1.5f)
            {
                Game1.spriteBatch.DrawString(Assets.font1, text, new Vector2(0, 375), Color.White * (existenceTime / 1.5f));
            }
            else
            {
                Game1.spriteBatch.DrawString(Assets.font1, text, new Vector2(0, 375), Color.White);
            }
            base.Draw();
        }
    }
}

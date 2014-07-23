using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Replica.Statics;

namespace Replica.Drawables
{
    class LevelText : Drawable
    {
        string text;
        float existenceTime = 5;
        public float ExistenceTime { get { return existenceTime; } }

        public LevelText(string text)
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

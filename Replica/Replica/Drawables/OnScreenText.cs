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
        float textExistenceTime = 3;
        

        float fadeout = 1.5f;

        public float TextExistenceTime { get { return textExistenceTime; } }
        public float ExistenceTime  { get { return existenceTime; } }

        public OnScreenText(string text)
        {
            this.text = text;
        }

        public override void Initialize()
        {
            base.Initialize();
            existenceTime = 4.25f;
        }

        public override void Update(GameTime gameTime)
        {
            textExistenceTime -= (float)gameTime.ElapsedGameTime.TotalSeconds;
            existenceTime  -= (float)gameTime.ElapsedGameTime.TotalSeconds;
            base.Update(gameTime);
        }

        public override void Draw()
        {
            //The text and the box will start to fade out once a certain amount of time has passed

            if (existenceTime < fadeout)
            {
                Game1.spriteBatch.Draw(Assets.textbox, new Rectangle(495, 5, Assets.textbox.Width, Assets.textbox.Height), Color.White * (existenceTime / fadeout));
            }
            else
            {
                Game1.spriteBatch.Draw(Assets.textbox, new Rectangle(495, 5, Assets.textbox.Width, Assets.textbox.Height), Color.White);
            }

            if (textExistenceTime < fadeout)
            {
                Game1.spriteBatch.DrawString(Assets.font1, text, new Vector2(495, 10), Color.White * (textExistenceTime / fadeout));
            }
            else
            {
                Game1.spriteBatch.DrawString(Assets.font1, text, new Vector2(495, 10), Color.White);
                
            }
            base.Draw();
        }
    }
}

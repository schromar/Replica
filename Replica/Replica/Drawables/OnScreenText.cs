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
        string currentText;

        int pos;
        int textY = 10;
        int linebreakCount;

        int boxX;
        int boxY;

        int count;

        List<int> linebreaks;

        bool done = false;
        /// <summary>
        /// How long the text will be drawn.
        /// </summary>
        float textExistenceTime;

        float textSpeedMax;
        float textSpeed;
        

        float fadeout = 1.5f;

        public float TextExistenceTime { get { return textExistenceTime; } }
        

        public OnScreenText(string text)
        {
            this.text = text;
        }

        public override void Initialize()
        {
            base.Initialize();
            pos = 0;
            linebreakCount = 0;
            textExistenceTime = 300000;
            existenceTime = textExistenceTime + 1.25f;
            currentText = "";
            linebreaks = new List<int>();
            count = 0;

            textSpeedMax = 0.01f;
            textSpeed = textSpeedMax;

            boxX = (int)(Globals.resolutionWidht * 0.7f);
            boxY = (int)(Globals.resolutionHeight * 0.01f);

            //count the number of lines
            
        }
        public override void Update(GameTime gameTime)
        {

            if (text == "")
                existenceTime = 0;

            currentText = text.Substring(0, pos);

            if (currentText.Length < text.Length && textSpeed < 0)
            {
                pos++;
                count++;
              
                    if (text.Substring(pos - 1, 1).Equals("\n"))
                    {
                        linebreaks.Add(count);
                        count = 0;

                        if (linebreakCount < 5)
                        {
                            Console.WriteLine("Test");
                            linebreakCount++;                          
                        }
                        else
                        {
                            //numberOfLines++;
                            text = text.Substring(linebreaks.ElementAt(0));
                            //textY -= 22;
                            pos -= linebreaks.ElementAt(0);
                            linebreaks.RemoveAt(0);
                        }
                    }                
                textSpeed = textSpeedMax;
            }

            if (done == false && currentText.Length >= text.Length)
            {
                existenceTime = 4 * fadeout;
                textExistenceTime = 3 * fadeout;
                done = true;
            }

            textSpeed -= (float)gameTime.ElapsedGameTime.TotalSeconds;
            textExistenceTime -= (float)gameTime.ElapsedGameTime.TotalSeconds;
            existenceTime  -= (float)gameTime.ElapsedGameTime.TotalSeconds;
            base.Update(gameTime);            
        }

        public override void Draw()
        {
            //The text and the box will start to fade out once a certain amount of time has passed

            if (existenceTime < fadeout)
            {
                Game1.spriteBatch.Draw(Assets.textbox, new Rectangle(boxX, boxY, Assets.textbox.Width, Assets.textbox.Height), Color.White * (existenceTime / fadeout));
            }
            else
            {
                Game1.spriteBatch.Draw(Assets.textbox, new Rectangle(boxX, boxY, Assets.textbox.Width, Assets.textbox.Height), Color.White);
            }

            if (textExistenceTime < fadeout)
            {
                Game1.spriteBatch.DrawString(Assets.font1, currentText, new Vector2(boxX, textY), Color.White * (textExistenceTime / fadeout));
            }
            else
            {
                Game1.spriteBatch.DrawString(Assets.font1, currentText, new Vector2(boxX, textY), Color.White);
                
            }
            base.Draw();
        }
    }
}

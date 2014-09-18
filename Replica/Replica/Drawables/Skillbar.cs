using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Replica.Statics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Replica.Entities;

namespace Replica.Drawables
{
    class Skillbar : Drawable
    {
        int skill1X;
        int skill2X;
        int skill3X;

        int skill1Size;
        int skill2Size;
        int skill3Size;

        int skill1OffsetY;
        int skill2OffsetY;
        int skill3OffsetY;

        int skillY;

        int offsetY;

        float normalScale;
        float activeScale;

        int normalSize;
        int activeSize;

        String count1;
        String count2;

        Vector2 count1Pos;
        Vector2 count2Pos;

        float count1Scale;
        float count2Scale;

        int countLength;

        Player player;

        public Skillbar(Player player)
        {
            this.player = player;
        }

        public override void Initialize()
        {

            normalSize = (int)(Globals.resolutionWidht * 0.09f);
            activeSize = (int)(Globals.resolutionWidht * 0.12f);

            skillY = (int)(Globals.resolutionHeight * 0.85f); 
            
            normalScale = 1.4f;
            activeScale = 1.9f;

            countLength = 22;

            skill1X = (int)(Globals.resolutionWidht * 0.40f);


            count1 = "0";
            count2 = "0";

            offsetY = (int) (activeSize * 0.1f);
        }

        public override void Update(GameTime gameTime)
        {
            skill1Size      = normalSize;
            skill2Size      = normalSize;
            skill3Size      = normalSize;

            count1Scale = normalScale;
            count2Scale = normalScale;

            skill1OffsetY   = 0;
            skill2OffsetY   = 0;
            skill3OffsetY   = 0;

            count1 = (Globals.normalReplicants    - player.GetReplicantCount(EntityType.Replicant)).ToString();
            count2 = (Globals.imitatingReplicants - player.GetReplicantCount(EntityType.ImitatingReplicant)).ToString();

            switch (Globals.spawnType)
            {
                case EntityType.Replicant :
                    skill1Size = activeSize;
                    count1Scale = activeScale;
                    skill1OffsetY = offsetY;
                    break;

                case EntityType.ImitatingReplicant :
                    skill2Size = activeSize;
                    count2Scale = activeScale;
                    skill2OffsetY = offsetY;
                    break;

                default :
                    break;
            }

            skill2X = skill1X + skill1Size;
            skill3X = skill2X + skill2Size;

            count1Pos = new Vector2(skill1X + skill1Size * 0.8f - (count1.Length - 1) * countLength, skillY + skill1Size * 0.68f - skill1OffsetY);
            count2Pos = new Vector2(skill2X + skill2Size * 0.8f - (count2.Length - 1) * countLength, skillY + skill2Size * 0.68f - skill2OffsetY);
        }

        public override void Draw()
        {
            Game1.spriteBatch.Draw(Assets.simpleReplicantButton, new Microsoft.Xna.Framework.Rectangle(skill1X, skillY - skill1OffsetY, skill1Size, skill1Size), Color.White);
            Game1.spriteBatch.Draw(Assets.imitatingReplicantButton, new Microsoft.Xna.Framework.Rectangle(skill2X, skillY - skill2OffsetY, skill2Size, skill2Size), Color.White);
            //Game1.spriteBatch.Draw(Assets.clearSkill, new Microsoft.Xna.Framework.Rectangle(skill3X, skillY - skill3OffsetY, skill3Size, skill3Size), Color.White);
      
            Game1.spriteBatch.DrawString(Assets.font1, count1, count1Pos, Color.Black, 0, new Vector2(0, 0), count1Scale, Microsoft.Xna.Framework.Graphics.SpriteEffects.None, 0);
            Game1.spriteBatch.DrawString(Assets.font1, count2, count2Pos, Color.Black, 0, new Vector2(0, 0), count2Scale, Microsoft.Xna.Framework.Graphics.SpriteEffects.None, 0);
                    
           
            base.Draw();
        }
    }
}

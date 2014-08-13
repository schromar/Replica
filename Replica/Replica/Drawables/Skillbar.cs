using Microsoft.Xna.Framework;
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

        int skill1CountActiveX;
        int skill2CountActiveX;
        int skill3CountActiveX;

        int skill1CountInactiveX;
        int skill2CountInactiveX;
        int skill3CountInactiveX;

        int skillCountActiveY;
        int skillCountInactiveY;
        

        

        int skill1OffsetX;
        int skill2OffsetX;
        int skill3OffsetX;

        int skill1OffsetY;
        int skill2OffsetY;
        int skill3OffsetY;

        int skillY;

        int offsetX;
        int offsetY;

        float normalScale;
        float activeScale;

        int normalSize;
        int activeSize;

        String count1;
        String count2;

        Player player;

        public Skillbar(Player player)
        {
            this.player = player;
        }

        public override void Initialize()
        {
            normalSize = 64;
            activeSize = 80;

            normalScale = 1.0f;
            activeScale = 1.2f;

            skill1CountActiveX = 363;
            skill1CountInactiveX = 349;

            skill2CountActiveX = 428;
            skill2CountInactiveX = 430;



            skillCountActiveY = 425;
            skillCountInactiveY = 421;

            count1 = "0";
            count2 = "0";

            skill1X = 300;
            skill2X = skill1X + normalSize;
            skill3X = skill2X + normalSize;

            skillY = 380;

            offsetX = activeSize - normalSize;
            offsetY = 8;
        }

        public override void Update(GameTime gameTime)
        {
            skill1Size      = normalSize;
            skill2Size      = normalSize;
            skill3Size      = normalSize;

            skill1OffsetX   = 0;
            skill2OffsetX   = 0;
            skill3OffsetX   = 0;

            skill1OffsetY   = 0;
            skill2OffsetY   = 0;
            skill3OffsetY   = 0;

            

            count1 = (Globals.normalReplicants - player.GetReplicantCount(EntityType.Replicant)).ToString();
            count2 = (Globals.imitatingReplicants - player.GetReplicantCount(EntityType.ImitatingReplicant)).ToString();
            


            switch (Globals.spawnType)
            {
                case EntityType.Replicant :
                    skill1Size = activeSize;
                    skill1OffsetX = offsetX;
                    skill1OffsetY = offsetY;
                    break;

                case EntityType.ImitatingReplicant :
                    skill2Size = activeSize;
                    skill2OffsetX = offsetX;
                    skill2OffsetY = offsetY;
                    break;

                default :
                    break;
            }

        }

        public override void Draw()
        {
            Game1.spriteBatch.Draw(Assets.simpleReplicantButton, new Microsoft.Xna.Framework.Rectangle(skill1X, skillY - skill1OffsetY, skill1Size, skill1Size), Color.White);
            Game1.spriteBatch.Draw(Assets.imitatingReplicantButton, new Microsoft.Xna.Framework.Rectangle(skill2X + skill1OffsetX, skillY - skill2OffsetY, skill2Size, skill2Size), Color.White);
            Game1.spriteBatch.Draw(Assets.clearSkill, new Microsoft.Xna.Framework.Rectangle(skill3X + skill1OffsetX + skill2OffsetX, skillY - skill3OffsetY, skill3Size, skill3Size), Color.White);

            switch (Globals.spawnType)
            {
                case EntityType.Replicant:
                    Game1.spriteBatch.DrawString(Assets.font1, count1, new Vector2(skill1CountActiveX, skillCountActiveY), Color.Black, 0, new Vector2(0, 0), activeScale, Microsoft.Xna.Framework.Graphics.SpriteEffects.None, 0);
                    Game1.spriteBatch.DrawString(Assets.font1, count2, new Vector2(skill2CountInactiveX, skillCountInactiveY), Color.Black, 0, new Vector2(0, 0), normalScale, Microsoft.Xna.Framework.Graphics.SpriteEffects.None, 0);
                    break;
                case EntityType.ImitatingReplicant:
                     Game1.spriteBatch.DrawString(Assets.font1, count1, new Vector2(skill1CountInactiveX, skillCountInactiveY), Color.Black, 0, new Vector2(0, 0), normalScale, Microsoft.Xna.Framework.Graphics.SpriteEffects.None, 0);
                     Game1.spriteBatch.DrawString(Assets.font1, count2, new Vector2(skill2CountActiveX, skillCountActiveY), Color.Black, 0, new Vector2(0, 0), activeScale, Microsoft.Xna.Framework.Graphics.SpriteEffects.None, 0);
                    break;
                default:
                    break;
            }
            
            base.Draw();
        }
    }
}

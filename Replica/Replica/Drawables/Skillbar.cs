using Microsoft.Xna.Framework;
using Replica.Statics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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

        int skill1OffsetX;
        int skill2OffsetX;
        int skill3OffsetX;

        int skill1OffsetY;
        int skill2OffsetY;
        int skill3OffsetY;

        int skillY;

        int offsetX;
        int offsetY;

        int normalSize;
        int activeSize;

        public override void Initialize()
        {
            normalSize = 64;
            activeSize = 80;

            skill1X = 300;
            skill2X = skill1X + normalSize;
            skill3X = skill2X + normalSize;

            skillY = 410;

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
            
            base.Draw();
        }
    }
}

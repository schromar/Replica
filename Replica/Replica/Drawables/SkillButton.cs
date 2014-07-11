using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Replica.Statics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Replica.Drawables
{
    class SkillButton : Drawable
    {
        EntityType type;
        int pos;
        bool active;
        Texture2D texture;
        int x;
        int y;
        int size;
        int start;

        int inactiveSize;
        int activeSize;

        int offsetX;
        int offsetY;

        public SkillButton(EntityType type, int pos)
        {
            this.type = type;
            this.pos = pos;

            if (type == EntityType.Replicant)
                texture = Assets.simpleReplicantButton;
            else if (type == EntityType.ImitatingReplicant)
                texture = Assets.imitatingReplicantButton;
            else texture = Assets.clearSkill;

            start = 300;

            inactiveSize = 64;
            activeSize = 80;

            x = start + pos * inactiveSize;
            y = 410;

            offsetX = activeSize - inactiveSize;
            offsetY = offsetX / 2;
        }

        public override void Initialize()
        {
        }

        public override void Update()
        {
            if (type == Globals.spawnType)
                active = true;
            else active = false;

            if (active)
            {
                size = 80;
            }
            else
            {
                size = 64;
            }
        }

        public override void Draw()
        {
            Game1.spriteBatch.Draw(texture, new Microsoft.Xna.Framework.Rectangle(x, y, size, size), Color.White);
        }
    }
}


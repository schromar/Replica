using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Replica.Statics;

namespace Replica
{
    class Button
    {
        Texture2D texture;
        Vector2 position;
        Rectangle rectangle;
        int index;
        String name;

        Color color = new Color(255,255,255, 255);
        public Vector2 size;

        public Button(Texture2D newTexture, GraphicsDevice gDevice, int index, String name)
        {
            texture = newTexture;
            size = new Vector2(gDevice.Viewport.Width / 8, gDevice.Viewport.Height / 15);
            this.index = index;
            this.name = name;
        }

        bool down;
        public bool clicked;
        public void Update(MouseState currentMouse, MouseState prevMouse)
        {
            rectangle = new Rectangle((int)position.X, (int)position.Y, (int)size.X, (int)size.Y);

            Rectangle mouseRec = new Rectangle(currentMouse.X, currentMouse.Y, 1, 1);
            if (mouseRec.Intersects(rectangle))
            {
                if (color.A == 255) down = false;
                if (color.A == 0) down = true;
                if (down) color.A += 3; else color.A -= 3;

                if (Input.isLeftClicked()) clicked = true;


            }
            else if (color.A < 255)
            {
                color.A += 3;
                clicked = false; 
            }
        }

        public void setPosition(Vector2 newPosition)
        {
            position = newPosition;
        }

        public bool IsClicked()
        {
            return clicked;
        }

        public int GetIndex()
        {
            return index;
        }

        public string GetName()
        {
            return name;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (index != 0 && index > Globals.highesstreachedlvl)
                spriteBatch.Draw(texture, rectangle, Color.Gray);
            else
                spriteBatch.Draw(texture, rectangle, Color.White);

           if (!name.Equals(""))
            {
                Game1.spriteBatch.DrawString(Assets.font1, name, new Vector2(position.X + 12,position.Y + 8), Color.Red, 0, new Vector2(0, 0), 1.5f, Microsoft.Xna.Framework.Graphics.SpriteEffects.None, 0);
            }

        }


    }
}

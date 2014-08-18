using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Replica
{
    class Button
    {
        Texture2D texture;
        Vector2 position;
        Rectangle rectangle;

        Color color = new Color(255,255,255, 255);
        public Vector2 size;

        public Button(Texture2D newTexture, GraphicsDevice gDevice)
        {
            texture = newTexture;
            size = new Vector2(gDevice.Viewport.Width / 8, gDevice.Viewport.Height / 15);
        }

        bool down;
        public bool isClicked;
        public void Update(MouseState currentMouse, MouseState prevMouse)
        {
            rectangle = new Rectangle((int)position.X, (int)position.Y, (int)size.X, (int)size.Y);

            Rectangle mouseRec = new Rectangle(currentMouse.X, currentMouse.Y, 1, 1);
            if (mouseRec.Intersects(rectangle))
            {
                if (color.A == 255) down = false;
                if (color.A == 0) down = true;
                if (down) color.A += 3; else color.A -= 3;

                if (Input.isLeftClicked()) isClicked = true;


            }
            else if (color.A < 255)
            {
                color.A += 3;
                isClicked = false; 
            }
        }

        public void setPosition(Vector2 newPosition)
        {
            position = newPosition;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, rectangle, Color.White);
        }


    }
}

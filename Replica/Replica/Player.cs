using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Replica
{
    class Player
    {
        Vector2 resolution;

        Vector3 position;
        Vector3 target;
        Vector3 up;

        Vector2 rotation;

        Matrix view;
        Matrix projection;

        public Player(int windowWidth, int windowHeight)
        {
            resolution = new Vector2(windowWidth, windowHeight);

            position = new Vector3(5, 1, 5);
            target = new Vector3(0, 0, 0);
            up = Vector3.Forward;

            UpdateView();
            
            projection = Matrix.CreatePerspectiveFieldOfView(MathHelper.ToRadians(90), resolution.X/resolution.Y, 0.5f, 1000.0f);
        }

        public void Update(GameTime gameTime)
        {
            //Calculate camera rotation
            //TODO: multiply with mouse speed as well
            MouseState mState=Mouse.GetState();
            Vector2 mousePosition=new Vector2(mState.X, mState.Y);
            rotation+=(resolution/2 - mousePosition)*(float)gameTime.ElapsedGameTime.TotalSeconds;

            //Math. It happens. Here.
            Vector3 direction = new Vector3((float)Math.Cos(rotation.Y) * (float)Math.Sin(rotation.X),
                                            (float)Math.Sin(rotation.Y),
                                            (float)Math.Cos(rotation.Y) * (float)Math.Cos(rotation.X));
            Vector3 right = new Vector3((float)Math.Sin(rotation.X - Math.PI / 2.0f),
                                        0,
                                        (float)Math.Cos(rotation.X - Math.PI / 2.0f));
            up = Vector3.Cross(right, direction);

            Mouse.SetPosition((int)resolution.X / 2, (int)resolution.Y / 2);

            //Calculate camera position
            //TODO: multiply with movement speed as well and normalize
            KeyboardState kState = Keyboard.GetState();
            if (kState.IsKeyDown(Keys.W))
            {
                position += direction * (float)gameTime.ElapsedGameTime.TotalSeconds;
            }
            if (kState.IsKeyDown(Keys.S))
            {
                position -= direction * (float)gameTime.ElapsedGameTime.TotalSeconds;
            }
            if (kState.IsKeyDown(Keys.A))
            {
                position -= right * (float)gameTime.ElapsedGameTime.TotalSeconds;
            }
            if (kState.IsKeyDown(Keys.D))
            {
                position += right * (float)gameTime.ElapsedGameTime.TotalSeconds;
            }

            target = position + direction;

            UpdateView();
        }

        void UpdateView()
        {
            view = Matrix.CreateLookAt(position, target, up);
        }

        public Matrix GetView()
        {
            return view;
        }

        public Matrix GetProjection()
        {
            return projection;
        }
    }
}

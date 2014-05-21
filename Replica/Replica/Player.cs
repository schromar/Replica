using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Replica
{
    class Player : Entity
    {
        Vector2 resolution;

        Vector3 position;
        Vector3 forward;
        Vector3 right;
        Vector3 up;

        float mouseSpeed;
        float movementSpeed;
        Vector2 rotation;

        Matrix view;
        Matrix projection;

        public Player(int windowWidth, int windowHeight)
        {
            resolution = new Vector2(windowWidth, windowHeight);

            position = new Vector3(5, 1, 5);
            forward = Vector3.Forward;
            right = Vector3.Right;
            up = Vector3.Up;

            mouseSpeed = 0.1f;
            movementSpeed = 5;
            rotation = Vector2.Zero;

            UpdateView();
            projection = Matrix.CreatePerspectiveFieldOfView(MathHelper.ToRadians(90), resolution.X/resolution.Y, 0.5f, 1000.0f);
        }

        public override void Update(GameTime gameTime)
        {
            RotateCamera(gameTime);
            MoveCamera(gameTime);

            UpdateView();
        }

        void RotateCamera(GameTime gameTime)
        {
            MouseState mState = Mouse.GetState();

            Vector2 mouseMovement = resolution / 2 - new Vector2(mState.X, mState.Y);
            Mouse.SetPosition((int)resolution.X / 2, (int)resolution.Y / 2);

            rotation += mouseMovement * mouseSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;

            //Math. It happens. Here.
            forward = new Vector3((float)Math.Cos(rotation.Y) * (float)Math.Sin(rotation.X),
                                            (float)Math.Sin(rotation.Y),
                                            (float)Math.Cos(rotation.Y) * (float)Math.Cos(rotation.X));
            right = new Vector3((float)Math.Sin(rotation.X - Math.PI / 2.0f),
                                        0,
                                        (float)Math.Cos(rotation.X - Math.PI / 2.0f));
            up = Vector3.Cross(right, forward);
        }

        void MoveCamera(GameTime gameTime)
        {
            KeyboardState kState = Keyboard.GetState();

            Vector2 movement = Vector2.Zero;
            if (kState.IsKeyDown(Keys.W))
            {
                movement.X += movementSpeed;
            }
            if (kState.IsKeyDown(Keys.S))
            {
                movement.X -= movementSpeed;
            }
            if (kState.IsKeyDown(Keys.A))
            {
                movement.Y -= movementSpeed;
            }
            if (kState.IsKeyDown(Keys.D))
            {
                movement.Y += movementSpeed;
            }

            if (movement.Length() > movementSpeed) //Diagonal movement must not be faster
            {
                movement.Normalize();
                movement *= movementSpeed;
            }
            movement *= (float)gameTime.ElapsedGameTime.TotalSeconds;
            position += forward * movement.X + right * movement.Y;
        }

        void UpdateView()
        {
            view = Matrix.CreateLookAt(position, position+forward, up);
        }

        public Vector3 GetPosition()
        {
            return position;
        }

        public Vector3 GetForward()
        {
            return forward;
        }

        public Vector3 GetUp()
        {
            return up;
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

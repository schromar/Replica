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

        float mouseSpeed;
        float movementSpeed;
        Vector2 rotation;

        Camera camera;

        public Player(List<Entity> entities, int windowWidth, int windowHeight)
            : base(entities)
        {
            resolution = new Vector2(windowWidth, windowHeight);

            transform.position = new Vector3(5, 1, 5);

            mouseSpeed = 0.1f;
            movementSpeed = 5;
            rotation = Vector2.Zero;

            camera = new Camera(resolution);
        }

        public override void Update(GameTime gameTime)
        {
            Rotate(gameTime);
            Move(gameTime);

            camera.SetTransform(transform);

            MouseState mState = Mouse.GetState();
            if (mState.LeftButton == ButtonState.Pressed)
            {
                entities.Add(new Replicant(entities, transform));
                Console.WriteLine(entities.Count);
            }
        }

        public Camera GetCamera()
        {
            return camera;
        }

        void Rotate(GameTime gameTime)
        {
            MouseState mState = Mouse.GetState();

            Vector2 mouseMovement = resolution / 2 - new Vector2(mState.X, mState.Y);
            Mouse.SetPosition((int)resolution.X / 2, (int)resolution.Y / 2);

            rotation += mouseMovement * mouseSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;

            //Math. It happens. Here.
            transform.forward = new Vector3((float)Math.Cos(rotation.Y) * (float)Math.Sin(rotation.X),
                                            (float)Math.Sin(rotation.Y),
                                            (float)Math.Cos(rotation.Y) * (float)Math.Cos(rotation.X));
            transform.right = new Vector3((float)Math.Sin(rotation.X - Math.PI / 2.0f),
                                        0,
                                        (float)Math.Cos(rotation.X - Math.PI / 2.0f));
            transform.up = Vector3.Cross(transform.right, transform.forward);
        }

        void Move(GameTime gameTime)
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
            transform.position += transform.forward * movement.X + transform.right * movement.Y;
        }
    }
}

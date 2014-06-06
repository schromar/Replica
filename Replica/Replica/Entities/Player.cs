using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Replica.Entities
{
    class Player : Entity
    {
        Vector2 resolution;

        float mouseSpeed;
        float movementSpeed;
        Vector2 rotation;

        Camera camera;
        Model model;

        public Player(List<Entity> entities, int windowWidth, int windowHeight, Model model)
            : base(entities)
        {
            transform.position = new Vector3(5, 1, 5);

            //Wrong assumption to simplify: camera position is middle of player model
            Vector3 boundSize = new Vector3(2, 2, 2);
            bounds.Min = transform.position - boundSize / 2.0f;
            bounds.Max = transform.position + boundSize / 2.0f;

            resolution = new Vector2(windowWidth, windowHeight);

            mouseSpeed = 0.1f;
            movementSpeed = 5;
            rotation = Vector2.Zero;

            camera = new Camera(resolution);
            this.model = model;
        }

        public override void Update(GameTime gameTime)
        {
            Rotate(gameTime);
            Move(gameTime);
            camera.SetTransform(transform);

            //Spawn Replicant on mouseclick
            MouseState mState = Mouse.GetState();
            if (mState.LeftButton == ButtonState.Pressed)
            {
                SpawnReplicant();
            }
        }

        public Camera GetCamera()
        {
            return camera; //What happens if camera is changed after getter was used?
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
            Vector2 movement = Vector2.Zero;

            if (Input.isPressed(Keys.W))
            {
                movement.X += movementSpeed;
            }
            if (Input.isPressed(Keys.S))
            {
                movement.X -= movementSpeed;
            }
            if (Input.isPressed(Keys.A))
            {
                movement.Y -= movementSpeed;
            }
            if (Input.isPressed(Keys.D))
            {
                movement.Y += movementSpeed;
            }

            if (movement.Length() > movementSpeed) //Diagonal movement must not be faster
            {
                movement.Normalize();
                movement *= movementSpeed;
            }
            movement *= (float)gameTime.ElapsedGameTime.TotalSeconds;
            Vector3 finalVelocity = transform.forward * movement.X + transform.right * movement.Y;
            transform.position += finalVelocity;

            //Move bounds with player
            bounds.Min += finalVelocity;
            bounds.Max += finalVelocity;
        }

        void SpawnReplicant()
        {
            //Create Ray from Player Transform to check if Player is looking at Entities
            Ray ray = new Ray(transform.position, transform.forward);
            List<KeyValuePair<float, Entity>> collisions = CollisionSystem.RayIntersection(entities, ray);

            //Check whether looked at Entity is solid to spawn the Replicant on
            int solidIndex = -1;
            for (int i = 0; i < collisions.Count; i++)
            {
                if (collisions[i].Value.GetType() == typeof(Block)) //TODO: RTTI!!!
                {
                    solidIndex = i;
                    break;
                }
            }

            if (solidIndex != -1) //Only spawn Replicant if we are not looking into infinity?
            {
                Transform replicantTransform = transform;
                replicantTransform.position = transform.position + transform.forward * collisions[solidIndex].Key;
                entities.Add(new Replicant(entities, replicantTransform, model));
            }
        }
    }
}

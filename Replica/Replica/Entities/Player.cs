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

        Vector3 prevMovement;
        FootSensor foot;

        float yVelocity;
        float gravity;

        public Player(List<Entity> entities, Level lvl, int windowWidth, int windowHeight, Model model)
            : base(entities, lvl, EntityType.Player, new Transform(), new Vector3(2, 2, 2))
        {
            transform.position = new Vector3(5, 100, 5);

            resolution = new Vector2(windowWidth, windowHeight);

            mouseSpeed = 0.1f;
            movementSpeed = 5;
            rotation = Vector2.Zero;

            camera = new Camera(resolution);
            this.model = model;

            prevMovement = Vector3.Zero;
            Transform footTransform=new Transform();
            footTransform.position=new Vector3(transform.position.X, bounds.Min.Y, transform.position.Z);
            foot = new FootSensor(entities, lvl, footTransform, new Vector3(0.5f));
            entities.Add(foot);

            yVelocity = 0;
            gravity = -0.25f;
        }

        public override void Update(GameTime gameTime)
        {
            Rotate(gameTime);
            MoveXZ(gameTime);
            MoveY(gameTime);
            camera.SetTransform(transform);

            //Spawn Replicant on mouseclick
            MouseState mState = Mouse.GetState();
            if (mState.LeftButton == ButtonState.Pressed)
            {
                SpawnReplicant();
            }
        }

        public override void OnCollision(Entity entity)
        {
            if (entity.GetEntityType() == EntityType.Block || entity.GetEntityType() == EntityType.Replicant)
            {
                yVelocity = 0;
                SetPosition(transform.position - prevMovement);
            }
        }

        public override void SetPosition(Vector3 position)
        {
            base.SetPosition(position);
            camera.SetTransform(transform);
            foot.SetPosition(new Vector3(transform.position.X, bounds.Min.Y, transform.position.Z));
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

        void MoveXZ(GameTime gameTime)
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
            Vector3 forwardWithoutY = transform.forward;
            forwardWithoutY.Y = 0;
            Vector3 finalVelocity = forwardWithoutY * movement.X + transform.right * movement.Y;

            SetPosition(transform.position + finalVelocity);
            prevMovement.X = finalVelocity.X;
            prevMovement.Z = finalVelocity.Z;
        }

        void MoveY(GameTime gameTime)
        {
            yVelocity = yVelocity + gravity * (float)gameTime.ElapsedGameTime.TotalSeconds;

            Vector3 jumpVector=new Vector3();
            if (foot.IsActivated())
            {
                if(Input.isPressed(Keys.Space))
                {
                    jumpVector.Y=100;
                }
            }
            Vector3 yVector = new Vector3(0, yVelocity, 0);
            yVector += jumpVector;
            SetPosition(transform.position + yVector);
            prevMovement.Y = yVector.Y;
        }

        void SpawnReplicant()
        {
            Transform replicantTransform = transform;
            replicantTransform.position = transform.position + transform.forward*boundsSize.Length();
            Replicant replicant = new Replicant(entities, lvl, replicantTransform, boundsSize, model);
            bool spawning = true;
            foreach (Entity entity in entities)
            {
                if (replicant.GetBounds().Intersects(entity.GetBounds()) && entity.GetEntityType() == EntityType.Block)
                {
                    spawning = false;
                    break;
                }
            }
            if (spawning)
            {
                entities.Add(replicant);
            }

            //ALTERNATIVE SPAWNING
            //Create Ray from Player Transform to check if Player is looking at Entities
            /*Ray ray = new Ray(transform.position, transform.forward);
            List<KeyValuePair<float, Entity>> collisions = CollisionSystem.RayIntersection(entities, ray);

            //Check whether looked at Entity is solid to spawn the Replicant on
            int solidIndex = -1;
            for (int i = 0; i < collisions.Count; i++)
            {
                if (collisions[i].Value.GetEntityType() == EntityType.Block)
                {
                    solidIndex = i;
                    break;
                }
            }

            if (solidIndex != -1) //Only spawn Replicant if we are not looking into infinity?
            {
                Transform replicantTransform = transform;
                replicantTransform.position = transform.position + transform.forward * collisions[solidIndex].Key;
                entities.Add(new Replicant(replicantTransform, model, entities, lvl));
            }*/
        }
    }
}

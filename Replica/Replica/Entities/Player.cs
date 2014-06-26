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
        Vector3 footBoundsSize;
        FootSensor foot;

        float yVelocity;
        float gravity;
        float jumpVelocity;

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
            footBoundsSize = new Vector3(1.75f, 0.2f, 1.75f); //Slightly smaller boundsSize than player
            Transform footTransform=new Transform();
            footTransform.position=new Vector3(transform.position.X, bounds.Min.Y-footBoundsSize.Y/2.0f, transform.position.Z);
            foot = new FootSensor(entities, lvl, footTransform, footBoundsSize);
            entities.Add(foot);

            yVelocity = -20;
            gravity = -0.25f;
            jumpVelocity = 10;
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
            if (entity.isSolid() && prevMovement!=Vector3.Zero)
            {
                ///Simple version
                Vector3 backwards = prevMovement;

                SetPosition(transform.position - backwards);
                //prevMovement = -backwards;
            }
        }

        public override void SetPosition(Vector3 position)
        {
            base.SetPosition(position);
            camera.SetTransform(transform);
            foot.SetPosition(new Vector3(transform.position.X, bounds.Min.Y-footBoundsSize.Y/2.0f, transform.position.Z));
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
            yVelocity = yVelocity + gravity;

            Vector3 jumpVector=new Vector3();
            if (foot.IsActivated())
            {
                //Landing
                Entity collider = foot.GetCollider();
                Vector3 newPosition = transform.position;
                newPosition.Y = collider.GetTransform().position.Y + collider.GetBoundsSize().Y / 2 + boundsSize.Y / 2 + 0.05f; //Setting player slightly above ground
                SetPosition(newPosition);
                yVelocity = 0;

                if (Input.isPressed(Keys.Space))
                {
                    yVelocity = jumpVelocity;
                }
            }
            Vector3 yVector = new Vector3(0, yVelocity, 0);
            yVector += jumpVector;
            yVector *= (float)gameTime.ElapsedGameTime.TotalSeconds;

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
                if (replicant.GetBounds().Intersects(entity.GetBounds()))
                    if (entity.GetEntityType() == EntityType.Block || entity.GetEntityType() == EntityType.Door || entity.GetEntityType() == EntityType.Goal)
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

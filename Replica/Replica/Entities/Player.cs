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

        Vector3 prevMovement;
        Vector3 movementBoundsSize;
        List<Trigger> movementBounds;

        float yVelocity;
        float gravity;
        float jumpVelocity;

        public Player(List<Entity> entities, Level lvl, Transform transform,  int windowWidth, int windowHeight)
            : base(entities, lvl, EntityType.Player, transform, new Vector3(2, 2, 2))
        {
            resolution = new Vector2(windowWidth, windowHeight);

            mouseSpeed = 0.1f;
            movementSpeed = 5;
            rotation = Vector2.Zero;

            camera = new Camera(resolution);

            prevMovement = Vector3.Zero;
            movementBoundsSize = new Vector3(1.75f, 0.2f, 1.75f); //in Y direction

            Transform t=new Transform();
            movementBounds = new List<Trigger>();
            //Y-, Y+
            Vector3 nextBoundsSize = movementBoundsSize;
            t.position = new Vector3(transform.position.X, bounds.Min.Y - nextBoundsSize.Y / 2.0f, transform.position.Z);
            Trigger trigger = new Trigger(entities, lvl, t, nextBoundsSize);
            movementBounds.Add(trigger);
            entities.Add(trigger);

            t.position = new Vector3(transform.position.X, bounds.Max.Y + nextBoundsSize.Y / 2.0f, transform.position.Z);
            trigger = new Trigger(entities, lvl, t, nextBoundsSize);
            movementBounds.Add(trigger);
            entities.Add(trigger);

            //X-, X+
            nextBoundsSize = new Vector3(movementBoundsSize.Y, movementBoundsSize.X, movementBoundsSize.Z);
            t.position = new Vector3(bounds.Min.X - nextBoundsSize.X / 2.0f, transform.position.Y, transform.position.Z);
            trigger = new Trigger(entities, lvl, t, nextBoundsSize);
            movementBounds.Add(trigger);
            entities.Add(trigger);

            t.position = new Vector3(bounds.Max.X + nextBoundsSize.X / 2.0f, transform.position.Y, transform.position.Z);
            trigger = new Trigger(entities, lvl, t, nextBoundsSize);
            movementBounds.Add(trigger);
            entities.Add(trigger);

            //Z-, Z+
            nextBoundsSize = new Vector3(movementBoundsSize.X, movementBoundsSize.Z, movementBoundsSize.Y);
            t.position = new Vector3(transform.position.X, transform.position.Y, bounds.Min.Z - nextBoundsSize.Z / 2.0f);
            trigger = new Trigger(entities, lvl, t, nextBoundsSize);
            movementBounds.Add(trigger);
            entities.Add(trigger);

            t.position = new Vector3(transform.position.X, transform.position.Y, bounds.Max.Z + nextBoundsSize.Z / 2.0f);
            trigger = new Trigger(entities, lvl, t, nextBoundsSize);
            movementBounds.Add(trigger);
            entities.Add(trigger);

            yVelocity = -20;
            gravity = -0.25f;
            jumpVelocity = 10;
        }

        public override void Update(GameTime gameTime)
        {
            HandleCollisions();

            Rotate(gameTime);
            MoveXZ(gameTime);
            MoveY(gameTime);
            camera.SetTransform(transform);

            //Spawn Replicant on mouseclick
            MouseState mState = Mouse.GetState();

            if (lvl.numberOfReplicants < lvl.maxReplicants)
            {
                //if (mState.RightButton == ButtonState.Pressed)
                if (Input.isClicked(Keys.F1))
                {
                    SpawnReplicant();
                }
            }
        }

        public override void OnCollision(Entity entity)
        {

        }

        public override void Move(Vector3 velocity)
        {
            base.Move(velocity);
            camera.SetTransform(transform);
            foreach (Trigger trigger in movementBounds)
            {
                trigger.Move(velocity);
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

            Move(finalVelocity);
            prevMovement.X = finalVelocity.X;
            prevMovement.Z = finalVelocity.Z;
        }

        void MoveY(GameTime gameTime)
        {
            yVelocity = yVelocity + gravity;
            Vector3 yVector = new Vector3(0, yVelocity, 0);
            yVector *= (float)gameTime.ElapsedGameTime.TotalSeconds;

            Move(yVector);
            prevMovement.Y = yVector.Y;
        }

        void HandleCollisions()
        {
            for (int i = 0; i < movementBounds.Count; i++)
            {
                if (movementBounds[i].IsActivated())
                {
                    float offset = 0.05f;
                    Entity collider = movementBounds[i].GetCollider();
                    Vector3 newPosition = transform.position;
                    switch (i)
                    {
                        case 0:
                            newPosition.Y = collider.GetTransform().position.Y + collider.GetBoundsSize().Y / 2 + boundsSize.Y / 2 + offset;
                            yVelocity = 0;
                            if(Input.isPressed(Keys.Space))
                            {
                                yVelocity = jumpVelocity;
                            }
                            break;
                        case 1:
                            if (prevMovement.Y > 0.0f)
                            {
                                newPosition.Y = collider.GetTransform().position.Y - collider.GetBoundsSize().Y / 2 - boundsSize.Y / 2 - offset;
                                yVelocity = 0;
                            }
                            break;
                        case 2:
                            if (prevMovement.X < 0.0f)
                            {
                                newPosition.X = collider.GetTransform().position.X + collider.GetBoundsSize().X / 2 + boundsSize.X / 2 + offset;
                            }
                            break;
                        case 3:
                            if (prevMovement.X > 0.0f)
                            {
                                newPosition.X = collider.GetTransform().position.X - collider.GetBoundsSize().X / 2 - boundsSize.X / 2 - offset;
                            }
                            break;
                        case 4:
                            if (prevMovement.Z < 0.0f)
                            {
                                newPosition.Z = collider.GetTransform().position.Z + collider.GetBoundsSize().Z / 2 + boundsSize.Z / 2 + offset;
                            }
                            break;
                        case 5:
                            if (prevMovement.Z > 0.0f)
                            {
                                newPosition.Z = collider.GetTransform().position.Z - collider.GetBoundsSize().Z / 2 - boundsSize.Z / 2 - offset;
                            }
                            break;
                        default:
                            break;
                    };
                    Move(newPosition - transform.position);
                }
            }
        }

        void SpawnReplicant()
        {
            Transform replicantTransform = transform;
            replicantTransform.position = transform.position + transform.forward*boundsSize.Length();
            Replicant replicant = new Replicant(entities, lvl, replicantTransform, boundsSize);
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

                lvl.numberOfReplicants++;
                entities.Add(replicant);
            }
        }
    }
}

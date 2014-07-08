using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using Replica.Statics;

namespace Replica.Entities
{
    class Player : PlayerBase
    {
        /// <summary>
        /// Needs resolution attribute to determine rotation once the mouse is moved.
        /// </summary>
        Vector2 resolution;

        float mouseSpeed = 0.1f;
        float movementSpeed = 5;

        Vector2 prevRotationChange;
        Camera camera;

        /// <summary>
        /// All the replicants that currently exist.
        /// </summary>
        List<Replicant> replicants = new List<Replicant>();

        /// <summary>
        /// Determines which type of Replicant the Player wants to spawn.
        /// </summary>
        EntityType spawnType = EntityType.Replicant;
        float spawnDistance;
        float finalSpawnDistance;
        int prevScrollWheel;
        /// <summary>
        /// The transform of the Replicant we are about to spawn.
        /// </summary>
        Transform replicantTransform;
        /// <summary>
        /// The bounds of the Replicant we are about to spawn.
        /// </summary>
        BoundingBox replicantBounds;

        public Player(List<Entity> entities, Level lvl, Transform transform,  int windowWidth, int windowHeight)
            : base(entities, lvl, EntityType.Player, transform)
        {
            resolution = new Vector2(windowWidth, windowHeight);

            camera = new Camera(resolution);

            spawnDistance = boundsSize.Length();
            finalSpawnDistance = spawnDistance;
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime); //PlayerBase is taking over Y movement and collisions, so that Replicant can behave in the same way
            Rotate(gameTime);
            MoveXZ(gameTime);
            if (Input.isPressed(Keys.Space))
            {
                jumping = true; //PlayerBase will react to this attribute in MoveY()
            }
            UpdateReplicants();
            

            //Switching between Replicant types
            if (Input.isPressed(Keys.D1))
            {
                spawnType = EntityType.Replicant;
            }
            if (Input.isPressed(Keys.D2))
            {
                spawnType = EntityType.ImitatingReplicant;
            }

            MouseState mState = Mouse.GetState();
            UpdateSpawnDistance(mState);

            replicantTransform = transform;
            replicantTransform.position = transform.position + transform.Forward * finalSpawnDistance;
            replicantBounds = Globals.GenerateBounds(replicantTransform, boundsSize);
            if (mState.RightButton == ButtonState.Pressed && CanSpawn())
            {
                SpawnReplicant();
            }
        }

        public override void Draw(GraphicsDevice graphics, GameTime gameTime, BasicEffect effect, Camera camera)
        {
            Color color=Color.Green;
            if (!CanSpawn())
            {
                color = Color.Red;
            }
            Globals.DrawBounds(replicantBounds, color, graphics, effect);
            base.Draw(graphics, gameTime, effect, camera);
        }

        public override void OnCollision(Entity entity)
        {
        }

        public override void Move(Vector3 velocity)
        {
            base.Move(velocity);
            camera.SetTransform(transform);
        }

        public Camera GetCamera()
        {
            return camera; //What happens if camera is changed after getter was used?
        }

        //TODO 2: Create universal rotation method?
        /// <summary>
        /// Rotates the player and his camera in case the mouse was moved.
        /// </summary>
        /// <param name="gameTime"></param>
        void Rotate(GameTime gameTime)
        {
            MouseState mState = Mouse.GetState();

            Vector2 mouseMovement = resolution / 2 - new Vector2(mState.X, mState.Y);
            Mouse.SetPosition((int)resolution.X / 2, (int)resolution.Y / 2);

            prevRotationChange = mouseMovement * mouseSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
            transform.Rotation += prevRotationChange;
            camera.SetTransform(transform);
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
            //We are not supposed to be able to move up/down by pressing forwards
            Vector3 forwardWithoutY = transform.Forward;
            forwardWithoutY.Y = 0;

            Vector3 finalVelocity = forwardWithoutY * movement.X + transform.Right * movement.Y;
            Move(finalVelocity);
            prevVelocity.X = finalVelocity.X;
            prevVelocity.Z = finalVelocity.Z;
        }

        void UpdateReplicants()
        {
            //Update every ImitatingReplicant
            for (int i = 0; i < replicants.Count; i++)
            {
                if (replicants[i].GetEntityType() == EntityType.ImitatingReplicant)
                {
                    Vector3 prevVelocityWithoutY = prevVelocity;
                    prevVelocityWithoutY.Y = 0;
                    ImitatingReplicant iReplicant = (ImitatingReplicant)replicants[i];
                    iReplicant.Imitate(prevVelocityWithoutY, prevRotationChange, jumping);
                }

                //Destroy Replicant once he has run out of time
                if (replicants[i].ExistenceTime <= 0)
                {
                    replicants[i].Destroy();
                    entities.Remove(replicants[i]);
                    replicants.RemoveAt(i);
                }
            }
        }

        void UpdateSpawnDistance(MouseState mState)
        {

            int scrollWheelChange = mState.ScrollWheelValue - prevScrollWheel;
            spawnDistance += scrollWheelChange / 200.0f; //TODO 2: Remove constant
            prevScrollWheel = mState.ScrollWheelValue;

            //Minimum
            if (spawnDistance < boundsSize.Length())
            {
                spawnDistance = boundsSize.Length();
            }

            //Maximum
            List<KeyValuePair<float, Entity>> rayIntersections = CollisionSystem.RayIntersection(entities, new Ray(transform.position, transform.Forward));
            for(int i=0; i<rayIntersections.Count; i++)
            {
                if (rayIntersections[i].Value == this || !rayIntersections[i].Value.isSolid()) //We don't care if ray intersected with Player or a non-solid Block
                {
                    rayIntersections.RemoveAt(i);
                    i--;
                }
                else
                {
                    if (rayIntersections[i].Key < spawnDistance)
                    {
                        //Found closest point with solid block
                        if (rayIntersections[i].Value.isSolid())
                        {
                            finalSpawnDistance = rayIntersections[i].Key;
                            break;
                        }
                    }
                    else
                    {
                        //User-picked point is closest (since rayIntersections is sorted by distance)
                        finalSpawnDistance = spawnDistance;
                        break;
                    }
                }
            }
            //We are not looking at any solid Block
            if (rayIntersections.Count == 0)
            {
                finalSpawnDistance = spawnDistance;
            }
        }

        /// <summary>
        /// Use Level.maxReplicants to check whether we have already spawned enough Replicants.
        /// Then we use precalculated replicantBounds to check whether we are trying to spawn a Replicant inside a solid Entity.
        /// </summary>
        /// <returns></returns>
        bool CanSpawn()
        {
            if (replicants.Count < lvl.maxReplicants)
            {
                foreach (Entity entity in entities)
                {
                    if (replicantBounds.Intersects(entity.GetBounds()) && entity.isSolid())
                    {
                        return false;
                    }
                }
                return true;
            }
            return false;
        }

        void SpawnReplicant()
        {
            //TODO 1: Define how long a Replicant will exist
            Replicant replicant;
            switch (spawnType)
            {
                case EntityType.Replicant:
                    replicant = new Replicant(entities, lvl, replicantTransform, boundsSize, 5);
                    break;
                case EntityType.ImitatingReplicant:
                    replicant = new ImitatingReplicant(entities, lvl, replicantTransform, boundsSize, 5);
                    break;
                default:
                    replicant = new Replicant(entities, lvl, replicantTransform, boundsSize, 5);
                    break;
            };

            entities.Add(replicant);
            replicants.Add(replicant);
        }
    }
}

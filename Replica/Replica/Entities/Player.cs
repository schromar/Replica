using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
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

        float maxRotation = 60;
        Vector2 prevRotationChange;
        Camera cam;
        public Camera Cam { get { return cam; } }

        /// <summary>
        /// All the replicants that currently exist.
        /// </summary>
        List<Replicant> replicants = new List<Replicant>();

        /// <summary>
        /// Number of replicants that exist of the different types
        /// </summary>

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

            cam = new Camera(resolution);

            spawnDistance = boundsSize.Length();
            finalSpawnDistance = spawnDistance;
        }

        public override void Update(GameTime gameTime, AudioListener listener)
        {
            Globals.inAntiblock = false;

            base.Update(gameTime, listener); //PlayerBase is taking over Y movement and collisions, so that Replicant can behave in the same way
            Rotate(gameTime);
            MoveXZ(gameTime);
            if (Input.isPressed(Keys.Space))
            {
                if (canJump)
                    jumping = true; //PlayerBase will react to this attribute in MoveY()
            }
            UpdateReplicants();
            

            //Switching between Replicant types
            if (Input.isPressed(Keys.D1))
            {
                Globals.spawnType = EntityType.Replicant;
            }
            if (Input.isPressed(Keys.D2))
            {
                Globals.spawnType = EntityType.ImitatingReplicant;
            }

            MouseState mState = Mouse.GetState();
            UpdateSpawnDistance(mState);

            replicantTransform = t;
            replicantTransform.position = t.position + t.Forward * finalSpawnDistance;
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
            if (entity.Type == EntityType.Antiblock)
            {
                for (int i = 0; i < replicants.Count; i++)
                {
                    switch (replicants[i].Type)
                    {
                        case EntityType.Replicant:
                            Globals.normalReplicantsCount--;
                            break;
                        case EntityType.ImitatingReplicant:
                            Globals.imitatingReplicantsCount--;
                            break;
                        default:
                            break;
                    }
                    replicants[i].Destroy();
                    entities.Remove(replicants[i]);
                    replicants.RemoveAt(i);
                }
                Globals.inAntiblock = true;
            }
        }

        public override void Move(Vector3 velocity)
        {
            base.Move(velocity);
            cam.SetTransform(t);
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

            Vector2 rotationChange = mouseMovement * mouseSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
            float yDegrees = MathHelper.ToDegrees(t.Rotation.Y + rotationChange.Y);
            if(yDegrees > maxRotation || yDegrees < -maxRotation)
            {
                rotationChange.Y = 0;
            }
            prevRotationChange = rotationChange;
            t.Rotation += rotationChange;
            cam.SetTransform(t);
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
            Vector3 forwardWithoutY = t.Forward;
            forwardWithoutY.Y = 0;
            forwardWithoutY.Normalize();

            Vector3 finalVelocity = forwardWithoutY * movement.X + t.Right * movement.Y;
            Move(finalVelocity);
            prevVelocity.X = finalVelocity.X;
            prevVelocity.Z = finalVelocity.Z;
        }

        void UpdateReplicants()
        {
            //Update every ImitatingReplicant
            for (int i = 0; i < replicants.Count; i++)
            {
                if (replicants[i].Type == EntityType.ImitatingReplicant)
                {
                    Vector3 prevVelocityWithoutY = prevVelocity;
                    prevVelocityWithoutY.Y = 0;
                    ImitatingReplicant iReplicant = (ImitatingReplicant)replicants[i];
                    iReplicant.Imitate(prevVelocityWithoutY, prevRotationChange, jumping);
                }

                //Destroy Replicant once he has run out of time
                if (replicants[i].ExistenceTime <= 0)
                {
                    switch(replicants[i].Type)
                    {
                        case EntityType.Replicant:
                            Globals.normalReplicantsCount--;
                            break;
                        case EntityType.ImitatingReplicant:
                            Globals.imitatingReplicantsCount--;
                            break;
                        default:
                            break;
                    }
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
            List<KeyValuePair<float, Entity>> rayIntersections = CollisionSystem.RayIntersection(entities, new Ray(t.position, t.Forward));
            for(int i=0; i<rayIntersections.Count; i++)
            {
                if (rayIntersections[i].Value == this || !rayIntersections[i].Value.Solid) //We don't care if ray intersected with Player or a non-solid Block
                {
                    rayIntersections.RemoveAt(i);
                    i--;
                }
                else
                {
                    if (rayIntersections[i].Key < spawnDistance)
                    {
                        //Found closest point with solid block
                        if (rayIntersections[i].Value.Solid)
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
            if (Globals.inAntiblock)
                return false;

            switch (Globals.spawnType)
            {
                case EntityType.Replicant:
                    if (Globals.normalReplicantsCount >= Globals.normalReplicants)
                        return false;
                        break;
                case EntityType.ImitatingReplicant:
                        if (Globals.imitatingReplicantsCount >= Globals.imitatingReplicants)
                            return false;
                        break;
                default:
                    break;
            }
            foreach (Entity entity in entities)
            {
                if (replicantBounds.Intersects(entity.Bounds) && entity.Solid)
                {
                    return false;
                }
            }
            return true;
        }

        void SpawnReplicant()
        {
            //TODO 1: Define how long a Replicant will exist
            Replicant replicant;
            switch (Globals.spawnType)
            {
                case EntityType.Replicant:
                    replicant = new Replicant(entities, lvl, replicantTransform, boundsSize, 1000);
                    Globals.normalReplicantsCount++;
                    break;
                case EntityType.ImitatingReplicant:
                    replicant = new ImitatingReplicant(entities, lvl, replicantTransform, boundsSize, 1000);
                    Globals.imitatingReplicantsCount++;
                    break;
                default:
                    replicant = new Replicant(entities, lvl, replicantTransform, boundsSize, 1000);
                    Globals.normalReplicantsCount++;
                    break;
            };

            entities.Add(replicant);
            replicants.Add(replicant);
        }
    }
}

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

        Int32 normalTime;
        Int32 imitatingTime;

        float maxRotation = 60;
        Vector2 prevRotationChange;
        Camera cam;
        public Camera Cam { get { return cam; } }

        /// <summary>
        /// All the replicants that currently exist.
        /// </summary>
        List<Replicant> replicants = new List<Replicant>();

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
            normalTime = lvl.NormalTime;
            imitatingTime = lvl.ImitatingTime;

            resolution = new Vector2(windowWidth, windowHeight);

            cam = new Camera(resolution);

            spawnDistance = boundsSize.Length();
            finalSpawnDistance = spawnDistance;
        }

        public override void Update(GameTime gameTime, AudioListener listener)
        {
            Globals.inAntiblock = false;

            //TODO 0: performance
            base.Update(gameTime, listener); //PlayerBase is taking over Y movement and collisions, so that Replicant can behave in the same way
            //TODO 0: performance (?????)
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

            MouseState mState = Input.currentMouse;
            //TODO 0: performance (Entity count?)
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
                    replicants[i].Destroy();
                    entities.Remove(replicants[i]);
                    replicants.RemoveAt(i);
                }
                Globals.inAntiblock = true;
            }

            if (entity.Type == EntityType.Eventblock)
            {
                int index = 0;

                lvl.text = lvl.Texts.ElementAt(entity.index - 1);
                index = entity.index;

                
                Globals.newText = true;

                for (int i = 0; i < entities.Count; i++)
                {
                    if (entities.ElementAt(i).Type == EntityType.Eventblock && entities.ElementAt(i).index == index)
                    {
                        entities.RemoveAt(i);
                        i--;
                    }
                }
            }
        }

        public override void Move(Vector3 velocity)
        {
            base.Move(velocity);
            cam.SetTransform(t);
        }

        public int GetReplicantCount(EntityType type)
        {
            int res = 0;
            foreach (Replicant replicant in replicants)
            {
                if (replicant.Type == type)
                {
                    res++;
                }
            }
            return res;
        }

        //TODO 2: Create universal rotation method?
        /// <summary>
        /// Rotates the player and his camera in case the mouse was moved.
        /// </summary>
        /// <param name="gameTime"></param>
        void Rotate(GameTime gameTime)
        {
            //TODO 0: performance (GetState?)
            MouseState mState = Input.currentMouse;

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
            float? rayIntersection = CollisionSystem.RayIntersection(entities, new Ray(t.position, t.Forward), this);
            if (rayIntersection != null && (float)rayIntersection < spawnDistance)
            {
                //Found closest point with solid block
                finalSpawnDistance = (float)rayIntersection;
            }
            else
            {
                //User-picked point is closest (since rayIntersections is sorted by distance)
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
                    if (Globals.normalReplicants <= 0)
                        return false;
                        break;
                case EntityType.ImitatingReplicant:
                        if (Globals.imitatingReplicants <= 0)
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
            EntityType type;
            switch (Globals.spawnType)
            {
                case EntityType.Replicant:
                    replicant = new Replicant(entities, lvl, replicantTransform, boundsSize, normalTime);
                    type = EntityType.Replicant;
                    break;
                case EntityType.ImitatingReplicant:
                    replicant = new ImitatingReplicant(entities, lvl, replicantTransform, boundsSize, imitatingTime);
                    type = EntityType.ImitatingReplicant;
                    break;
                default:
                    replicant = new Replicant(entities, lvl, replicantTransform, boundsSize, normalTime);
                    type = EntityType.Replicant;
                    break;
            }

            switch (type)
            {
                case EntityType.Replicant:
                    if (GetReplicantCount(EntityType.Replicant) >= Globals.normalReplicants)
                    {
                        for (int i = 0; i < entities.Count; i++)
                        {
                            if (entities.ElementAt(i).Type == EntityType.Replicant)
                            {
                                entities.RemoveAt(i);
                                break;
                            }
                        }
                        for (int i = 0; i < replicants.Count; i++)
                        {
                            if (replicants.ElementAt(i).Type == EntityType.Replicant)
                            {
                                replicants.RemoveAt(i);
                                break;
                            }
                        }
                    }                    
                    break;
                case EntityType.ImitatingReplicant:
                    if (GetReplicantCount(EntityType.ImitatingReplicant) >= Globals.imitatingReplicants)
                    {
                        for (int i = 0; i < entities.Count; i++)
                        {
                            if (entities.ElementAt(i).Type == EntityType.ImitatingReplicant)
                            {
                                entities.RemoveAt(i);
                                break;
                            }
                        }
                        for (int i = 0; i < replicants.Count; i++)
                        {
                            if (replicants.ElementAt(i).Type == EntityType.ImitatingReplicant)
                            {
                                replicants.RemoveAt(i);
                                break;
                            }
                        }
                    }                    
                    break;
                default:
                    if (GetReplicantCount(EntityType.Replicant) >= Globals.normalReplicants)
                    {
                        for (int i = 0; i < entities.Count; i++)
                        {
                            if (entities.ElementAt(i).Type == EntityType.Replicant)
                            {
                                entities.RemoveAt(i);
                                break;
                            }
                        }
                        for (int i = 0; i < replicants.Count; i++)
                        {
                            if (replicants.ElementAt(i).Type == EntityType.Replicant)
                            {
                                replicants.RemoveAt(i);
                                break;
                            }
                        }
                    }                    
                    break;
            }
            

            entities.Add(replicant);
            replicants.Add(replicant);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

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

        public Player(List<Entity> entities, Level lvl, Transform transform,  int windowWidth, int windowHeight)
            : base(entities, lvl, EntityType.Player, transform)
        {
            resolution = new Vector2(windowWidth, windowHeight);

            camera = new Camera(resolution);
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

            //Update every ImitatingReplicant
            for(int i=0; i<replicants.Count; i++)
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
            if (replicants.Count < lvl.maxReplicants)
            {
                if (mState.RightButton == ButtonState.Pressed)
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

        void SpawnReplicant()
        {
            Transform replicantTransform = transform;
            replicantTransform.position = transform.position + transform.Forward*boundsSize.Length(); //Position of the Replicant will currently be slightly in front of the Player
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

            bool spawning = true;
            foreach (Entity entity in entities)
            {
                if (replicant.GetBounds().Intersects(entity.GetBounds()) && entity.isSolid())
                {
                    spawning = false;
                    replicant.Destroy();
                    break;
                }
            }

            //Not spawning inside a wall so we create the Replicant
            if (spawning)
            {
                entities.Add(replicant);
                replicants.Add(replicant);
            }
        }
    }
}

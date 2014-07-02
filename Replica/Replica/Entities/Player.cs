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
        Vector2 resolution;

        float mouseSpeed;
        float movementSpeed;

        Vector2 prevRotationChange;
        Camera camera;

        List<Replicant> replicants;

        public Player(List<Entity> entities, Level lvl, Transform transform,  int windowWidth, int windowHeight)
            : base(entities, lvl, EntityType.Player, transform)
        {
            resolution = new Vector2(windowWidth, windowHeight);

            mouseSpeed = 0.1f;
            movementSpeed = 5;

            prevRotationChange = Vector2.Zero;
            camera = new Camera(resolution);

            replicants = new List<Replicant>();
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            Rotate(gameTime);
            MoveXZ(gameTime);
            if (Input.isPressed(Keys.Space))
            {
                jumping = true;
            }

            foreach (Replicant replicant in replicants)
            {
                if (replicant.GetEntityType() == EntityType.ImitatingReplicant)
                {
                    Vector3 prevVelocityWithoutY = prevVelocity;
                    prevVelocityWithoutY.Y = 0;
                    ImitatingReplicant iReplicant = (ImitatingReplicant)replicant;
                    iReplicant.Imitate(prevVelocityWithoutY, prevRotationChange, jumping);
                }
            }

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
            replicantTransform.position = transform.position + transform.Forward*boundsSize.Length();
            Trigger spawnTest = new Trigger(entities, lvl, replicantTransform, boundsSize);
            bool spawning = true;
            foreach (Entity entity in entities)
            {
                if (spawnTest.GetBounds().Intersects(entity.GetBounds()) && entity.isSolid())
                {
                    spawning = false;
                    break;
                }
            }
            if (spawning)
            {
                //TODO 1: Get rid of lvl.numberOfReplicants
                lvl.numberOfReplicants++;
                ImitatingReplicant replicant = new ImitatingReplicant(entities, lvl, replicantTransform, boundsSize);
                entities.Add(replicant);
                replicants.Add(replicant);
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Input;
using Replica.Entities.Blocks;
using Replica.Statics;

namespace Replica.Entities
{
    /// <summary>
    /// Both Player and Replicant derive from this.
    /// Handles mostly Y movement and collision.
    /// </summary>
    class PlayerBase : Entity
    {
        protected Vector3 movementBoundsSize;
        /// <summary>
        /// Triggers for each side of the Player in the following order:
        /// Bottom, Top, Left, Right, Front, Back
        /// </summary>
        protected List<Trigger> movementBounds = new List<Trigger>();

        protected Vector3 prevVelocity;

        protected float yVelocity;
        protected float gravity = -0.25f;
        protected float jumpVelocity = 10.5f;
        protected bool jumping;

        /// <summary>
        /// Has to be a List because a single SoundEffectInstance can't be playing twice at the same time.
        /// </summary>
        List<SoundEffectInstance> jumpingSounds;
        //TODO 1: Emitter should be part of entity?
        AudioEmitter emitter = new AudioEmitter();

        public PlayerBase(List<Entity> entities, Level lvl, EntityType type, Transform transform)
            : base(entities, lvl, type, transform, new Vector3(2, 2, 2))
        {
            drawBounds = false;
            solid = true;

            movementBoundsSize = new Vector3(1.75f, 0.2f, 1.75f); //TODO 2: Use bounds variable instead, less copypasta

            Transform t = new Transform();
            //Bottom, Top (Y-, Y+)
            Vector3 nextBoundsSize = movementBoundsSize;
            t.position = new Vector3(transform.position.X, bounds.Min.Y - nextBoundsSize.Y / 2.0f, transform.position.Z);
            Trigger trigger = new Trigger(entities, lvl, t, nextBoundsSize);
            trigger.excluded.Add(this);
            movementBounds.Add(trigger);
            entities.Add(trigger);

            t.position = new Vector3(transform.position.X, bounds.Max.Y + nextBoundsSize.Y / 2.0f, transform.position.Z);
            trigger = new Trigger(entities, lvl, t, nextBoundsSize);
            trigger.excluded.Add(this);
            movementBounds.Add(trigger);
            entities.Add(trigger);

            //Left, Right (X-, X+)
            nextBoundsSize = new Vector3(movementBoundsSize.Y, movementBoundsSize.X, movementBoundsSize.Z);
            t.position = new Vector3(bounds.Min.X - nextBoundsSize.X / 2.0f, transform.position.Y, transform.position.Z);
            trigger = new Trigger(entities, lvl, t, nextBoundsSize);
            trigger.excluded.Add(this);
            movementBounds.Add(trigger);
            entities.Add(trigger);

            t.position = new Vector3(bounds.Max.X + nextBoundsSize.X / 2.0f, transform.position.Y, transform.position.Z);
            trigger = new Trigger(entities, lvl, t, nextBoundsSize);
            trigger.excluded.Add(this);
            movementBounds.Add(trigger);
            entities.Add(trigger);

            //Front, Back (Z-, Z+)
            nextBoundsSize = new Vector3(movementBoundsSize.X, movementBoundsSize.Z, movementBoundsSize.Y);
            t.position = new Vector3(transform.position.X, transform.position.Y, bounds.Min.Z - nextBoundsSize.Z / 2.0f);
            trigger = new Trigger(entities, lvl, t, nextBoundsSize);
            trigger.excluded.Add(this);
            movementBounds.Add(trigger);
            entities.Add(trigger);

            t.position = new Vector3(transform.position.X, transform.position.Y, bounds.Max.Z + nextBoundsSize.Z / 2.0f);
            trigger = new Trigger(entities, lvl, t, nextBoundsSize);
            trigger.excluded.Add(this);
            movementBounds.Add(trigger);
            entities.Add(trigger);

            jumpingSounds = new List<SoundEffectInstance>();
            emitter.Position = transform.position;
        }

        public override void Update(GameTime gameTime, AudioListener listener)
        {
            for(int i=0; i<jumpingSounds.Count; i++)
            {
                jumpingSounds[i].Apply3D(listener, emitter);
                if (jumpingSounds[i].State == SoundState.Stopped)
                {
                    jumpingSounds.RemoveAt(i);
                    i--;
                }
            }

            //Sequence is important
            MoveY(gameTime, listener);
            HandleCollisions(gameTime);
        }

        /// <summary>
        /// Adding the movementBounds to the components that Move with a Player
        /// </summary>
        /// <param name="velocity"></param>
        public override void Move(Vector3 velocity)
        {
            base.Move(velocity);
            foreach (Trigger trigger in movementBounds)
            {
                trigger.Move(velocity);
            }
            emitter.Position += velocity;
        }

        public override void Destroy()
        {
            for (int i = 0; i < movementBounds.Count; i++)
            {
                entities.Remove(movementBounds[i]);
            }
            base.Destroy();
        }

        /// <summary>
        /// If a side of the Player collides with a solid object, we push him out of said object (only so far that he touches its bounds).
        /// </summary>
        void HandleCollisions(GameTime gameTime)
        {
            for (int i = 0; i < movementBounds.Count; i++)
            {
                if (movementBounds[i].IsActivated())
                {
                    float offset = 0.05f;
                    List<Entity> colliders = movementBounds[i].GetColliders();
                    foreach (Entity collider in colliders)
                    {
                        Vector3 newPosition = transform.position;
                        switch (i)
                        {
                            case 0:
                                if (prevVelocity.Y < 0.0f)
                                {
                                    newPosition.Y = collider.GetTransform().position.Y + collider.GetBoundsSize().Y / 2 + boundsSize.Y / 2 + offset;
                                    yVelocity = 0;

                                    if (collider.GetEntityType() == EntityType.Conveyor)
                                    {
                                        Conveyor conveyor = (Conveyor)collider;
                                        Vector3 velocity = conveyor.Direction * conveyor.Speed * (float)gameTime.ElapsedGameTime.TotalSeconds;
                                        prevVelocity += velocity;
                                        newPosition += velocity;
                                    }
                                    if (collider.GetEntityType() == EntityType.JumpPad)
                                    {
                                        JumpPad jumpPad=(JumpPad)collider;
                                        yVelocity = jumpPad.Velocity;
                                    }
                                }
                                break;
                            case 1:
                                if (prevVelocity.Y > 0.0f)
                                {
                                    newPosition.Y = collider.GetTransform().position.Y - collider.GetBoundsSize().Y / 2 - boundsSize.Y / 2 - offset;
                                    yVelocity = 0;
                                }
                                break;
                            case 2:
                                if (prevVelocity.X < 0.0f)
                                {
                                    newPosition.X = collider.GetTransform().position.X + collider.GetBoundsSize().X / 2 + boundsSize.X / 2 + offset;
                                }
                                break;
                            case 3:
                                if (prevVelocity.X > 0.0f)
                                {
                                    newPosition.X = collider.GetTransform().position.X - collider.GetBoundsSize().X / 2 - boundsSize.X / 2 - offset;
                                }
                                break;
                            case 4:
                                if (prevVelocity.Z < 0.0f)
                                {
                                    newPosition.Z = collider.GetTransform().position.Z + collider.GetBoundsSize().Z / 2 + boundsSize.Z / 2 + offset;
                                }
                                break;
                            case 5:
                                if (prevVelocity.Z > 0.0f)
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
        }

        void MoveY(GameTime gameTime, AudioListener listener)
        {
            if (movementBounds[0].IsActivated() && jumping)
            {
                yVelocity = jumpVelocity;

                SoundEffectInstance jumpingSound = Assets.jumping.CreateInstance();
                jumpingSound.Volume = 0.1f;
                jumpingSound.Apply3D(listener, emitter);
                jumpingSound.Play();
                jumpingSounds.Add(jumpingSound);
            }
            jumping = false;

            yVelocity += gravity; //Pulling Player down
            Vector3 yVector = new Vector3(0, yVelocity, 0);
            yVector *= (float)gameTime.ElapsedGameTime.TotalSeconds;

            Move(yVector);
            prevVelocity.Y = yVector.Y;
        }
    }
}

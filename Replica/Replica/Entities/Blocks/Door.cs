using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Replica.Statics;

namespace Replica.Entities.Blocks
{
    /// <summary>
    /// Block which stops being solid once all switches of the same color are activated.
    /// </summary>
    class Door : Block
    {
        /// <summary>
        /// The switches that need to be activated for the door to be open.
        /// </summary>
        List<Switch> requirements;
        bool playerCollided = false;
        /// <summary>
        /// Status on the previous frame. This attribute is needed to keep the door open if the Player is still inside.
        /// </summary>
        bool prevSolid;

        SoundEffectInstance closingSound;
        SoundEffectInstance openingSound;
        AudioEmitter emitter = new AudioEmitter();

        public Door(List<Entity> entities, Level lvl, Transform transform, Vector3 boundsSize, String color)
            : base(entities, lvl, transform, boundsSize, EntityType.Door)
        {
            if (color == "green")
            {
                boundsColor = Color.Green;
            }
            if (color == "red")
            {
                boundsColor = Color.Red;
            }
            if (color == "blue")
            {
                boundsColor = Color.Blue;
            }
            drawBounds = true;
            solid = true;

            draw = false;

            requirements = lvl.getSwitches(color);
            prevSolid = solid;

            closingSound = Assets.doorClosing.CreateInstance();
            openingSound = Assets.doorOpening.CreateInstance();
            emitter.Position = transform.position;
        }

        public override void Update(GameTime gameTime, AudioListener listener)
        {
            openingSound.Apply3D(listener, emitter);
            closingSound.Apply3D(listener, emitter);

            solid = false;
            foreach(Switch requirement in requirements)
            {
                if (!requirement.IsActivated()) //If only one switch in requirements is not activated the door is closed
                {
                    solid = true;
                    break;
                }
            }

            //If the door was open before and the player is still inside, keep it open
            if (!prevSolid && solid && playerCollided)
            {
                solid = false;
            }

            //Door Open/Close events
            if (!prevSolid && solid)
            {
                closingSound.Play();
            }
            if (prevSolid && !solid)
            {
                openingSound.Play();
            }

            playerCollided = false;
            prevSolid = solid;
        }

        public override void OnCollision(Entity entity)
        {
            if (entity.GetEntityType() == EntityType.Player || entity.GetEntityType() == EntityType.Replicant || entity.GetEntityType() == EntityType.ImitatingReplicant)
            {
                playerCollided = true;
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Replica.Statics;
using Microsoft.Xna.Framework.Graphics;

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

            draw = true;

            requirements = lvl.GetSwitches(color);
            prevSolid = solid;

            closingSound = Assets.doorClosing.CreateInstance();
            openingSound = Assets.doorOpening.CreateInstance();
            emitter.Position = transform.position;
        }

        public override void Update(GameTime gameTime)
        {
            openingSound.Apply3D(Globals.listener, emitter);
            closingSound.Apply3D(Globals.listener, emitter);

            solid = false;
            foreach(Switch requirement in requirements)
            {
                if (!requirement.Activated) //If only one switch in requirements is not activated the door is closed
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

            draw = solid;
        }
     /*   public override void Draw(GraphicsDevice graphics, GameTime gameTime, BasicEffect effect, Camera camera)
        {
            if (draw)
            {
                Globals.DrawModel(Assets.doorModel, t, new Vector3(2), 1, camera);
                base.Draw(graphics, gameTime, effect, camera);
            }
        } */

        public override void OnCollision(Entity entity)
        {
            if (entity.Type == EntityType.Player || entity.Type == EntityType.Replicant || entity.Type == EntityType.ImitatingReplicant)
            {
                playerCollided = true;
            }
        }
       
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Replica.Statics;

namespace Replica.Entities
{
    /// <summary>
    /// Replicant with an Imitate method to which the Player can pass his past actions.
    /// </summary>
    class ImitatingReplicant : Replicant
    {
        public ImitatingReplicant(List<Entity> entities, Level lvl, Transform transform, Vector3 boundsSize, float existenceTime)
            : base(entities, lvl, transform, boundsSize, existenceTime, EntityType.ImitatingReplicant)
        {
        }

        //TODO 2: Make actions into its own class (once it has to be stored in a list)
        public void Imitate(Vector3 velocity, Vector2 rotation, bool jumping)
        {
            prevVelocity = velocity;
            Move(velocity);
            transform.Rotation += rotation;
            this.jumping = jumping;
        }
    }
}

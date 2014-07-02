using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Replica.Statics;

namespace Replica.Entities
{
    class ImitatingReplicant : Replicant
    {
        public ImitatingReplicant(List<Entity> entities, Level lvl, Transform transform, Vector3 boundsSize)
            : base(entities, lvl, transform, boundsSize, EntityType.ImitatingReplicant)
        {

        }

        public void Imitate(Vector3 velocity, Vector2 rotation, bool jumping)
        {
            prevVelocity = velocity;
            Move(velocity);
            transform.Rotation += rotation;
            this.jumping = jumping;
        }
    }
}

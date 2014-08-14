using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Replica.Statics;

namespace Replica.Entities
{
    /// <summary>
    /// Drawing a model and disappearing once existenceTime has passed.
    /// </summary>
    class Replicant : PlayerBase
    {
        float maxTime;
        float existenceTime;
        public float ExistenceTime { get { return existenceTime; } }

        public Replicant(List<Entity> entities, Level lvl, Transform transform, Vector3 boundsSize, float existenceTime, EntityType type=EntityType.Replicant)
            : base(entities, lvl, type, transform)
        {
            this.maxTime = existenceTime;
            this.existenceTime = existenceTime;
        }

        public override void Update(GameTime gameTime, AudioListener listener)
        {
            base.Update(gameTime, listener);
            existenceTime -= (float)gameTime.ElapsedGameTime.TotalSeconds;
        }

        public override void Draw(GraphicsDevice graphics, GameTime gameTime, BasicEffect effect, Camera camera)
        {    

            Globals.DrawModel(Assets.model, t, 0.001f, existenceTime/maxTime, camera);
            
            base.Draw(graphics, gameTime, effect, camera);
        }
    }
}

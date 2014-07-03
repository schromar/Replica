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
    /// Drawing a model and disappearing once existenceTime has passed.
    /// </summary>
    class Replicant : PlayerBase
    {
        float existenceTime;
        public float ExistenceTime { get { return existenceTime; } }

        public Replicant(List<Entity> entities, Level lvl, Transform transform, Vector3 boundsSize, float existenceTime, EntityType type=EntityType.Replicant)
            : base(entities, lvl, type, transform)
        {
            this.existenceTime = existenceTime;
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            existenceTime -= (float)gameTime.ElapsedGameTime.TotalSeconds;
        }

        public override void Draw(GraphicsDevice graphics, GameTime gameTime, BasicEffect effect, Camera camera)
        {
            Matrix rotation = Matrix.Identity;
            rotation.Forward = transform.Forward;
            rotation.Right = transform.Right;
            rotation.Up = transform.Up;

            Model model = Assets.model;
            Matrix[] transforms = new Matrix[model.Bones.Count];
            model.CopyAbsoluteBoneTransformsTo(transforms);

            foreach (ModelMesh mesh in model.Meshes)
            {
                foreach (BasicEffect mEffect in mesh.Effects)
                {
                    mEffect.EnableDefaultLighting();
                    mEffect.World = transforms[mesh.ParentBone.Index] * rotation * Matrix.CreateScale(0.001f) * Matrix.CreateTranslation(transform.position); //TODO 1: Proper scaling for Replicant once Model is added
                    mEffect.View = camera.GetView();
                    mEffect.Projection = camera.GetProjection();
                }
                mesh.Draw();
            }

            base.Draw(graphics, gameTime, effect, camera);
        }
    }
}

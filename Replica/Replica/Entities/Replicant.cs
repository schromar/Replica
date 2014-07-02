using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Replica.Statics;

namespace Replica.Entities
{
    class Replicant : PlayerBase
    {
        public Replicant(List<Entity> entities, Level lvl, Transform transform, Vector3 boundsSize, EntityType type=EntityType.Replicant)
            : base(entities, lvl, type, transform)
        {

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
                    mEffect.World = transforms[mesh.ParentBone.Index] * rotation * Matrix.CreateScale(0.001f) * Matrix.CreateTranslation(transform.position);
                    mEffect.View = camera.GetView();
                    mEffect.Projection = camera.GetProjection();
                }
                mesh.Draw();
            }

            base.Draw(graphics, gameTime, effect, camera);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Replica.Entities
{
    class Replicant : Entity
    {
        Model model;

        public Replicant(Transform transform, Model model, Vector3 boundSize, List<Entity> entities, Level lvl)
            : base(entities, lvl, EntityType.Replicant)
        {
            this.transform = transform;

            bounds.Min = transform.position - boundSize / 2.0f;
            bounds.Max = transform.position + boundSize / 2.0f;

            this.model = model;
        }

        public override void Draw(GraphicsDevice graphics, GameTime gameTime, BasicEffect effect, Camera camera)
        {
            Matrix rotation=Matrix.Identity;
            rotation.Forward = transform.forward;
            rotation.Right = transform.right;
            rotation.Up = transform.up;

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

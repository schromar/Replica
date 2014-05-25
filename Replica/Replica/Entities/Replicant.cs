using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Replica
{
    class Replicant : Entity
    {
        Model model;

        public Replicant(List<Entity> entities, Transform transform, Model model)
            : base(entities)
        {
            this.transform = transform;

            this.model = model;
        }

        public override void Draw(GameTime gameTime, BasicEffect effect, Camera camera)
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
        }
    }
}

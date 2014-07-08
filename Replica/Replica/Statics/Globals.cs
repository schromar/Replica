using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

using Replica.Entities;
using Replica.Entities.Blocks;
using Replica.Statics;

namespace Replica.Statics
{
    public class Globals
    {
        public static String currentLvl;
        public static eGamestates currentState = eGamestates.MainMenu;
        public static eGamestates prevState = eGamestates.MainMenu;
        public static bool reachedGoal = false;

        public static int windowheight = 480;
        public static int windowwidth = 800;

        public static BoundingBox GenerateBounds(Transform transform, Vector3 boundsSize)
        {
            BoundingBox bounds = new BoundingBox();
            bounds.Min = transform.position - boundsSize / 2.0f;
            bounds.Max = transform.position + boundsSize / 2.0f;
            return bounds;
        }

        public static void DrawBounds(BoundingBox bounds, Color boundsColor, GraphicsDevice graphics, BasicEffect effect)
        {
            short[] bBoxIndices = {
                    0, 1, 1, 2, 2, 3, 3, 0, // Front edges
                    4, 5, 5, 6, 6, 7, 7, 4, // Back edges
                    0, 4, 1, 5, 2, 6, 3, 7 // Side edges connecting front and back
                                      };

            Vector3[] corners = bounds.GetCorners();
            VertexPositionColor[] primitiveList = new VertexPositionColor[corners.Length];

            // Assign the 8 box vertices
            for (int i = 0; i < corners.Length; i++)
            {
                primitiveList[i] = new VertexPositionColor(corners[i], boundsColor);
            }

            // Draw the box with a LineList
            foreach (EffectPass pass in effect.CurrentTechnique.Passes)
            {
                pass.Apply();
                graphics.DrawUserIndexedPrimitives(PrimitiveType.LineList, primitiveList, 0, 8, bBoxIndices, 0, 12);
            }
        }
    
        public static void DrawModel(Model model,Transform t,float scale,Camera camera)
        {
            Matrix rotation = Matrix.Identity;
            rotation.Forward = t.Forward;
            rotation.Right = t.Right;
            rotation.Up = t.Up;

            Matrix[] transforms = new Matrix[model.Bones.Count];
            model.CopyAbsoluteBoneTransformsTo(transforms);

            foreach (ModelMesh mesh in model.Meshes)
            {
                foreach (BasicEffect mEffect in mesh.Effects)
                {
                    mEffect.EnableDefaultLighting();
                    mEffect.World = transforms[mesh.ParentBone.Index] * rotation * Matrix.CreateScale(scale) * Matrix.CreateTranslation(t.position); //TODO 1: Proper scaling for Replicant once Model is added
                    mEffect.View = camera.GetView();
                    mEffect.Projection = camera.GetProjection();
                }
                mesh.Draw();
            }
        }
    }

}

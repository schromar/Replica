using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Replica.Statics;
using Microsoft.Xna.Framework.Graphics;

namespace Replica.Entities.Blocks
{
    /// <summary>
    /// Block that causes the Player to win once he enters it.
    /// </summary>
    class Goal : Block
    {
        public Goal(Transform transform, Vector3 boundSize, List<Entity> entities, Level lvl)
            : base(entities, lvl, transform, boundSize, EntityType.Goal)
        {
            boundsColor = Color.DarkOrange;
            drawBounds = true;
            solid = false;

            draw = false;
        }

        public override void OnCollision(Entity entity)
        {
            if (entity.Type == EntityType.Player)
            {
                Globals.reachedGoal = true;
            }
        }

        public override void Draw(GraphicsDevice graphics, GameTime gameTime, BasicEffect effect, Camera camera)
        {
            string text = "Come"+ "\n" + "Here";

            //Drawing 3D Text
            Matrix rotation = Matrix.Identity;
            rotation.Right = -lvl.P.T.Right;
            rotation.Up = lvl.P.T.Up;

            Matrix worldBackup = effect.World;
            effect.World = rotation * Matrix.CreateScale(new Vector3(-0.075f)) * Matrix.CreateTranslation(t.position);
            effect.TextureEnabled = true;

            //Having to end already open spriteBatch
            Game1.spriteBatch.End();
            Game1.graphics.GraphicsDevice.DepthStencilState = DepthStencilState.Default;

            Game1.spriteBatch.Begin(0, null, SamplerState.PointWrap, DepthStencilState.DepthRead, null, effect);
            Vector2 textSize = Assets.font1.MeasureString(text);
            Game1.spriteBatch.DrawString(Assets.font1, text, -textSize / 2.0f, Color.White);

            Game1.spriteBatch.End();
            Game1.graphics.GraphicsDevice.DepthStencilState = DepthStencilState.Default;

            Game1.spriteBatch.Begin();

            effect.World = worldBackup;
            effect.TextureEnabled = false;

            base.Draw(graphics, gameTime, effect, camera);
        }
    }
}

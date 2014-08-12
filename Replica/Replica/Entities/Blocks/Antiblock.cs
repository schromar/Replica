using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Replica.Statics;

namespace Replica.Entities.Blocks
{
    class Antiblock : Block
    {

        public Antiblock(List<Entity> entities, Level lvl, Transform transform, Vector3 boundsSize)
            : base(entities, lvl, transform, boundsSize, EntityType.Antiblock)
        {
            
            drawBounds = true;
            solid = false;

            draw = false;
        }

        public override void Update(GameTime gameTime, AudioListener listener)
        {
            
        }

        public override void Draw(GraphicsDevice graphics, GameTime gameTime, BasicEffect effect, Camera camera)
        {

            string text = "Anti";

            //Drawing 3D Text
            Matrix rotation = Matrix.Identity;
            rotation.Right = -lvl.P.T.Right;
            rotation.Up = lvl.P.T.Up;

            BasicEffect textEffect = new BasicEffect(Game1.graphics.GraphicsDevice);
            textEffect.World = rotation * Matrix.CreateScale(new Vector3(-0.075f)) * Matrix.CreateTranslation(t.position);
            textEffect.View = effect.View;
            textEffect.Projection = effect.Projection;
            textEffect.TextureEnabled = true;
            textEffect.VertexColorEnabled = true;

            //Having to end already open spriteBatch
            Game1.spriteBatch.End();
            Game1.graphics.GraphicsDevice.DepthStencilState = DepthStencilState.Default;

            Game1.spriteBatch.Begin(0, null, SamplerState.PointWrap, DepthStencilState.DepthRead, null, textEffect);

            Vector2 textSize = Assets.font1.MeasureString(text);
            Game1.spriteBatch.DrawString(Assets.font1, text, -textSize / 2.0f, Color.White);

            Game1.spriteBatch.End();
            Game1.graphics.GraphicsDevice.DepthStencilState = DepthStencilState.Default;

            Game1.spriteBatch.Begin();

            base.Draw(graphics, gameTime, effect, camera);
        }

        public override void OnCollision(Entity entity)
        {

        }
    }
}
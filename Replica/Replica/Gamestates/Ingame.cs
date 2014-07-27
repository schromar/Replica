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
using Replica.Drawables;

namespace Replica.Gamestates
{
    public class Ingame : Gamestate
    {
        List<Entity> entities;

        List<Drawable> drawables;
        LevelText text;

        Level lvl;
        GraphicsDevice gDevice;
        BasicEffect defaultEffect;

        AudioListener listener=new AudioListener();

        public void Init(GraphicsDevice gDevice)
        {
            this.gDevice = gDevice;
            defaultEffect = new BasicEffect(gDevice);
            defaultEffect.VertexColorEnabled = true;

            entities = new List<Entity>();
            lvl = new Level(entities);

            drawables = new List<Drawable>();

            drawables.Add(new Skillbar());
            text = new LevelText(lvl.Text);
            drawables.Add(text);

            foreach (Drawable drawable in drawables)
            {
                drawable.Initialize();
            }
        }

        public eGamestates Update(GameTime gameTime)
        {
            Transform playerTransform=lvl.P.T;
            listener.Position = playerTransform.position;
            listener.Forward = playerTransform.Forward;
            listener.Up = playerTransform.Up;

            for (int i = 0; i < entities.Count; i++) //Certain entities will create/delete other entities in their Update, foreach does not work
            {
                entities[i].Update(gameTime, listener);
            }

            CollisionSystem.CheckCollisions(entities);


            if(Input.isClicked(Microsoft.Xna.Framework.Input.Keys.Escape))
                return eGamestates.MainMenu;

            if (Globals.reachedGoal == true)
            {
                Globals.reachedGoal = false;
                return eGamestates.Credits;
            }

            foreach (Drawable drawable in drawables)
            {
                drawable.Update(gameTime);
            }
            if (text.ExistenceTime < 0)
            {
                drawables.Remove(text);
            }

            return eGamestates.InGame;
        }

        public void Draw(GraphicsDevice graphicDevice, GameTime gameTime)
        {
            Camera camera = lvl.P.Cam;
            defaultEffect.World = Matrix.Identity;
            defaultEffect.View = camera.View;
            defaultEffect.Projection = camera.Projection;

            

            foreach (Entity entity in entities)
            {
                entity.Draw(graphicDevice, gameTime, defaultEffect, camera);
            }

            foreach (Drawable drawable in drawables)
            {
                drawable.Draw();
            }

            //TESTING 3D TEXT
            Game1.spriteBatch.End();
            Matrix rotation = Matrix.Identity;
            rotation.Right = -lvl.P.T.Right;
            rotation.Up = lvl.P.T.Up;

            BasicEffect effect = new BasicEffect(graphicDevice);
            effect.World = rotation*Matrix.CreateScale(new Vector3(-0.5f))*Matrix.CreateTranslation(new Vector3(0, 4, 0));
            effect.View = defaultEffect.View;
            effect.Projection = defaultEffect.Projection;
            effect.TextureEnabled = true;
            effect.VertexColorEnabled = true;

            Game1.spriteBatch.Begin(0, null, SamplerState.PointWrap, DepthStencilState.DepthRead, null, effect);
            Vector2 test=Assets.font1.MeasureString("bla");
            Game1.spriteBatch.DrawString(Assets.font1, "bla", -test/2.0f, Color.White);
            Game1.spriteBatch.End();

            Game1.spriteBatch.Begin();
        }
    }
}

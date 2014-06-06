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

namespace Replica
{
   
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        BasicEffect defaultEffect;

        List<Entity> entities;
        Player player;

        Model model;

        //AUDIO TESTING
        /*SoundEffectInstance soundEffectInstance;
        AudioEmitter emitter = new AudioEmitter();
        AudioListener listener = new AudioListener();*/

        Texture2D pix;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        
        protected override void Initialize()
        {
            //IsMouseVisible = true;

            defaultEffect = new BasicEffect(GraphicsDevice);
            //defaultEffect.EnableDefaultLighting();
            defaultEffect.VertexColorEnabled = true;

            base.Initialize();
        }
      
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            model = Content.Load<Model>("Models\\p1_wedge");

            //AUDIO TESTING
            /*SoundEffect soundEffect = Content.Load<SoundEffect>("Music\\Neolectrical");
            soundEffectInstance = soundEffect.CreateInstance();
            emitter.Position = Vector3.Zero;
            soundEffectInstance.Apply3D(listener, emitter);
            soundEffectInstance.Play();*/

            pix = Content.Load<Texture2D>("Textures\\pix");

            entities = new List<Entity>();
            player = new Player(entities, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height, model);
            entities.Add(player);

            //Building the Level
            Transform pos = new Transform();
            pos.position = new Vector3(10, 0, 10);
            List<Switch> requirements = new List<Switch>();
            requirements.Add(new Switch(entities, pos));
            pos.position = new Vector3(10, 0, 0);
            requirements.Add(new Switch(entities, pos));

            foreach (Switch entity in requirements)
            {
                entities.Add(entity);
            }

            pos.position = new Vector3(0, 0, 10);
            entities.Add(new Door(entities, pos, requirements));

            pos.position = new Vector3(8, 0, 10);
            entities.Add(new Block(entities, pos));
            pos.position = new Vector3(8, 0, 0);
            entities.Add(new Block(entities, pos));
            pos.position = new Vector3(6, 0, 0);
            entities.Add(new Block(entities, pos));
            pos.position = new Vector3(4, 0, 0);
            entities.Add(new Block(entities, pos));
        }

        protected override void UnloadContent()
        {
        }

        protected override void Update(GameTime gameTime)
        {

            Input.prevKeyboard = Input.currentKeyboard;
            Input.currentKeyboard = Keyboard.GetState();
            

            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Input.isClicked(Keys.Escape))
                this.Exit();

            for (int i = 0; i < entities.Count; i++) //Certain entities will create/delete other entities in their Update, foreach does not work
            {
                entities[i].Update(gameTime);
            }

            CollisionSystem.CheckCollisions(entities);

            //AUDIO TESTING
            /*listener.Position = player.GetTransform().position;
            listener.Forward = player.GetTransform().forward;
            listener.Up = player.GetTransform().up;
            soundEffectInstance.Apply3D(listener, emitter);*/

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            defaultEffect.World = Matrix.Identity;
            defaultEffect.View = player.GetCamera().GetView();
            defaultEffect.Projection = player.GetCamera().GetProjection();

            foreach (Entity entity in entities)
            {
                entity.Draw(GraphicsDevice, gameTime, defaultEffect, player.GetCamera());
            }

            spriteBatch.Begin();
            Rectangle crosshairBounds = new Rectangle(GraphicsDevice.Viewport.Width / 2-2, GraphicsDevice.Viewport.Height / 2-2, 4, 4); //TODO: Replace with variables
            spriteBatch.Draw(pix, crosshairBounds, Color.Red);
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}

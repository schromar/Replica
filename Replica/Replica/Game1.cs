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

namespace Replica
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        BasicEffect defaultEffect;
        VertexBuffer vBuffer;

        SoundEffectInstance soundEffectInstance;
        AudioEmitter emitter = new AudioEmitter();
        AudioListener listener = new AudioListener();

        List<Entity> entities;
        Player player;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            //IsMouseVisible = true;

            defaultEffect = new BasicEffect(GraphicsDevice);
            //defaultEffect.EnableDefaultLighting();
            defaultEffect.VertexColorEnabled = true;

            List<VertexPositionColor> vertexList = new List<VertexPositionColor>();
            vertexList.Add(new VertexPositionColor(new Vector3(0, 0, 0), Color.White));
            vertexList.Add(new VertexPositionColor(new Vector3(10, 0, 0), Color.White));
            vertexList.Add(new VertexPositionColor(new Vector3(10, 0, 10), Color.White));
            
            vertexList.Add(new VertexPositionColor(new Vector3(0, 0, 0), Color.Red));
            vertexList.Add(new VertexPositionColor(new Vector3(10, 0, 10), Color.Red));
            vertexList.Add(new VertexPositionColor(new Vector3(0, 0, 10), Color.Red));
            vBuffer = new VertexBuffer(GraphicsDevice, VertexPositionColor.VertexDeclaration, vertexList.Count, BufferUsage.WriteOnly);
            vBuffer.SetData<VertexPositionColor>(vertexList.ToArray());

            entities = new List<Entity>();
            player = new Player(entities, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height);
            entities.Add(player);

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            SoundEffect soundEffect = Content.Load<SoundEffect>("Neolectrical");
            soundEffectInstance = soundEffect.CreateInstance();
            emitter.Position = Vector3.Zero;
            soundEffectInstance.Apply3D(listener, emitter);
            soundEffectInstance.Play();
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            for (int i = 0; i < entities.Count; i++) //Certain entities will have to create/delete other entities in their Update, foreach does not work
            {
                entities[i].Update(gameTime);
            }

            listener.Position = player.GetTransform().position;
            listener.Forward = player.GetTransform().forward;
            listener.Up = player.GetTransform().up;
            soundEffectInstance.Apply3D(listener, emitter);

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            defaultEffect.World = Matrix.Identity;
            defaultEffect.View = player.GetCamera().GetView();
            defaultEffect.Projection = player.GetCamera().GetProjection();

            foreach (EffectPass pass in defaultEffect.CurrentTechnique.Passes)
            {
                pass.Apply();
                GraphicsDevice.SetVertexBuffer(vBuffer);
                GraphicsDevice.DrawPrimitives(PrimitiveType.TriangleList, 0, vBuffer.VertexCount / 3);
            }

            foreach (Entity entity in entities)
            {
                entity.Draw(gameTime, defaultEffect);
            }

            base.Draw(gameTime);
        }
    }
}

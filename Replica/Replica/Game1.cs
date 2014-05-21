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
        Vector3 objectPos;

        List<Entity> entities;

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
            entities.Add(new Player(GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height));

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

            foreach(Entity entity in entities)
            {
                entity.Update(gameTime);
            }

            Player player = (Player)entities[0];
            listener.Position = player.GetPosition();
            listener.Forward = player.GetForward();
            listener.Up = player.GetUp();
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

            Player player = (Player)entities[0];

            defaultEffect.World = Matrix.Identity;
            defaultEffect.View = player.GetView();
            defaultEffect.Projection = player.GetProjection();

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

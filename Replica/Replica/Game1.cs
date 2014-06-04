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

        //RENDER TESTING
        VertexBuffer vBuffer;

        //AUDIO TESTING
        SoundEffectInstance soundEffectInstance;
        AudioEmitter emitter = new AudioEmitter();
        AudioListener listener = new AudioListener();

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

            //RENDER TESTING
            List<VertexPositionColor> vertexList = new List<VertexPositionColor>();
            vertexList.Add(new VertexPositionColor(new Vector3(0, 0, 0), Color.White));
            vertexList.Add(new VertexPositionColor(new Vector3(10, 0, 0), Color.White));
            vertexList.Add(new VertexPositionColor(new Vector3(10, 0, 10), Color.White));
            
            vertexList.Add(new VertexPositionColor(new Vector3(0, 0, 0), Color.Red));
            vertexList.Add(new VertexPositionColor(new Vector3(10, 0, 10), Color.Red));
            vertexList.Add(new VertexPositionColor(new Vector3(0, 0, 10), Color.Red));
            vBuffer = new VertexBuffer(GraphicsDevice, VertexPositionColor.VertexDeclaration, vertexList.Count, BufferUsage.WriteOnly);
            vBuffer.SetData<VertexPositionColor>(vertexList.ToArray());

            base.Initialize();
        }
      
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            model = Content.Load<Model>("Models\\p1_wedge");

            //AUDIO TESTING
            SoundEffect soundEffect = Content.Load<SoundEffect>("Music\\Neolectrical");
            soundEffectInstance = soundEffect.CreateInstance();
            emitter.Position = Vector3.Zero;
            soundEffectInstance.Apply3D(listener, emitter);
            soundEffectInstance.Play();

            entities = new List<Entity>();
            player = new Player(entities, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height, model);
            entities.Add(player);

            Transform switchPos=new Transform();
            switchPos.position=new Vector3(10, 0, 10);
            entities.Add(new Switch(entities, switchPos));
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
            listener.Position = player.GetTransform().position;
            listener.Forward = player.GetTransform().forward;
            listener.Up = player.GetTransform().up;
            soundEffectInstance.Apply3D(listener, emitter);

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

            //RENDER TESTING
            foreach (EffectPass pass in defaultEffect.CurrentTechnique.Passes)
            {
                pass.Apply();
                GraphicsDevice.SetVertexBuffer(vBuffer);
                GraphicsDevice.DrawPrimitives(PrimitiveType.TriangleList, 0, vBuffer.VertexCount / 3);
            }

            base.Draw(gameTime);
        }
    }
}

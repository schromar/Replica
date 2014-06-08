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
        MouseState mouse = Mouse.GetState();
        BasicEffect defaultEffect;

        List<Entity> entities;
        Player player;

        Model model;

        //AUDIO TESTING

     /*   SoundEffectInstance soundEffectInstance;
=======
        /*SoundEffectInstance soundEffectInstance;
>>>>>>> 80191197c166f56cbcee4dc2916c7820616ac713
        AudioEmitter emitter = new AudioEmitter();
        AudioListener listener = new AudioListener();*/

       // Texture2D pix;


        enum Gamestate
        {
            MainMenu,
            Options,
            Credits,
            InGame,
            Cutszene,
            GameOver
        }
        Gamestate Currentstate = Gamestate.MainMenu;
        int screenwidth = 800;
        int screenheight = 600; 



        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        
        protected override void Initialize()
        {
          
            

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
            VertexBuffer vBuffer = new VertexBuffer(GraphicsDevice, VertexPositionColor.VertexDeclaration, vertexList.Count, BufferUsage.WriteOnly);
            vBuffer.SetData<VertexPositionColor>(vertexList.ToArray());
            Texture2D play = Content.Load<Texture2D>("Textures\\game");
            Button playbutton = new Button(play, graphics.GraphicsDevice);
            playbutton.setPosition(new Vector2(350, 300));


            base.Initialize();
        }
      
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
           
            
            spriteBatch = new SpriteBatch(GraphicsDevice);
            graphics.PreferredBackBufferWidth = screenwidth;
            graphics.PreferredBackBufferHeight = screenheight;
            graphics.ApplyChanges();
            
            model = Content.Load<Model>("Models\\p1_wedge");

            //AUDIO TESTING

          /*  SoundEffect soundEffect = Content.Load<SoundEffect>("Music\\Neolectrical");
            soundEffectInstance = soundEffect.CreateInstance();
            emitter.Position = Vector3.Zero;
            soundEffectInstance.Apply3D(listener, emitter);
            if (Currentstate == Gamestate.InGame)
            {
                soundEffectInstance.Play();
            }*/
           Texture2D pix = Content.Load<Texture2D>("Textures\\game");

            /*SoundEffect soundEffect = Content.Load<SoundEffect>("Music\\Neolectrical");
            soundEffectInstance = soundEffect.CreateInstance();
            emitter.Position = Vector3.Zero;
            soundEffectInstance.Apply3D(listener, emitter);
            soundEffectInstance.Play();*/

           // pix = Content.Load<Texture2D>("Textures\\pix");


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

            switch (Currentstate)
            {
                case Gamestate.MainMenu:
                    if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Input.isClicked(Keys.Escape))
                        this.Exit();

            Button playbutton = new Button(Content.Load<Texture2D>("Textures\\game"), graphics.GraphicsDevice);
            playbutton.setPosition( new Vector2(350, 100));
             Texture2D exit = Content.Load<Texture2D>("Textures\\exit");
                    Button exitbutton = new Button(exit, graphics.GraphicsDevice);
                    exitbutton.setPosition (new Vector2 (350, 200));
                    playbutton.Update(Mouse.GetState());
                    exitbutton.Update(Mouse.GetState());
           
                    if (playbutton.isClicked == true)
                    {
                        Currentstate = Gamestate.InGame;
                        playbutton.Update(Mouse.GetState());
                    }
                   

                    if (exitbutton.isClicked == true)
                    {
                            this.Exit();
                        
                    }

                    break;
                case Gamestate.InGame :
                    IsMouseVisible = false; 
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Input.isClicked(Keys.Escape))
                this.Exit();

            for (int i = 0; i < entities.Count; i++) //Certain entities will create/delete other entities in their Update, foreach does not work
            {
                entities[i].Update(gameTime);
            }

            CollisionSystem.CheckCollisions(entities);

            //AUDIO TESTING

           /* listener.Position = player.GetTransform().position;
            listener.Forward = player.GetTransform().forward;
            listener.Up = player.GetTransform().up;
            soundEffectInstance.Apply3D(listener, emitter);*/
                    break;
                case Gamestate.Options :
                    break;
                case Gamestate.GameOver:
                    break;
                case Gamestate.Cutszene:
                    break;
                case Gamestate.Credits:
                    break; 



            }




            


            /*listener.Position = player.GetTransform().position;
            listener.Forward = player.GetTransform().forward;
            listener.Up = player.GetTransform().up;
            soundEffectInstance.Apply3D(listener, emitter);*/


            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {   spriteBatch.Begin();
            switch (Currentstate)
            {
                case Gamestate.MainMenu:
                    Texture2D play = Content.Load<Texture2D>("Textures\\game");
            Button playbutton = new Button(play, graphics.GraphicsDevice);
            playbutton.setPosition( new Vector2(350, 100));
                    spriteBatch.Draw(Content.Load<Texture2D>("Textures\\dna"),new Rectangle(0,0,screenwidth,screenheight),Color.WhiteSmoke);

                    playbutton.Draw(spriteBatch, play, new Rectangle(350, 100, GraphicsDevice.Viewport.Width / 8, GraphicsDevice.Viewport.Height / 15), Color.White);
                      Texture2D exit = Content.Load<Texture2D>("Textures\\exit");
                    Button exitbutton = new Button(exit, graphics.GraphicsDevice);
                    exitbutton.setPosition (new Vector2 (350, 200));
                    exitbutton.Draw(spriteBatch,exit,new Rectangle (350,200,GraphicsDevice.Viewport.Width/8, GraphicsDevice.Viewport.Height/15), Color.AliceBlue
                        );
                   

                    break;
                case Gamestate.InGame:
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
                List<VertexPositionColor> vertexList = new List<VertexPositionColor>();
                vertexList.Add(new VertexPositionColor(new Vector3(0, 0, 0), Color.White));
                vertexList.Add(new VertexPositionColor(new Vector3(10, 0, 0), Color.White));
                vertexList.Add(new VertexPositionColor(new Vector3(10, 0, 10), Color.White));

                vertexList.Add(new VertexPositionColor(new Vector3(0, 0, 0), Color.Red));
                vertexList.Add(new VertexPositionColor(new Vector3(10, 0, 10), Color.Red));
                vertexList.Add(new VertexPositionColor(new Vector3(0, 0, 10), Color.Red));
                pass.Apply();
                GraphicsDevice.SetVertexBuffer(new VertexBuffer(GraphicsDevice, VertexPositionColor.VertexDeclaration, vertexList.Count, BufferUsage.WriteOnly));
                GraphicsDevice.DrawPrimitives(PrimitiveType.TriangleList, 0, new VertexBuffer(GraphicsDevice, VertexPositionColor.VertexDeclaration, vertexList.Count, BufferUsage.WriteOnly).VertexCount / 3);
             }
             Rectangle crosshairBounds = new Rectangle(GraphicsDevice.Viewport.Width / 2-2, GraphicsDevice.Viewport.Height / 2-2, 4, 4); //TODO: Replace with variables
             spriteBatch.Draw(Content.Load<Texture2D>("Textures\\game"), crosshairBounds, Color.Red);
           
            break; 
                case Gamestate.Options:
                    break;
                case Gamestate.GameOver:
                    break;
                case Gamestate.Cutszene:
                    break;
                case Gamestate.Credits:
                    break;



            }


           
 
            spriteBatch.End();

      
           

            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}

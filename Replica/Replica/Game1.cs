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
        Level lvl;

        String currentLvl;

        Model model;

        //AUDIO TESTING
        /*SoundEffectInstance soundEffectInstance;
        AudioEmitter emitter = new AudioEmitter();
        AudioListener listener = new AudioListener();*/

        Texture2D pix;
        Texture2D dna;
        Texture2D happy;

        Button playbutton;
        Button exitbutton;

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

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }


        protected override void Initialize()
        {
            IsMouseVisible = true;
            currentLvl = "01_OneButton";

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

            pix = Content.Load<Texture2D>("Textures\\pix");
            dna = Content.Load<Texture2D>("Textures\\dna");
            happy = Content.Load<Texture2D>("Textures\\happy");
            Texture2D play = Content.Load<Texture2D>("Textures\\game");
            playbutton = new Button(play, graphics.GraphicsDevice);
            playbutton.setPosition(new Vector2(350, 100));

            Texture2D exit = Content.Load<Texture2D>("Textures\\exit");
            exitbutton = new Button(exit, graphics.GraphicsDevice);
            exitbutton.setPosition(new Vector2(350, 200));

            //AUDIO TESTING
            /*SoundEffect soundEffect = Content.Load<SoundEffect>("Music\\Neolectrical");
            soundEffectInstance = soundEffect.CreateInstance();
            emitter.Position = Vector3.Zero;
            soundEffectInstance.Apply3D(listener, emitter);
            soundEffectInstance.Play();*/

            entities = new List<Entity>();
            lvl = new Level(entities, currentLvl);
            //red = new List<Entity>();
            player = new Player(GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height, model, entities, lvl);
            entities.Add(player);
        }

        protected override void UnloadContent()
        {
        }

        protected override void Update(GameTime gameTime)
        {

            Input.prevKeyboard = Input.currentKeyboard;
            Input.currentKeyboard = Keyboard.GetState();

            
                if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Input.isClicked(Keys.Escape))
                {
                    if (Currentstate == Gamestate.MainMenu)
                        this.Exit();
                    else Currentstate = Gamestate.MainMenu;
                }

            if(Input.isClicked(Keys.F1))
            {
                currentLvl = "01_OneButton";
                entities.Clear();
                lvl = new Level(entities, currentLvl);
                player = new Player(GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height, model, entities, lvl);
                entities.Add(player);
            }

            if (Input.isClicked(Keys.F2))
            {
                currentLvl = "02_TwoButtons";
                entities.Clear();
                lvl = new Level(entities, currentLvl);
                player = new Player(GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height, model, entities, lvl);
                entities.Add(player);
            }

            switch (Currentstate)
            {
                case Gamestate.MainMenu:
                    playbutton.Update(Mouse.GetState());
                    exitbutton.Update(Mouse.GetState());
                    if (playbutton.isClicked)
                    {
                        Currentstate = Gamestate.InGame;
                        IsMouseVisible = false;
                    }
                    if (exitbutton.isClicked)
                    {
                            this.Exit();
                    }
                    break;
                case Gamestate.InGame:
                    for (int i = 0; i < entities.Count; i++) //Certain entities will create/delete other entities in their Update, foreach does not work
                    {
                        entities[i].Update(gameTime);
                    }

                    


                    CollisionSystem.CheckCollisions(entities);
                    if (Door.done == true)
                    {
                        Currentstate = Gamestate.Credits;
                            
                    }
                    //AUDIO TESTING
                    /*listener.Position = player.GetTransform().position;
                    listener.Forward = player.GetTransform().forward;
                    listener.Up = player.GetTransform().up;
                    soundEffectInstance.Apply3D(listener, emitter);*/
                    break;
                default:
                    break;
            };

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin();
            switch (Currentstate)
            {
                case Gamestate.MainMenu:
                    spriteBatch.Draw(dna, new Rectangle(0, 0, dna.Width, dna.Height), Color.White);
                    playbutton.Draw(spriteBatch);
                    exitbutton.Draw(spriteBatch);
                    break;
                case Gamestate.InGame:
                    defaultEffect.World = Matrix.Identity;
                    defaultEffect.View = player.GetCamera().GetView();
                    defaultEffect.Projection = player.GetCamera().GetProjection();

                    foreach (Entity entity in entities)
                    {
                        entity.Draw(GraphicsDevice, gameTime, defaultEffect, player.GetCamera());
                    }

                    Rectangle crosshairBounds = new Rectangle(GraphicsDevice.Viewport.Width / 2 - 2, GraphicsDevice.Viewport.Height / 2 - 2, 4, 4); //TODO: Replace with variables
                    spriteBatch.Draw(pix, crosshairBounds, Color.Red);
                    break;
                case Gamestate.Credits:
                    spriteBatch.Draw(happy, new Rectangle(0, 0, dna.Width-150, dna.Height), Color.White);
                    break;
                default:
                    break;
            };
            spriteBatch.End();

            base.Draw(gameTime);
        }

        public String getLevel()
        {
            return currentLvl;
        }

        public void epicFuncttionOfHell(String woooooohoo)
        {
            Console.WriteLine("Gerd was here");
        }
    }
}

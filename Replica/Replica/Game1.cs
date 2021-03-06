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
using Replica.Gamestates;
using Replica.Statics;

namespace Replica
{

    public class Game1 : Microsoft.Xna.Framework.Game
    {
        public static GraphicsDeviceManager graphics;
        public static SpriteBatch spriteBatch;


        Gamestate gamestate = new Mainmenu();

        int fpsCounter;
        float fpsTimer;

        public Game1()
        {
            //TargetElapsedTime = TimeSpan.FromSeconds(1.0f / 120.0f);
            graphics = new GraphicsDeviceManager(this);
            graphics.PreferredBackBufferHeight = Globals.resolutionHeight;
            graphics.PreferredBackBufferWidth = Globals.resolutionWidht;

            Content.RootDirectory = "Content";

            /*graphics.SynchronizeWithVerticalRetrace = false;
            IsFixedTimeStep = false;
            graphics.ApplyChanges();*/

            SoundEffect.MasterVolume = 1;
            SoundEffect.DistanceScale = 4;
            MediaPlayer.Volume = 0.15f;
            
            //MediaPlayer.Play(Assets.song[Globals.random.Next(Assets.song.Length)]);
            //MediaPlayer.Volume = 0;
        }

        protected override void Initialize()
        {
            //MediaPlayer.Play(Assets.song[Globals.random.Next(Assets.song.Length)]);
            base.Initialize();
        }

        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            Assets.Loadcontent(Content);
            gamestate.Init(GraphicsDevice);
        }

        protected override void UnloadContent()
        {
        }

        protected override void Update(GameTime gameTime)
        {
            if (IsActive)
            {
                if (Input.isClicked(Keys.F1))
                    graphics.ToggleFullScreen();




                fpsCounter++;
                fpsTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;
                if (fpsTimer >= 1)
                {
                    Window.Title = "" + fpsCounter;
                    fpsCounter = 0;
                    fpsTimer = 0;
                }
                if (Globals.levelnamecounter - 1 < Globals.levelnames.Length)
                {
                    Globals.currentLvl = Globals.levelnames[Globals.levelnamecounter];
                }
                Input.prevKeyboard = Input.currentKeyboard;
                Input.currentKeyboard = Keyboard.GetState();

                Input.prevMouse = Input.currentMouse;
                Input.currentMouse = Mouse.GetState();

                Globals.currentState = gamestate.Update(gameTime);

                switch (Globals.currentState)
                {
                    case eGamestates.MainMenu:

                        IsMouseVisible = true;
                        break;

                    case eGamestates.InGame:

                        IsMouseVisible = false;
                        break;

                    default:

                        break;
                };

                if (Globals.currentState != Globals.prevState)
                    HandleNewGameState();

                Globals.prevState = Globals.currentState;

            }
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin();
            gamestate.Draw(gameTime);
            spriteBatch.End();
            graphics.GraphicsDevice.DepthStencilState = DepthStencilState.Default;

            base.Draw(gameTime);
        }

         private void HandleNewGameState()
        {
           

            switch (Globals.currentState)
            {
                case eGamestates.LeaveGame:
                    this.Exit();
                    break;

                case eGamestates.InGame:
                    
                    gamestate = new Ingame();
                    break;

                case eGamestates.Levelselection:
                    gamestate = new Levelselection();
                    break;

                case eGamestates.Intro:
                    gamestate = new Intro();
                    break;

                case eGamestates.Ending:
                    gamestate = new Ending();
                    break;

                case eGamestates.Options:
                    gamestate = new Options();
                    break;

                case eGamestates.MainMenu:
                    gamestate = new Mainmenu();
                    break;

                case eGamestates.GameOver:
                    gamestate = new Gameover();
                    break;

                case eGamestates.Credits:
                    gamestate = new Credits();
                    break;

                case eGamestates.Loadingscreen:
                    gamestate = new Loadinscreen();
                    break;
                default:
                    System.Console.WriteLine("unknown gamestate in - handleNewGameState() - in Game1");
                    break;
            }
            gamestate.Init(GraphicsDevice);
        }
        
        public void EpicFuncttionOfHell(String woooooohoo)
        {
            Console.WriteLine("Gerd was here");
        }
    }
}

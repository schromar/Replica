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

        //AUDIO TESTING
        /*SoundEffectInstance soundEffectInstance;
        AudioEmitter emitter = new AudioEmitter();
        AudioListener listener = new AudioListener();*/

        Gamestate gamestate = new Mainmenu();

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
            Globals.currentLvl = "06_Bridge";

            base.Initialize();
        }

        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            Assets.Loadcontent(Content);
            gamestate.Init(GraphicsDevice);

            //AUDIO TESTING
            /*SoundEffect soundEffect = Content.Load<SoundEffect>("Music\\Neolectrical");
            soundEffectInstance = soundEffect.CreateInstance();
            emitter.Position = Vector3.Zero;
            soundEffectInstance.Apply3D(listener, emitter);
            soundEffectInstance.Play();*/
        }

        protected override void UnloadContent()
        {
        }

        protected override void Update(GameTime gameTime)
        {

            Input.prevKeyboard = Input.currentKeyboard;
            Input.currentKeyboard = Keyboard.GetState();

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

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin();
           
            gamestate.Draw(GraphicsDevice, gameTime);
            
            spriteBatch.End();

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

                case eGamestates.Cutscene:
                    gamestate = new Cutscene();
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

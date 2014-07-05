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

        BasicEffect defaultEffect;
        
        List<Entity> entities = new List<Entity>();
        Level lvl;

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

            defaultEffect = new BasicEffect(GraphicsDevice);
            defaultEffect.VertexColorEnabled = true;

            base.Initialize();
        }

        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            Assets.Loadcontent(Content);
            gamestate.Init();

            //AUDIO TESTING
            /*SoundEffect soundEffect = Content.Load<SoundEffect>("Music\\Neolectrical");
            soundEffectInstance = soundEffect.CreateInstance();
            emitter.Position = Vector3.Zero;
            soundEffectInstance.Apply3D(listener, emitter);
            soundEffectInstance.Play();*/

            lvl = new Level(entities);
        }

        protected override void UnloadContent()
        {
        }

        protected override void Update(GameTime gameTime)
        {

            Input.prevKeyboard = Input.currentKeyboard;
            Input.currentKeyboard = Keyboard.GetState();

            Globals.currentState = gamestate.Update();

            switch (Globals.currentState)
            {
                case eGamestates.MainMenu:

                    IsMouseVisible = true;      
                    break;

                case eGamestates.InGame:

                    IsMouseVisible = false;

                    for (int i = 0; i < entities.Count; i++) //Certain entities will create/delete other entities in their Update, foreach does not work
                    {
                        entities[i].Update(gameTime);
                    }

                    CollisionSystem.CheckCollisions(entities);

                    if (Globals.reachedGoal == true)
                    {
                        Globals.reachedGoal = false;
                        Globals.currentState = eGamestates.Credits;
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

            if (Globals.currentState != Globals.prevState)
                HandleNewGameState();

            Globals.prevState = Globals.currentState;

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin();
           
            gamestate.Draw();
            
            //TODO 2: Use Ingame's Draw method instead (what is the use of the eGamestates enum?)
            switch (Globals.currentState)
            {                
                case eGamestates.InGame:
                    Camera camera = lvl.GetPlayer().GetCamera();
                    defaultEffect.World = Matrix.Identity;
                    defaultEffect.View = camera.GetView();
                    defaultEffect.Projection = camera.GetProjection();

                    foreach (Entity entity in entities)
                    {
                        entity.Draw(GraphicsDevice, gameTime, defaultEffect, camera);
                    }

                    Rectangle crosshairBounds = new Rectangle(GraphicsDevice.Viewport.Width / 2 - 2, GraphicsDevice.Viewport.Height / 2 - 2, 4, 4); //TODO: Replace with variables
                    spriteBatch.Draw(Assets.pix, crosshairBounds, Color.Red);
                    break;
                
                default:
                    break;
            };
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
                    entities.Clear();
                    lvl = new Level(entities);

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
            gamestate.Init();
        }
        
        public void EpicFuncttionOfHell(String woooooohoo)
        {
            Console.WriteLine("Gerd was here");
        }
    }
}

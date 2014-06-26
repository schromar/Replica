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

namespace Replica
{

    public class Game1 : Microsoft.Xna.Framework.Game
    {
        public static GraphicsDeviceManager graphics;
        public static SpriteBatch spriteBatch;

        BasicEffect defaultEffect;
        
        List<Entity> entities;
        
        Player player;
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
            Globals.currentLvl = "01_OneButton";

            defaultEffect = new BasicEffect(GraphicsDevice);
            
            defaultEffect.VertexColorEnabled = true;

            base.Initialize();
        }

        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            Assets.loadcontent(Content);
            gamestate.init();

            //AUDIO TESTING
            /*SoundEffect soundEffect = Content.Load<SoundEffect>("Music\\Neolectrical");
            soundEffectInstance = soundEffect.CreateInstance();
            emitter.Position = Vector3.Zero;
            soundEffectInstance.Apply3D(listener, emitter);
            soundEffectInstance.Play();*/

            entities = new List<Entity>();
            lvl = new Level(entities);
            //player = new Player(entities, lvl, Globals.windowwidth, Globals.windowheight, Assets.model);
            Console.WriteLine(GraphicsDevice.Viewport.Width);
            Console.WriteLine(GraphicsDevice.Viewport.Height);
            //entities.Add(player);
        }

        protected override void UnloadContent()
        {
        }

        protected override void Update(GameTime gameTime)
        {

            Input.prevKeyboard = Input.currentKeyboard;
            Input.currentKeyboard = Keyboard.GetState();

            Globals.currentState = gamestate.update();

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
                handleNewGameState();

            Globals.prevState = Globals.currentState;

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin();
           
            gamestate.draw();
            
            switch (Globals.currentState)
            {                
                case eGamestates.InGame:
                    defaultEffect.World = Matrix.Identity;
                    defaultEffect.View = lvl.GetPlayer().GetCamera().GetView();

                    defaultEffect.Projection = lvl.GetPlayer().GetCamera().GetProjection();

                    foreach (Entity entity in entities)
                    {
                        entity.Draw(GraphicsDevice, gameTime, defaultEffect, lvl.GetPlayer().GetCamera());
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

        private void handleNewGameState()
        {
            switch (Globals.currentState)
            {

                case eGamestates.LeaveGame:
                    this.Exit();
                    break;

                case eGamestates.InGame:

                    entities.Clear();
                    lvl = new Level(entities);
                    //player = new Player(entities, lvl,Globals.windowwidth, Globals.windowheight, Assets.model); 
                    //entities.Add(player);

                    gamestate = new Ingame();

                    gamestate.init();
                    break;

                case eGamestates.Levelselection:

                    gamestate = new Levelselection();

                    gamestate.init();
                    break;

                case eGamestates.Cutscene:

                    gamestate = new Cutscene();

                    gamestate.init();
                    break;

                case eGamestates.Options:

                    gamestate = new Options();

                    gamestate.init();
                    break;

                case eGamestates.MainMenu:

                    gamestate = new Mainmenu();

                    gamestate.init();

                    break;

                case eGamestates.GameOver:

                    gamestate = new Gameover();

                    gamestate.init();

                    break;

                case eGamestates.Credits:

                    gamestate = new Credits();

                    gamestate.init();

                    break;

                default:
                    System.Console.WriteLine("unknown gamestate in - handleNewGameState() - in Game1");
                    break;
            }
        }
        
        public void epicFuncttionOfHell(String woooooohoo)
        {
            Console.WriteLine("Gerd was here");
        }

        public GraphicsDevice GetGraphicDevice()
        {
            return this.GraphicsDevice;
        }
    }
}

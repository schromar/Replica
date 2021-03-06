﻿using System;
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
using Replica.Drawables;

namespace Replica.Gamestates
{
    public class Ingame : Gamestate
    {
        List<Entity> entities;

        List<Drawable> drawables;
        OnScreenText text;

        Level lvl;
        GraphicsDevice gDevice;
        BasicEffect defaultEffect;
        bool music = false;

        public void Init(GraphicsDevice gDevice)
        {
            this.gDevice = gDevice;
            defaultEffect = new BasicEffect(gDevice);
            defaultEffect.VertexColorEnabled = true;

            entities = new List<Entity>();
            lvl = new Level(entities, gDevice);

            drawables = new List<Drawable>();

            drawables.Add(new Skillbar(lvl.P));

            text = new OnScreenText(lvl.Text);
            drawables.Add(text);

            foreach (Drawable drawable in drawables)
            {
                drawable.Initialize();
            }

            

            
        }

        public eGamestates Update(GameTime gameTime)
        {
            Transform playerTransform=lvl.P.T;
            Globals.listener.Position = playerTransform.position;
            Globals.listener.Forward = playerTransform.Forward;
            Globals.listener.Up = playerTransform.Up;

            for (int i = 0; i < entities.Count; i++) //Certain entities will create/delete other entities in their Update, foreach does not work
            {
                entities[i].Update(gameTime);
            }

            CollisionSystem.CheckCollisions(entities);

            if(Input.isClicked(Microsoft.Xna.Framework.Input.Keys.Escape))
                return eGamestates.MainMenu;

            if (Globals.newText)
            {
                OnScreenText newText = new OnScreenText(lvl.text);
                newText.Initialize();
                drawables.Add(newText);
                Globals.newText = false;
            }


            if (Globals.reachedGoal == true)
            {
                entities.Clear();
                if (Globals.highesstreachedlvl <= Globals.levelnamecounter)
                {
                    Globals.highesstreachedlvl++; 
                }


                if (Globals.levelnamecounter == Globals.levelnames.Length - 1)
                {
                    if (!Globals.reachedEnd)
                    {
                        Globals.reachedEnd = true;
                        return eGamestates.Ending;
                    }
                }
                Globals.levelnamecounter++; 
                Globals.reachedGoal = false;

                return eGamestates.Loadingscreen;
               
            }

            for (int i = 0; i < drawables.Count; i++)
            {
                drawables[i].Update(gameTime);
                if (drawables[i].ExistenceTime < 0)
                {
                    drawables.RemoveAt(i);
                }

            }
           

            return eGamestates.InGame;
        }

        public void Draw(GameTime gameTime)
        {
            Camera camera = lvl.P.Cam;
            defaultEffect.World = Matrix.Identity;
            defaultEffect.View = camera.View;
            defaultEffect.Projection = camera.Projection;

            List<Entity> switches=new List<Entity>();
            foreach (Entity entity in entities)
            {
                if (entity.Type != EntityType.Switch && entity.Type != EntityType.Goal)
                {
                    entity.Draw(Game1.graphics.GraphicsDevice, gameTime, defaultEffect, camera);
                }
                else
                {
                    switches.Add(entity);
                }
            }

            //TODO 1: Find another way to draw 3D Text without drawing behind other objects
            foreach (Entity s in switches)
            {
                s.Draw(Game1.graphics.GraphicsDevice, gameTime, defaultEffect, camera);
            }

            foreach (Drawable drawable in drawables)
            {
                drawable.Draw();
            }
        }
    }
}

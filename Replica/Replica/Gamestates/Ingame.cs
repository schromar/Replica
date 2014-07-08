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

namespace Replica.Gamestates
{
    public class Ingame : Gamestate
    {
        List<Entity> entities;

        Player player;
        Level lvl;
        GraphicsDevice gDevice;
        BasicEffect defaultEffect;

        public void Init(GraphicsDevice gDevice)
        {
            this.gDevice = gDevice;
            defaultEffect = new BasicEffect(gDevice);
            defaultEffect.VertexColorEnabled = true;

            entities = new List<Entity>();

            lvl = new Level(entities);
        }

        public eGamestates Update(GameTime gameTime)
        {
            for (int i = 0; i < entities.Count; i++) //Certain entities will create/delete other entities in their Update, foreach does not work
            {
                entities[i].Update(gameTime);
            }


            if(Input.isClicked(Microsoft.Xna.Framework.Input.Keys.Escape))
                return eGamestates.MainMenu;

            if (Globals.reachedGoal == true)
            {
                Globals.reachedGoal = false;
                return eGamestates.Credits;
            }

            return eGamestates.InGame;
        }

        public void Draw()
        {
        }
    }
}

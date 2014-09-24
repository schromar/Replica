using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;


namespace Replica
{
    public enum eGamestates
    {
        LeaveGame,
        MainMenu,
        Levelselection,
        Options,
        Credits,
        InGame,
        Cutscene,
        GameOver,
        Loadingscreen,
        Intro,
        Ending
    }

    public interface Gamestate
    {
        void Init(GraphicsDevice gDevice);

        

        eGamestates Update(GameTime gameTime);

        void Draw(GameTime gameTime);       
    }
}

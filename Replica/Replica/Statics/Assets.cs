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

namespace Replica.Statics
{
    public class Assets
    {
        public static Texture2D mainmenu;
        public static Texture2D happy;
        public static Texture2D play;
        public static Texture2D exit;
        public static Texture2D levelselection;
        public static Texture2D pix;
        public static Texture2D loading;
        public static Texture2D textbox;

        public static Texture2D lvl_clear;
        public static Texture2D lvl00;
        public static Texture2D lvl01;
        public static Texture2D lvl02;

        public static Texture2D simpleReplicantButton;
        public static Texture2D simpleReplicantButton2;
        public static Texture2D imitatingReplicantButton;
        public static Texture2D clearSkill;
        public static Texture2D levelSelection;

        public static Model model;
        public static Model wallModel;
        public static Model conveyorModel;
        public static Model redSwitchModel;
        public static Model jumppadModel;
        public static Model greenSwitchModel;
        public static Model blueSwitchModel;
        public static Model normalReplicantModel;
        public static Model imitatingReplicantModel;
        public static Model doorModel;

        public static SoundEffect doorClosing;
        public static SoundEffect doorOpening;
        public static SoundEffect jumping;

        public static SpriteFont font1;

        public static Song[] song;

        public static void Loadcontent(ContentManager Manager)
        {
            play                        = Manager.Load<Texture2D>("Textures\\game");
            exit                        = Manager.Load<Texture2D>("Textures\\exit");
            levelselection              = Manager.Load<Texture2D>("Textures\\levelselection");
            mainmenu                        = Manager.Load<Texture2D>("Textures\\MainMenu");
            happy                       = Manager.Load<Texture2D>("Textures\\credits");
            pix                         = Manager.Load<Texture2D>("Textures\\pix");
            loading                     = Manager.Load<Texture2D>("Textures\\loading");
            textbox                     = Manager.Load<Texture2D>("Textures\\textBoxtexture");
            levelSelection              = Manager.Load<Texture2D>("Textures\\Levelselectionscreen");

            lvl_clear                   = Manager.Load<Texture2D>("Textures\\lvl_clear");
            lvl00                       = Manager.Load<Texture2D>("Textures\\lvl00");
            lvl01                       = Manager.Load<Texture2D>("Textures\\lvl01");
            lvl02                       = Manager.Load<Texture2D>("Textures\\lvl02");

            simpleReplicantButton       = Manager.Load<Texture2D>("Textures\\simpleReplicant");
            simpleReplicantButton2      = Manager.Load<Texture2D>("Textures\\simpleReplicantV2");
            imitatingReplicantButton    = Manager.Load<Texture2D>("Textures\\imitate");
            clearSkill                  = Manager.Load<Texture2D>("Textures\\clearSkill");
            

          //  model                       = Manager.Load<Model>("Models\\p1_wedge");
            wallModel                   = Manager.Load<Model>("Models\\wall");
           // conveyorModel               = Manager.Load<Model>("Models\\Conveyor");
            redSwitchModel              = Manager.Load<Model>("Models\\redSwitch");
            greenSwitchModel            = Manager.Load<Model>("Models\\greenSwitch");
            blueSwitchModel             = Manager.Load<Model>("Models\\blueSwitch");
            jumppadModel                = Manager.Load<Model>("Models\\Jumppad");
            normalReplicantModel        = Manager.Load<Model>("Models\\ReplicantNormal");
            imitatingReplicantModel     = Manager.Load<Model>("Models\\ReplicantImitating");
            doorModel                   = Manager.Load<Model>("Models\\door");

            doorClosing                 = Manager.Load<SoundEffect>("Sounds\\doorClosing");
            doorOpening                 = Manager.Load<SoundEffect>("Sounds\\doorOpening");
            jumping                     = Manager.Load<SoundEffect>("Sounds\\jumping");


            font1 = Manager.Load<SpriteFont>("Fonts\\SpriteFont1");

            song = new Song[]
            { 
                Manager.Load<Song>("Music\\Chris Zabriskie - I Am a Man Who Will Fight for Your Honor"),
                Manager.Load<Song>("Music\\DOCTOR VOX - Radar"),
                Manager.Load<Song>("Music\\Goto80 and the Uwe Schenk Band - Ponky Fonky Ferret"),
                Manager.Load<Song>("Music\\Mooma - Tachyon Lullaby"),
                Manager.Load<Song>("Music\\Rainfallen - Phyzo's Right"),
                Manager.Load<Song>("Music\\Rolemusic - The Pirate And The Dancer")
            };
        }
    }
}

    

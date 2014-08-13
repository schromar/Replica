using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TiledSharp;

using Replica.Entities;
using Replica.Entities.Blocks;
using Replica.Statics;
using Microsoft.Xna.Framework.Graphics;

namespace Replica
{
    public enum eLevels
    {
        Level00,
        Level01,
        Level02,
        Level03,
        Level04,
        Level05,
        Level06
    }

    class Level
    {
        /// <summary>
        /// Threedimensional array of Entity tiles. Not used yet.
        /// </summary>
        private Entity[, ,] lvl;
        private List<Switch> redSwitches;
        private List<Switch> greenSwitches;
        private List<Switch> blueSwitches;
        private Player p;
        public  Player P { get { return p; } }

        string text;
        string currentText;

        public string Text { get { return text; } }

        public Level(List<Entity> entities, GraphicsDevice gDevice)
        {
            //Load map with TiledSharp
            TmxMap map = new TmxMap("Levels/" + Globals.currentLvl + ".tmx"); //TODO 2: Check if file even exists

            
            Globals.normalReplicants = Convert.ToInt32(map.Properties["ReplicantsNormal"]);
            Globals.imitatingReplicants = Convert.ToInt32(map.Properties["ReplicantsImitating"]);


            text = map.Properties["Text"];

            Vector3 size = new Vector3(map.Width, map.Layers.Count, map.Height);

            lvl = new Entity[map.Width, map.Layers.Count, map.Height];

            redSwitches = new List<Switch>();
            greenSwitches = new List<Switch>();
            blueSwitches = new List<Switch>();

            Vector3 blockSize = new Vector3(4, 4, 4);

            //Iterate through map and create Entity for each tile
            for (int y = 0; y < map.Layers.Count; y++)
            {
                for (int index = 0; index < map.Layers[y].Tiles.Count; index++)
                {
                    TmxLayerTile currentTile = map.Layers[y].Tiles[index];
                    
                    Vector3 position=new Vector3(currentTile.X, y, currentTile.Y); //The Y coordinate in Tiled is our Z coordinate
                    Entity currentEntity = null;
                    Transform t = new Transform();
                    t.position = position * blockSize;

                    //Depending on the id of our Tile we create a different type of Entity
                    switch (currentTile.Gid)
                    {
                        case 0:
                            break;
                        case 1:
                            currentEntity = new Block(entities, this, t, blockSize);
                            break;
                        case 2:
                            p = new Player(entities, this, t, gDevice.Viewport.Width, gDevice.Viewport.Height);
                            currentEntity = p;
                            break;
                        case 3:
                            currentEntity = new Goal(t, blockSize, entities, this);    
                            break;
                        case 4:
                            currentEntity = new Switch(entities, this, t, blockSize, "red");
                            redSwitches.Add((Switch)currentEntity);
                            break;
                        case 5:
                            currentEntity = new Switch(entities, this, t, blockSize, "green");
                            greenSwitches.Add((Switch)currentEntity);
                            break;
                        case 6:
                            currentEntity = new Switch(entities, this, t, blockSize, "blue");
                            blueSwitches.Add((Switch)currentEntity);
                            break;
                        case 7:
                            currentEntity = new Door(entities, this, t, blockSize, "red");
                            break;
                        case 8:
                            currentEntity = new Door(entities, this, t, blockSize, "green");
                            break;
                        case 9:
                            currentEntity = new Door(entities, this, t, blockSize, "blue");
                            break;
                        case 10:
                            if (currentTile.DiagonalFlip)
                            {
                                if (currentTile.VerticalFlip)
                                {
                                    currentEntity = new Conveyor(entities, this, t, blockSize, new Vector3(1, 0, 0));
                                }
                                else
                                {
                                    currentEntity = new Conveyor(entities, this, t, blockSize, new Vector3(-1, 0, 0));
                                }
                            }
                            else
                            {
                                if (currentTile.HorizontalFlip)
                                {
                                    currentEntity = new Conveyor(entities, this, t, blockSize, new Vector3(0, 0, -1));
                                }
                                else
                                {
                                    currentEntity = new Conveyor(entities, this, t, blockSize, new Vector3(0, 0, 1));
                                }
                            }
                            break;
                        case 11:
                            currentEntity = new JumpPad(entities, this, t, blockSize);
                            break;
                        case 12:
                            currentEntity = new Glass(entities, this, t, blockSize);
                            break;
                        case 13:
                            currentEntity = new Antiblock(entities, this, t, blockSize);
                            break;
                        case 21:
                            currentEntity = new Eventblock(entities, this, t, blockSize, 1);
                            break;
                        case 22:
                            currentEntity = new Eventblock(entities, this, t, blockSize, 2);
                            break;
                        case 23:
                            currentEntity = new Eventblock(entities, this, t, blockSize, 3);
                            break;
                        default:
                            break;
                    };
                    lvl[(int)position.X, (int)position.Y, (int)position.Z] = currentEntity;
                    if(currentEntity != null)
                        entities.Add(currentEntity);
                }
            }
        }

        public List<Switch> GetSwitches(String color)
        {
            if (color.Equals("red"))
                return redSwitches;
            else if (color.Equals("green"))
                return greenSwitches;
            else if (color.Equals("blue"))
                return blueSwitches;
            else throw new NotSupportedException();
        }
    }
}

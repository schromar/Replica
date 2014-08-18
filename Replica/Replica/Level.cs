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

        public string text = "";
        public string t = "";

        private List<String> texts;

        public Int32 normalTime;
        public Int32 imitatingTime;

        public Int32 NormalTime { get { return normalTime; } }
        public Int32 ImitatingTime { get { return imitatingTime; } }

        public string Text { get { return text; } }
        public List<String> Texts { get { return texts; } }

        Vector3 blockSize = new Vector3(4, 4, 4);

        public Level(List<Entity> entities, GraphicsDevice gDevice)
        {
            //Load map with TiledSharp
            TmxMap map = new TmxMap("Levels/" + Globals.currentLvl + ".tmx"); //TODO 2: Check if file even exists

            
            Globals.normalReplicants = Convert.ToInt32(map.Properties["ReplicantsNormal"]);
            Globals.imitatingReplicants = Convert.ToInt32(map.Properties["ReplicantsImitating"]);

            //check, if there are timelimits for the replicants
            if (map.Properties.ContainsKey("ReplicantsNormalTime"))
                normalTime = Convert.ToInt32(map.Properties["ReplicantsNormalTime"]);
            else normalTime = 9000;

            if (map.Properties.ContainsKey("ReplicantsImitatingTime"))
                imitatingTime = Convert.ToInt32(map.Properties["ReplicantsImitatingTime"]);
            else imitatingTime = 9000;

            //if the map contains text, it is loaded here
            string curName = "";
            texts = new List<string>();

            if (map.Properties.ContainsKey("Text1"))
            {
                for (int i = 1; i <= 99; i++)
                {
                    curName = "Text" + i;
                    if (map.Properties.ContainsKey(curName))
                        texts.Add(map.Properties[curName]);
                    else
                        break;               
                }
            
                    for (int i = 1; i <=99; i++)
                    {
                        text = texts.ElementAt(i - 1);

                        for (int j = 1; j <= 99; j++)
                        {
                            if (!map.Properties.ContainsKey("Text" + i + "_" + j))
                                break;
                            t = map.Properties["Text" + i + "_" + j];

                            text += "\n";
                            text += t;
                        }
                        texts.RemoveAt(i - 1);
                        texts.Insert(i - 1, text);

                        if (i == texts.Count)
                            break;
                    }               
            }
            text = texts.ElementAt(0);

            Vector3 size = new Vector3(map.Width, map.Layers.Count, map.Height);

            lvl = new Entity[map.Width, map.Layers.Count, map.Height];

            redSwitches = new List<Switch>();
            greenSwitches = new List<Switch>();
            blueSwitches = new List<Switch>();

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
                        case 24:
                            currentEntity = new Eventblock(entities, this, t, blockSize, 4);
                            break;
                        case 25:
                            currentEntity = new Eventblock(entities, this, t, blockSize, 5);
                            break;
                        default:
                            break;
                    };
                    lvl[(int)position.X, (int)position.Y, (int)position.Z] = currentEntity;
                    if(currentEntity != null)
                        entities.Add(currentEntity);
                }
            }

            Merge(entities);
        }

        void Merge(List<Entity> entities)
        {
            bool[, ,] merged=new bool[lvl.GetLength(0), lvl.GetLength(1), lvl.GetLength(2)];
            for (int x = 0; x < lvl.GetLength(0); x++)
            {
                for (int y = 0; y < lvl.GetLength(1); y++)
                {
                    for (int z = 0; z < lvl.GetLength(2); z++)
                    {
                        if (lvl[x, y, z] != null && !merged[x, y, z])
                        {
                            EntityType type = lvl[x, y, z].Type;
                            if (type == EntityType.Block || type == EntityType.Glass)
                            {
                                merged[x, y, z] = true;
                                Vector3 size = new Vector3(1, 1, 1);
                                //optimization: check previous extension axis, axis focus?

                                bool extendX = true;
                                bool extendY = true;
                                bool extendZ = true;
                                while (extendX || extendY || extendZ)
                                {
                                    //Check each axis to extend into
                                    int subX = x + (int)size.X;
                                    int subY = 0;
                                    int subZ = 0;
                                    if (subX < lvl.GetLength(0))
                                    {
                                        extendX = true;
                                        for (subY = y; subY < y + size.Y; subY++)
                                        {
                                            for (subZ = z; subZ < z + size.Z; subZ++)
                                            {
                                                if (lvl[subX, subY, subZ] == null || lvl[subX, subY, subZ].Type != type || merged[subX, subY, subZ])
                                                {
                                                    extendX = false;
                                                    break;
                                                }
                                            }
                                            if (!extendX)
                                            {
                                                break;
                                            }
                                        }

                                        if (extendX)
                                        {
                                            size.X++;
                                            for (subY = y; subY < y + size.Y; subY++)
                                            {
                                                for (subZ = z; subZ < z + size.Z; subZ++)
                                                {
                                                    merged[subX, subY, subZ] = true;
                                                }
                                            }
                                        }
                                    }
                                    else
                                    {
                                        extendX = false;
                                    }


                                    subY = y + (int)size.Y;
                                    if (subY < lvl.GetLength(1))
                                    {
                                        extendY = true;
                                        for (subX = x; subX < x + size.X; subX++)
                                        {
                                            for (subZ = z; subZ < z + size.Z; subZ++)
                                            {
                                                if (lvl[subX, subY, subZ] == null || lvl[subX, subY, subZ].Type != type || merged[subX, subY, subZ])
                                                {
                                                    extendY = false;
                                                    break;
                                                }
                                            }
                                            if (!extendY)
                                            {
                                                break;
                                            }
                                        }

                                        if (extendY)
                                        {
                                            size.Y++;
                                            for (subX = x; subX < x + size.X; subX++)
                                            {
                                                for (subZ = z; subZ < z + size.Z; subZ++)
                                                {
                                                    merged[subX, subY, subZ] = true;
                                                }
                                            }
                                        }
                                    }
                                    else
                                    {
                                        extendY = false;
                                    }


                                    subZ = z + (int)size.Z;
                                    if (subZ < lvl.GetLength(2))
                                    {
                                        extendZ = true;
                                        for (subX = x; subX < x + size.X; subX++)
                                        {
                                            for (subY = y; subY < y + size.Y; subY++)
                                            {
                                                if (lvl[subX, subY, subZ] == null || lvl[subX, subY, subZ].Type != type || merged[subX, subY, subZ])
                                                {
                                                    extendZ = false;
                                                    break;
                                                }
                                            }
                                            if (!extendZ)
                                            {
                                                break;
                                            }
                                        }

                                        if (extendZ)
                                        {
                                            size.Z++;
                                            for (subX = x; subX < x + size.X; subX++)
                                            {
                                                for (subY = y; subY < y + size.Y; subY++)
                                                {
                                                    merged[subX, subY, subZ] = true;
                                                }
                                            }
                                        }
                                    }
                                    else
                                    {
                                        extendZ = false;
                                    }
                                }

                                //use size and position to create new block
                                Transform t = new Transform();
                                t.position = (new Vector3(x, y, z) + size / 2.0f) * blockSize - blockSize / 2;
                                Entity entity = null;
                                if (type == EntityType.Block)
                                {
                                    entity = new Block(entities, this, t, size * blockSize);
                                }
                                else if (type == EntityType.Glass)
                                {
                                    entity = new Glass(entities, this, t, size * blockSize);
                                }

                                entities.Add(entity);
                                for (int newX = x; newX < x + size.X; newX++)
                                {
                                    for (int newY = y; newY < y + size.Y; newY++)
                                    {
                                        for (int newZ = z; newZ < z + size.Z; newZ++)
                                        {
                                            entities.Remove(lvl[newX, newY, newZ]);
                                            lvl[newX, newY, newZ] = entity;
                                        }
                                    }
                                }
                            }
                        }
                    }
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

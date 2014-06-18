using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TiledSharp;

using Replica.Entities;
using Replica.Entities.Blocks;

namespace Replica
{
    class Level
    {
        
        Entity[, ,] lvl;
        List<Switch> redSwitches;
        List<Switch> greenSwitches;
        List<Switch> blueSwitches;
        

        public Level(List<Entity> entities, string mapName)
        {
            TmxMap map = new TmxMap("Levels/" + mapName + ".tmx"); //TODO: Check if file even exists
            
            Vector3 size = new Vector3(map.Width, map.Layers.Count, map.Height);

            lvl = new Entity[map.Width, map.Layers.Count, map.Height];

            redSwitches = new List<Switch>();
            greenSwitches = new List<Switch>();
            blueSwitches = new List<Switch>();

            for (int y = 0; y < map.Layers.Count; y++)
            {
                for (int index = 0; index < map.Layers[y].Tiles.Count; index++)
                {
                    TmxLayerTile currentTile = map.Layers[y].Tiles[index];
                    
                    Vector3 position=new Vector3(currentTile.X, y, currentTile.Y);
                    Entity currentEntity;
                    Transform t = new Transform();
                    t.position = new Vector3(position.X * 2, position.Y *2, position.Z * 2); //TODO: Assuming block size

                    switch (currentTile.Gid)
                    {

                        case 0:
                            currentEntity = null;
                            break;
                        case 1:
                            currentEntity = new Block(t,entities,this);
                            break;
                        case 2:
                            currentEntity = null;
                            break;
                        case 3:
                            currentEntity = null;    
                            break;
                        case 4:
                            currentEntity = new Switch(t, "red", entities, this);
                            redSwitches.Add((Switch)currentEntity);
                            break;
                        case 5:
                            currentEntity = new Switch(t, "green", entities, this);
                            greenSwitches.Add((Switch)currentEntity);
                            break;
                        case 6:
                            currentEntity = new Switch(t, "blue", entities, this);
                            blueSwitches.Add((Switch)currentEntity);
                            break;
                        case 7:
                            currentEntity = new Door(t, "red", entities, this);
                            break;
                        case 8:
                            currentEntity = new Door(t, "green", entities, this);
                            break;
                        case 9:
                            currentEntity = new Door(t, "blue", entities, this);
                            break;
                        //TODO: remaining EntityTypes
                        default:
                            currentEntity = null;
                            break;


                    };
                    lvl[(int)position.X, (int)position.Y, (int)position.Z] = currentEntity;
                    if(currentEntity != null)
                        entities.Add(currentEntity);
                }
            }
        }

          

        public List<Switch> getSwitches(String color)
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

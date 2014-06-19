using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TiledSharp;

using Replica.Entities;
using Replica.Entities.Blocks;
using Replica.Statics;

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
        
        Entity[, ,] lvl;
        List<Switch> redSwitches;
        List<Switch> greenSwitches;
        List<Switch> blueSwitches;

        Level[] levels = new Level[Enum.GetNames(typeof(eLevels)).Length];

        
        

        public Level(List<Entity> entities)
        {
            TmxMap map = new TmxMap("Levels/" + Globals.currentLvl + ".tmx"); //TODO: Check if file even exists
            
            Vector3 size = new Vector3(map.Width, map.Layers.Count, map.Height);

            lvl = new Entity[map.Width, map.Layers.Count, map.Height];

            redSwitches = new List<Switch>();
            greenSwitches = new List<Switch>();
            blueSwitches = new List<Switch>();

            Vector3 blockSize = new Vector3(4, 4, 4);

            for (int y = 0; y < map.Layers.Count; y++)
            {
                for (int index = 0; index < map.Layers[y].Tiles.Count; index++)
                {
                    TmxLayerTile currentTile = map.Layers[y].Tiles[index];
                    
                    Vector3 position=new Vector3(currentTile.X, y, currentTile.Y);
                    Entity currentEntity;
                    Transform t = new Transform();
                    t.position = position * blockSize;

                    switch (currentTile.Gid)
                    {

                        case 0:
                            currentEntity = null;
                            break;
                        case 1:
                            currentEntity = new Block(t, blockSize, entities,this);
                            break;
                        case 2:
                            currentEntity = null;
                            break;
                        case 3:
                            currentEntity = new Goal(t, blockSize, entities, this);    
                            break;
                        case 4:
                            currentEntity = new Switch(t, blockSize, "red", entities, this);
                            redSwitches.Add((Switch)currentEntity);
                            break;
                        case 5:
                            currentEntity = new Switch(t, blockSize, "green", entities, this);
                            greenSwitches.Add((Switch)currentEntity);
                            break;
                        case 6:
                            currentEntity = new Switch(t, blockSize, "blue", entities, this);
                            blueSwitches.Add((Switch)currentEntity);
                            break;
                        case 7:
                            currentEntity = new Door(t, blockSize, "red", entities, this);
                            break;
                        case 8:
                            currentEntity = new Door(t, blockSize, "green", entities, this);
                            break;
                        case 9:
                            currentEntity = new Door(t, blockSize, "blue", entities, this);
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

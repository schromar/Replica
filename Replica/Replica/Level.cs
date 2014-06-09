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

        public Level(List<Entity> entities, string mapName)
        {
            TmxMap map = new TmxMap("Levels/" + mapName + ".tmx"); //TODO: Check if file even exists
            
            Vector3 size = new Vector3(map.Width, map.Layers.Count, map.Height);

            lvl = new Entity[map.Width, map.Layers.Count, map.Height];

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
                            currentEntity = new Block(entities, t);
                            break;
                        case 2:
                            currentEntity = null;
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
    }
}

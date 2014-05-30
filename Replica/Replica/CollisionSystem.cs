using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Replica
{
    class CollisionSystem
    {
        public static void CheckCollisions(List<Entity> entities)
        {
            for (int i = 0; i < entities.Count; i++) //Certain entities will create/delete other entities in their OnCollision
            {
                for (int j = i + 1; j < entities.Count; j++)
                {
                    if (entities[i].GetBounds().Intersects(entities[j].GetBounds()))
                    {
                        entities[i].OnCollision(entities[j]);
                        entities[j].OnCollision(entities[i]);
                    }
                }
            }
        }
    }
}

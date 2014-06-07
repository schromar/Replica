using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

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

        //Returns a List of Entities that collide with a Ray (sorted by their distance from said Ray)
        public static List<KeyValuePair<float, Entity>> RayIntersection(List<Entity> entities, Ray ray)
        {
            List<KeyValuePair<float, Entity>> res = new List<KeyValuePair<float, Entity>>();
            for (int i = 0; i < entities.Count; i++)
            {
                float? distance=ray.Intersects(entities[i].GetBounds());
                if (distance!=null)
                {
                    res.Add(new KeyValuePair<float, Entity>((float)distance, entities[i]));
                }
            }
            res.Sort(Compare);
            return res;
        }

        //Have to define own comparison for the KeyValuePair
        private static int Compare(KeyValuePair<float, Entity> a, KeyValuePair<float, Entity> b)
        {
            return a.Key.CompareTo(b.Key);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Replica
{
    class CollisionSystem
    {
        /// <summary>
        /// Checks for each Entity in the List whether it collides with each other Entity.
        /// In case they do we call OnCollision of both entities.
        /// </summary>
        /// <param name="entities">The List of entities in which we want to look for collisions</param>
        public static void CheckCollisions(List<Entity> entities)
        {
            for (int i = 0; i < entities.Count; i++) //Certain entities will create/delete other entities in their OnCollision
            {
                for (int j = i + 1; j < entities.Count; j++)
                {
                    if (entities[i].Bounds.Intersects(entities[j].Bounds)) //TODO 2: Proper collision optimization
                    {
                        entities[i].OnCollision(entities[j]);
                        entities[j].OnCollision(entities[i]);
                    }
                }
            }
        }

        /// <summary>
        /// Checks whether a ray intersects one or more entities.
        /// </summary>
        /// <param name="entities"></param>
        /// <param name="ray"></param>
        /// <returns>A List of entities that collide with the ray (sorted by their distance from said ray)</returns>
        public static List<KeyValuePair<float, Entity>> RayIntersection(List<Entity> entities, Ray ray)
        {
            List<KeyValuePair<float, Entity>> res = new List<KeyValuePair<float, Entity>>();
            for (int i = 0; i < entities.Count; i++)
            {
                float? distance = ray.Intersects(entities[i].Bounds);
                if (distance!=null)
                {
                    res.Add(new KeyValuePair<float, Entity>((float)distance, entities[i]));
                }
            }
            res.Sort(Compare);
            return res;
        }

        /// <summary>
        /// Have to define own comparison for the KeyValuePair
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        private static int Compare(KeyValuePair<float, Entity> a, KeyValuePair<float, Entity> b)
        {
            return a.Key.CompareTo(b.Key);
        }
    }
}

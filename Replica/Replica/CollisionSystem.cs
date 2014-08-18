using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Replica.Statics;

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
                bool check = true;
                foreach (EntityType type in Globals.collisionDisabled)
                {
                    if (entities[i].Type == type)
                    {
                        check = false;
                        break;
                    }
                }

                if (check)
                {
                    for (int j = 0; j < entities.Count; j++)
                    {
                        if (entities[i].Bounds.Intersects(entities[j].Bounds)) //TODO 2: Proper collision optimization
                        {
                            entities[i].OnCollision(entities[j]);
                            entities[j].OnCollision(entities[i]);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Checks whether a ray intersects one or more (solid) entities.
        /// </summary>
        /// <param name="entities"></param>
        /// <param name="ray"></param>
        /// <returns>A List of entities that collide with the ray (sorted by their distance from said ray)</returns>
        public static float? RayIntersection(List<Entity> entities, Ray ray, Entity exception)
        {
            float? min = null;
            for (int i = 0; i < entities.Count; i++)
            {
                if (entities[i].Solid && entities[i] != exception)
                {
                    float? distance = ray.Intersects(entities[i].Bounds);

                    Vector3[] corners = entities[i].Bounds.GetCorners();

                    if (distance != null)
                    {
                        if (min == null || distance < min)
                        {
                            min = distance;
                        }
                    }
                }
            }
            return min;
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

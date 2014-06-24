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
                    if (!(entities[i].GetEntityType()==Entity.EntityType.Block && entities[j].GetEntityType()==Entity.EntityType.Block)
                        && entities[i].GetBounds().Intersects(entities[j].GetBounds()))
                    {
                        entities[i].OnCollision(entities[j]);
                        entities[j].OnCollision(entities[i]);
                    }
                }
            }
        }

        public static List<Plane> PlanesFromBox(BoundingBox box)
        {
            List<Plane> planes = new List<Plane>();
            Vector3[] corners=box.GetCorners();
            //Numbers
            planes.Add(new Plane(corners[7], corners[4], corners[5]));
            planes.Add(new Plane(corners[6], corners[5], corners[1]));
            planes.Add(new Plane(corners[2], corners[1], corners[0]));
            planes.Add(new Plane(corners[3], corners[0], corners[4]));
            planes.Add(new Plane(corners[4], corners[0], corners[1]));
            planes.Add(new Plane(corners[7], corners[3], corners[2]));
            return planes;
        }

        public static Vector3 OverlappingPoint(BoundingBox box1, BoundingBox box2)
        {
            //Assuming boxes already overlap? Not sure which point will get chosen
            Vector3 result = new Vector3();

            if (box1.Min.X > box2.Min.X)
            {
                result.X = box1.Min.X;
            }
            else
            {
                result.X = box1.Max.X;
            }

            if (box1.Min.Y > box2.Min.Y)
            {
                result.Y = box1.Min.Y;
            }
            else
            {
                result.Y = box1.Max.Y;
            }

            if (box1.Min.Z > box2.Min.Z)
            {
                result.Z = box1.Min.Z;
            }
            else
            {
                result.Z = box1.Max.Z;
            }

            return result;
        }

        public static BoundingBox Intersection(BoundingBox box1, BoundingBox box2)
        {
            BoundingBox result = new BoundingBox();

            //Setting Min
            if (box1.Min.X > box2.Min.X)
            {
                result.Min.X = box1.Min.X;
            }
            else
            {
                result.Min.X = box2.Min.X;
            }

            if (box1.Min.Y > box2.Min.Y)
            {
                result.Min.Y = box1.Min.Y;
            }
            else
            {
                result.Min.Y = box2.Min.Y;
            }

            if (box1.Min.Z > box2.Min.Z)
            {
                result.Min.Z = box1.Min.Z;
            }
            else
            {
                result.Min.Z = box2.Min.Z;
            }

            //Setting Max
            if (box1.Max.X < box2.Max.X)
            {
                result.Max.X = box1.Max.X;
            }
            else
            {
                result.Max.X = box2.Max.X;
            }

            if (box1.Max.Y < box2.Max.Y)
            {
                result.Max.Y = box1.Max.Y;
            }
            else
            {
                result.Max.Y = box2.Max.Y;
            }

            if (box1.Max.Z < box2.Max.Z)
            {
                result.Max.Z = box1.Max.Z;
            }
            else
            {
                result.Max.Z = box2.Max.Z;
            }

            return result;
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

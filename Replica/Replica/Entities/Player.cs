using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Replica.Entities
{
    class Player : Entity
    {
        Vector2 resolution;

        float mouseSpeed;
        float movementSpeed;
        Vector2 rotation;

        Camera camera;
        Model model;

        Vector3 prevMovement;
        FootSensor foot;

        float yVelocity;
        float gravity;

        public Player(List<Entity> entities, Level lvl, int windowWidth, int windowHeight, Model model)
            : base(entities, lvl, EntityType.Player, new Transform(), new Vector3(2, 2, 2))
        {
            transform.position = new Vector3(5, 100, 5);

            resolution = new Vector2(windowWidth, windowHeight);

            mouseSpeed = 0.1f;
            movementSpeed = 5;
            rotation = Vector2.Zero;

            camera = new Camera(resolution);
            this.model = model;

            prevMovement = Vector3.Zero;
            Vector3 footBoundsSize = new Vector3(0.5f);
            Transform footTransform=new Transform();
            footTransform.position=new Vector3(transform.position.X, bounds.Min.Y/*+footBoundsSize.Y/2.0f*/, transform.position.Z);
            foot = new FootSensor(entities, lvl, footTransform, footBoundsSize);
            entities.Add(foot);

            yVelocity = -20;
            gravity = -0.25f;
        }

        public override void Update(GameTime gameTime)
        {
            Rotate(gameTime);
            MoveXZ(gameTime);
            MoveY(gameTime);
            camera.SetTransform(transform);

            Console.WriteLine(lvl.numberOfReplicants);
            Console.WriteLine(lvl.maxReplicants);

            //Spawn Replicant on mouseclick
            MouseState mState = Mouse.GetState();

            if (lvl.numberOfReplicants < lvl.maxReplicants)
            {
                //if (mState.RightButton == ButtonState.Pressed)
                if (Input.isClicked(Keys.F1))
                {
                    SpawnReplicant();
                }
            }
        }

        public override void OnCollision(Entity entity)
        {
            if (entity.isSolid() && prevMovement!=Vector3.Zero)
            //if (entity.GetEntityType() == EntityType.Block || entity.GetEntityType() == EntityType.Replicant)
            {
                ///Simple version
                Vector3 backwards = prevMovement;

                ///Intersectionbox combined with prevMovement
                /*BoundingBox intersection=CollisionSystem.Intersection(bounds, entity.GetBounds());
                Vector3 intersectionVec = new Vector3(intersection.Max.X - intersection.Min.X, intersection.Max.Y - intersection.Min.Y, intersection.Max.Z - intersection.Min.Z);
                Vector3 backwards = prevMovement;
                backwards.Normalize();
                backwards *= intersectionVec;*/

                ///Moving towards smaller side of Intersectionbox (least likely to work)
                /*Vector3 backwards=new Vector3();
                BoundingBox intersection = CollisionSystem.Intersection(bounds, entity.GetBounds());
                Vector3 intersectionVec = new Vector3(intersection.Max.X - intersection.Min.X, intersection.Max.Y - intersection.Min.Y, intersection.Max.Z - intersection.Min.Z);
                if (intersectionVec.X < intersectionVec.Y)
                {
                    if (intersectionVec.X < intersectionVec.Z)
                    {
                        backwards.X = intersectionVec.X;
                    }
                    else
                    {
                        backwards.Z = intersectionVec.Z;
                    }
                }
                else
                {
                    if (intersectionVec.Y < intersectionVec.Z)
                    {
                        backwards.Y = intersectionVec.Y;
                    }
                    else
                    {
                        backwards.Z = intersectionVec.Z;
                    }
                }*/

                ///Using point from Intersectionbox and ray from that point towards box face
                /*Vector3 backwards = prevMovement;
                backwards.Normalize();
                Vector3 point = CollisionSystem.OverlappingPoint(bounds, entity.GetBounds());
                Ray ray = new Ray(point, -backwards);

                List<Plane> planes=CollisionSystem.PlanesFromBox(entity.GetBounds());
                List<float> distances = new List<float>();
                foreach (Plane plane in planes)
                {
                    float? distance = ray.Intersects(plane);
                    if (distance != null)
                    {
                        distances.Add((float)distance);
                    }
                }
                backwards *= distances.Min();*/

                SetPosition(transform.position - backwards);
                //prevMovement = -backwards;
                if (foot.IsActivated())
                {
                    yVelocity = 0;
                }
            }
        }

        public override void SetPosition(Vector3 position)
        {
            base.SetPosition(position);
            camera.SetTransform(transform);
            foot.SetPosition(new Vector3(transform.position.X, bounds.Min.Y, transform.position.Z));
        }

        public Camera GetCamera()
        {
            return camera; //What happens if camera is changed after getter was used?
        }

        void Rotate(GameTime gameTime)
        {
            MouseState mState = Mouse.GetState();

            Vector2 mouseMovement = resolution / 2 - new Vector2(mState.X, mState.Y);
            Mouse.SetPosition((int)resolution.X / 2, (int)resolution.Y / 2);

            rotation += mouseMovement * mouseSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;

            //Math. It happens. Here.
            transform.forward = new Vector3((float)Math.Cos(rotation.Y) * (float)Math.Sin(rotation.X),
                                            (float)Math.Sin(rotation.Y),
                                            (float)Math.Cos(rotation.Y) * (float)Math.Cos(rotation.X));
            transform.right = new Vector3((float)Math.Sin(rotation.X - Math.PI / 2.0f),
                                        0,
                                        (float)Math.Cos(rotation.X - Math.PI / 2.0f));
            transform.up = Vector3.Cross(transform.right, transform.forward);
        }

        void MoveXZ(GameTime gameTime)
        {          
            Vector2 movement = Vector2.Zero;

            if (Input.isPressed(Keys.W))
            {
                movement.X += movementSpeed;
            }
            if (Input.isPressed(Keys.S))
            {
                movement.X -= movementSpeed;
            }
            if (Input.isPressed(Keys.A))
            {
                movement.Y -= movementSpeed;
            }
            if (Input.isPressed(Keys.D))
            {
                movement.Y += movementSpeed;
            }

            if (movement.Length() > movementSpeed) //Diagonal movement must not be faster
            {
                movement.Normalize();
                movement *= movementSpeed;
            }
            movement *= (float)gameTime.ElapsedGameTime.TotalSeconds;
            Vector3 forwardWithoutY = transform.forward;
            forwardWithoutY.Y = 0;
            Vector3 finalVelocity = forwardWithoutY * movement.X + transform.right * movement.Y;

            SetPosition(transform.position + finalVelocity);
            prevMovement.X = finalVelocity.X;
            prevMovement.Z = finalVelocity.Z;
        }

        void MoveY(GameTime gameTime)
        {
            yVelocity = yVelocity + gravity;

            Vector3 jumpVector=new Vector3();
            if (foot.IsActivated())
            {
                if (Input.isPressed(Keys.Space))
                {
                    jumpVector.Y = 100;
                }
            }
            Vector3 yVector = new Vector3(0, yVelocity, 0);
            yVector += jumpVector;
            yVector *= (float)gameTime.ElapsedGameTime.TotalSeconds;

            SetPosition(transform.position + yVector);
            prevMovement.Y = yVector.Y;
        }

        void SpawnReplicant()
        {
            Transform replicantTransform = transform;
            replicantTransform.position = transform.position + transform.forward*boundsSize.Length();
            Replicant replicant = new Replicant(entities, lvl, replicantTransform, boundsSize, model);
            bool spawning = true;
            foreach (Entity entity in entities)
            {
                if (replicant.GetBounds().Intersects(entity.GetBounds()))
                    if (entity.GetEntityType() == EntityType.Block || entity.GetEntityType() == EntityType.Door || entity.GetEntityType() == EntityType.Goal)
                {
                    spawning = false;
                    break;
                }
            }
            if (spawning)
            {

                lvl.numberOfReplicants++;
                entities.Add(replicant);
            }

            //ALTERNATIVE SPAWNING
            //Create Ray from Player Transform to check if Player is looking at Entities
            /*Ray ray = new Ray(transform.position, transform.forward);
            List<KeyValuePair<float, Entity>> collisions = CollisionSystem.RayIntersection(entities, ray);

            //Check whether looked at Entity is solid to spawn the Replicant on
            int solidIndex = -1;
            for (int i = 0; i < collisions.Count; i++)
            {
                if (collisions[i].Value.GetEntityType() == EntityType.Block)
                {
                    solidIndex = i;
                    break;
                }
            }

            if (solidIndex != -1) //Only spawn Replicant if we are not looking into infinity?
            {
                Transform replicantTransform = transform;
                replicantTransform.position = transform.position + transform.forward * collisions[solidIndex].Key;
                entities.Add(new Replicant(replicantTransform, model, entities, lvl));
            }*/
        }
    }
}

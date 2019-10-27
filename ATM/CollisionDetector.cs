using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using NUnit.Framework;

namespace ATM.System
{
    public class CollisionDetector: ICollisionDetector
    {
        private ICollisionCollection collisionCollection = new CollisionCollection();

        public event EventHandler<CollisionArgs> newCollision;


        public CollisionDetector()
        {
            var log = new Log(this);
        }

        public void DetectCollisions(List<Flight> flights)
        {
            var sortedFlights = flights.OrderBy(f => f.TData.X).ToList();
            var collisions = GenerateCollisions(sortedFlights);
            var newCollisions = collisionCollection.HandleUpdatedCollisions(collisions);
            EmitNewCollisions(newCollisions);
        }

        public List<Collision> GenerateCollisions(List<Flight> sortedFlights)
        {
            List<Collision> collisions = new List<Collision>();
            for (var i = 0; i < sortedFlights.Count; i++)
            {
                var currentFlight = sortedFlights[i];
                var j = i + 1;

                if (j >= sortedFlights.Count) continue;
                while (sortedFlights[j].TData.X - currentFlight.TData.X < 5000)
                {
                    if (IsCollision(currentFlight, sortedFlights[j]))
                    {
                        collisions.Add(new Collision(currentFlight, sortedFlights[j], DateTime.Now));
                    }
                }
            }

            return collisions;
        }

       
        public bool IsCollision(Flight flightA, Flight flightB)
        {
            int deltaX = Math.Abs(flightA.TData.X - flightB.TData.X);
            int deltaY = Math.Abs(flightA.TData.Y - flightB.TData.Y);

            double distance = Math.Sqrt(deltaX * deltaX + deltaY * deltaY);
            int altitudeDistance = Math.Abs(flightA.TData.Altitude - flightB.TData.Altitude);

            if (altitudeDistance < 300 && distance < 5000)
            {
                return true;
            }
            else return false;
        }
        private void EmitNewCollisions(List<Collision> newCollisions)
        {
            foreach (var c in newCollisions)
            {
                EmitNewCollisions(c);
            }
        }

        public void EmitNewCollisions(Collision col)
        {
            newCollision?.Invoke(this, new CollisionArgs(col));
        }

    }
}
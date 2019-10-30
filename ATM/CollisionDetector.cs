﻿using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Windows.Controls;
using NUnit.Framework;

namespace ATM.System
{
    public class CollisionDetector: ICollisionDetector
    {
        public ICollisionCollection CollisionCollection {get; private set;}
        public List<Collision> Collisions => CollisionCollection.Collisions;

        public event EventHandler<CollisionArgs> NewCollision;


        public CollisionDetector(IFlightCollection flightCollection, ICollisionCollection collisionCollection)
        {
            flightCollection.flightsChanged += OnFlightsChanged;
            CollisionCollection = collisionCollection;

            ILog logger = new Log(this);
        }

        public void OnFlightsChanged(object sender, FlightArgs e)
        {
            DetectCollisions(e.flights);
        }

        public void DetectCollisions(List<Flight> flights)
        {
            var sortedFlights = flights.OrderBy(f => f.TData.X).ToList();
            var updatedCollisions = GenerateCollisions(sortedFlights);
            var newCollisions = CollisionCollection.HandleUpdatedCollisions(updatedCollisions);
            EmitNewCollisions(newCollisions);
        }

        private List<Collision> GenerateCollisions(List<Flight> sortedFlights)
        {
            List<Collision> collisions = new List<Collision>();
            for (var i = 0; i < sortedFlights.Count; i++)
            {
                var currentFlight = sortedFlights[i];
                var j = i + 1;

                while (j < sortedFlights.Count && sortedFlights[j].TData.X - currentFlight.TData.X < 5000)
                {
                    if (IsCollision(currentFlight, sortedFlights[j]))
                    {
                        collisions.Add(new Collision(currentFlight, sortedFlights[j], DateTime.Now));
                    }

                    j++;
                }
            }
            return collisions;
        }

       
        private bool IsCollision(Flight flightA, Flight flightB)
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
                EmitNewCollision(c);
            }
        }

        private void EmitNewCollision(Collision col)
        {
            NewCollision?.Invoke(this, new CollisionArgs(col));
        }

    }
}
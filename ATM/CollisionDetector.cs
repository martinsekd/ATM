using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using NUnit.Framework;

namespace ATM.System
{
    public class CollisionDetector: ICollisionDetector
    {
        private ICollisionCollection collisionCollection = new CollisionCollection();

        public event EventHandler<CollisionArgs> newCollision;


        public CollisionDetector()
        {

        }

        public void DetectCollisions(List<Flight> flights)
        { 
        }

        public void EmitNewCollisions(Collision col)
        {
            newCollision?.Invoke(this, new CollisionArgs(col));
        }

    }
}
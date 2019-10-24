using System.Collections.Generic;
using System.Linq;
using NUnit.Framework.Constraints;

namespace ATM.System
{
    public class CollisionCollection: ICollisionCollection
    {
        private List<Collision> collisions = new List<Collision>();

        public CollisionCollection()
        {
        }

        public CollisionCollection(List<Collision> collisions)
        {
            this.collisions = collisions;
        }

        //Takes a new List of collisions, creates a list of collisions that are in the supplied list and not in the first
        //Sets the collection to the updated list and returns the new collisions
        public List<Collision> HandleUpdatedCollisions(List<Collision> updatedCollisions)
        {
            List <Collision> newCollisions = collisions.Except(updatedCollisions).ToList();

            this.collisions = updatedCollisions;

            return newCollisions;

        }
    }
}
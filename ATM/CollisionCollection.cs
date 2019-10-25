using System.Collections.Generic;
using System.Linq;
using NUnit.Framework.Constraints;

namespace ATM.System
{
    public enum koorTypes
    {
        X,
        Y,
        H
    }

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
        private void startQuicksort(List<Flight> ar, int n,koorTypes type)
        {
            quicksort(ar, 0, n - 1, type);
        }

        private void quicksort(List<Flight> ar, int start, int slut, koorTypes type)
        {
            if (slut - start < 1) return;
            Flight pivot = ar[slut];
            int i = start - 1;
            int j = start;

            while (j < slut)
            {
                //if (ar[j] > pivot)
                if (compare(pivot, ar[j], type))
                {
                    j++;
                }
                else
                {
                    i++;
                    Flight a = ar[i];
                    Flight b = ar[j];
                    ar[i] = b;
                    ar[j] = a;
                    j++;
                }
            }
            Flight b2 = ar[i + 1];
            ar[i + 1] = pivot;
            ar[slut] = b2;

            quicksort(ar, start, i, type);
            quicksort(ar, i + 2, slut, type);
        }

        private bool compare(Flight a,Flight b, koorTypes type)
        {
            switch(type)
            {
                case koorTypes.X:
                    if(a.TData.X<b.TData.X)
                    {
                        return true;
                    } else
                    {
                        return false;
                    }
                    break;
                case koorTypes.Y:
                    if (a.TData.Y < b.TData.Y)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                    break;
                case koorTypes.H:
                    if (a.TData.Altitude < b.TData.Altitude)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                    break;
                default:
                    return true;
                    break;
            }
        }
    }
}
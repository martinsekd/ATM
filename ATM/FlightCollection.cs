using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATM
{
    class FlightCollection
    {
        private bool seperation = false;
        public void Seperation(TransponderData t1, TransponderData t2)
        {
            int deltaX = t1.X - t2.X;
            int deltaY = t1.Y - t2.Y;

            double distance = Math.Sqrt(deltaX * deltaX + deltaY * deltaY);
            int altitudeDistance = Math.Abs(t1.Altitude - t2.Altitude);

            if(altitudeDistance < 300 && distance < 5000)
            {
                seperation = true;
            } else
            {
                seperation = false;
            }

        }
    }
}

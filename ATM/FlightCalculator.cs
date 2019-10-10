using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATM
{
    class FlightCalculator
    {
        public int CalculateSpeed(TransponderData oldData, TransponderData newData)
        {
            int deltaX = oldData.X - newData.X;
            int deltaY = oldData.Y - newData.Y;

            double distance = Math.Sqrt(deltaX * deltaX + deltaY * deltaY);
            TimeSpan timespan = newData.Time - oldData.Time;
            int timedif = (int)timespan.TotalMilliseconds;
            return (int) Math.Round(distance/(timedif*1000),0);
        }

        public double CalculateDegree(TransponderData oldData, TransponderData newData)
        {
            double deltaX = newData.X - oldData.X;
            double deltaY = newData.Y - oldData.Y;

            double toDegreeFactor = 360 / (2 * Math.PI);

            double angle = Math.Atan(deltaY/deltaX)*toDegreeFactor;

            double degree = 0;
            if(newData.X > oldData.X)
            {
                degree = 90 - angle;
            } else if (newData.X < oldData.X)
            {
                degree = 360 - (angle + 90);
            } else
            {
                if(newData.Y > oldData.Y)
                {
                    degree = 0;
                } else if(newData.Y < oldData.Y)
                {
                    degree = 180;
                } else
                {
                    degree = 0;
                }
            }

            return degree;
        }

    }
}

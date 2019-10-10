using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATM
{
    class FlightCalculator: IFlightCalculator
    {
    public double CalculateSpeed(TransponderData oldData, TransponderData newData)
    {
        double oldX = Convert.ToDouble(oldData.X);
        double oldY = Convert.ToDouble(oldData.Y);
        double newX = Convert.ToDouble(newData.X);
        double newY = Convert.ToDouble(newData.Y);

        double distance = Math.Sqrt(Math.Pow(oldX - newX, 2) + Math.Pow(oldY - newY, 2));
        double timeDif = (oldData.Time - newData.Time).TotalMilliseconds;

        return distance / timeDif / 1000;
    }

    public double CalculateDirection(TransponderData oldData, TransponderData newData)
    {
        return 0;
    }
    }
}

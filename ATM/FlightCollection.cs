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

            if (altitudeDistance < 300 && distance < 5000)
            {
                seperation = true;
            }
            else
            {
                seperation = false;
            }
        }

        private List<Flight> FlightList;
        private IFlightCalculator flightCalculator;
        private IDataFormatter dataFormatter;
        public FlightCollection(IFlightCalculator flightCalculator, IDataFormatter dataFormatter)
        {
            this.FlightList = new List<Flight>();
            this.flightCalculator = flightCalculator;
            this.dataFormatter = dataFormatter;
        }

        public void HandleNewData(string s)
        {
            TransponderData newData = dataFormatter.StringToTransponderData(s);

            if (FlightList.Exists(f => f.TData.Tag == newData.Tag))
            {
                Flight flight = FlightList.Find(f => f.TData.Tag == newData.Tag);
                FlightList.Remove(flight);

                flight.Speed = flightCalculator.CalculateSpeed(flight.TData, newData);
                flight.Direction = flightCalculator.CalculateDirection(flight.TData, newData);
                flight.TData = newData;
                FlightList.Add(flight);
            }
            else
            {
                FlightList.Add(new Flight(newData));
            }
        }

        public void Render()
        {
            Console.Clear();
            foreach (Flight f in FlightList)
            {
                Console.WriteLine("Flight: {0}, Position: {1}, {2}, Altitude: {3}, Speed: {4}, Direction {5}", f.TData.Tag, f.TData.X, f.TData.Y, f.TData.Altitude, f.Speed, f.Direction);
            }
        }
    }
}

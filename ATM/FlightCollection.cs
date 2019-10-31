using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace ATM.System
{
    public class FlightCollection : IFlightCollection
    {
        private List<Flight> FlightList;

        private IFlightCalculator flightCalculator;
        //private IDataFormatter dataFormatter;

        private IRender render;

        public FlightCollection(IFlightCalculator flightCalculator, IFlightFilter flightFilter)
        {
            flightFilter.transponderFilterChanged += getTransponderData;
            this.FlightList = new List<Flight>();
            this.flightCalculator = flightCalculator;

            ICollisionDetector collisionDetector = new CollisionDetector(this, new CollisionCollection());
            render = new Render(this,new Console());
        }

        public void getTransponderData(object sender,TransponderArgs e)
        {
            var transponderList = e.transponderData;

            foreach(TransponderData transponderData in transponderList)
            {
                HandleNewData(transponderData);
            }
            notify();
        }

        public event EventHandler<FlightArgs> flightsChanged;

        protected virtual void OnFlightsChanged(FlightArgs e)
        {
            flightsChanged?.Invoke(this,e);
        }

        public void HandleNewData(TransponderData newData)
        {
            

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

        public void notify()
        {
            OnFlightsChanged(new FlightArgs() { flights = FlightList });
        }

        /*public void Render()
        {
            
            //Console.Clear();
            for(int i=0;i<FlightList.Count;i++)
            {
                Flight f = FlightList[i];
                if (Program.shapes[i] != null)
                {
                    Program.setflight(f.TData.X / 200, f.TData.Y / 200,i);
                }
                //Console.WriteLine("Flight: {0}, Position: {1}, {2}, Altitude: {3}, Speed: {4}, Direction {5}", f.TData.Tag, f.TData.X, f.TData.Y, f.TData.Altitude, f.Speed, f.Direction);
            }
        }*/
    }
}

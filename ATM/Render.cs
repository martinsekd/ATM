using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATM.System
{
    class Render : IRender
    {
        public Render(IFlightCollection col)
        {
            col.flightsChanged += RenderFlights;
        }

        public void RenderFlights(object sender, FlightArgs e)
        {
            List<Flight> flights = e.flights;

            Console.Clear();
            //Canvas.SetTop(Program.shape, 50);
            //Canvas.SetLeft(Program.shape, 50);

            //foreach (Flight f in FlightList)
            for (int i = 0; i < flights.Count; i++)
            {
                Flight f = flights[i];
                if (Program.shapes[i] != null)
                {
                    Program.setflight(f.TData.X / 200, f.TData.Y / 200, i);
                }
                Console.WriteLine("Flight: {0}, Position: {1}, {2}, Altitude: {3}, Speed: {4}, Direction {5}", f.TData.Tag, f.TData.X, f.TData.Y, f.TData.Altitude, f.Speed, f.Direction);
            }
        }

    }
}

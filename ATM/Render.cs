using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ATM.Interfaces;

namespace ATM.System
{
    public class Render : IRender
    {
        private IConsole console_;

        public Render(IFlightCollection col,IConsole console)
        {
            console_ = console;
            col.flightsChanged += RenderFlights;
            
        }


        public void RenderFlights(object sender, FlightArgs e)
        {
            flightsChanged?.Invoke(this, e);

            List<Flight> flights = e.flights;
            console_.Clear();

            for (int i = 0; i < flights.Count; i++)
            {
                Flight f = flights[i];
                //if (Program.shapes[i] != null)
                //{
                //    Program.setflight(f.TData.X / 200, f.TData.Y / 200, i);
                //}
                console_.WriteLine(f.TData.Tag,f.TData.X,f.TData.Y,f.TData.Altitude,f.Speed,f.Direction);
                
                //console_.WriteLine("Flight: {0}, Position: {1}, {2}, Altitude: {3}, Speed: {4}, Direction {5}", f.TData.Tag, f.TData.X, f.TData.Y, f.TData.Altitude, f.Speed, f.Direction);
            }
            
        }

        protected virtual void OnTransponderChanged(FlightArgs e)
        {
            flightsChanged?.Invoke(this, e);
        }

        public event EventHandler<FlightArgs> flightsChanged;
    }
}

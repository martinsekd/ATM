using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATM.System
{
    public class Render : IRender
    {
        public bool renderInGui = true;
        private IConsole console_;

        public Render(IFlightCollection col,IConsole console,bool renderInGui)
        {
            this.console_ = console;
            this.renderInGui = renderInGui;

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
                if (renderInGui && Program.shapes[i] != null)
                {
                    //This is not covered by tests, since we do not test the GUI implementation and this affects the GUI
                    Program.setflight(f.TData.X / 200, f.TData.Y / 200, i);
                }

                console_.WriteLine(string.Format("Flight: {0}\n\tPosition X:{1} Y:{2}\tAltitude: {3}\n\tSpeed: {4}\t\tDirection {5}\n", f.TData.Tag, f.TData.X, f.TData.Y, f.TData.Altitude, f.Speed, f.Direction));
                
                //console_.WriteLine("Flight: {0}, Position: {1}, {2}, Altitude: {3}, Speed: {4}, Direction {5}", f.TData.Tag, f.TData.X, f.TData.Y, f.TData.Altitude, f.Speed, f.Direction);
            }
            
        }

        

        public event EventHandler<FlightArgs> flightsChanged;
    }
}

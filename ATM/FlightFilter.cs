using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATM.System
{
    public class FlightFilter : IFlightFilter
    {
        private List<TransponderData> transponderListe;
        public FlightFilter(IDataFormatter dataFormatter)
        {
            FlightCollection flightCollection = new FlightCollection(new FlightCalculator(), this);
            dataFormatter.transponderChanged += FilterFlight;
        }

        public event EventHandler<TransponderArgs> transponderFilterChanged;

        public void FilterFlight(object sender, TransponderArgs e)
        {
            transponderListe = e.transponderData;
            foreach (var transponder in transponderListe.ToList())
            {
                if (transponder.X < 10000 || transponder.X > 90000)
                {
                    transponderListe.Remove(transponder);

                }
                else if (transponder.Y < 10000 || transponder.Y > 90000)
                {
                    transponderListe.Remove(transponder);

                }
                else if (transponder.Altitude < 500 || transponder.Altitude > 20000)
                {
                    transponderListe.Remove(transponder);
                }
            }
            transponderFilterChanged?.Invoke(this, new TransponderArgs { transponderData = transponderListe });
        }


    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATM.System
{
    public sealed class ATM
    {
        //private static readonly ATM instance = new ATM(new DataFormatter(TransponderArgs), new FlightCalculator());
        private FlightCollection flightCollection;

        static ATM()
        {

        }

        private ATM(IDataFormatter dataFormatter, IFlightCalculator flightCalculator)
        {
            flightCollection = new FlightCollection(flightCalculator, dataFormatter);
        }

        /*public static void Receive(object sender, TransponderReceiver.RawTransponderDataEventArgs e)
        {
            foreach (var data in e.TransponderData)
            {
                instance.flightCollection.HandleNewData(data);
            }

            instance.flightCollection.notify();
        }

        public static ATM Instance
        {
            get { return instance; }
        }*/
    }
}
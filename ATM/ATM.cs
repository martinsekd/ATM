using System.Data;
using ATM.Interfaces;
using ATM.System;
using TransponderReceiver;

namespace ATM.System
{
    public class ATM
    {
        private ITransponderReceiver receiver;
        private IDataFormatter dataFormatter;
        private IFlightFilter flightFilter;
        private IFlightCollection flightCollection;
        private ICollisionDetector collisionDetector;
        private ILog logger;
        private IConsole console;
        private IRender render;
        public ATM()
        {
            receiver = TransponderReceiver.TransponderReceiverFactory.CreateTransponderDataReceiver();

            dataFormatter = new DataFormatter(receiver);
            flightFilter = new FlightFilter(dataFormatter);
            flightCollection = new FlightCollection(new FlightCalculator(), flightFilter);
            collisionDetector = new CollisionDetector(flightCollection, new CollisionCollection());
            logger = new Log(collisionDetector);
            console = new Console();
            render = new Render(flightCollection,console);
        }
    }
}
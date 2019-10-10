using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATM
{
    public class Program
    {
        
        static void Main(string[] args)
        {
            var rec = TransponderReceiver.TransponderReceiverFactory.CreateTransponderDataReceiver();

            rec.TransponderDataReady += receive;

            FlightCalculator fc = new FlightCalculator();
            TransponderData t1 = new TransponderData("HEJA",0, 0, 20, new DateTime());
            TransponderData t2 = new TransponderData("HEJA", -9, -4, 20, new DateTime());

            Console.WriteLine(fc.CalculateDegree(t1,t2));
            while (true)
            {

            }
            
        }

        static public void receive(object sender, TransponderReceiver.RawTransponderDataEventArgs e)
        {
            List<TransponderData> TransponderListe = new List<TransponderData>();
            foreach (var data in e.TransponderData)
            {
                //string[] newstring = data.Split(';');
                //TransponderListe.Add(new TransponderData(newstring[0], newstring[1], newstring[2], newstring[3], newstring[4]));
                //System.Console.WriteLine($"Transponderdata {data}");
            }
        }
    }
}

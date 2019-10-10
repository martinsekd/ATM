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

            while(true)
            {

            }
            
        }

        static public void receive(object sender, TransponderReceiver.RawTransponderDataEventArgs e)
        {
            List<TransponderData> TransponderListe = new List<TransponderData>();
            foreach (var data in e.TransponderData)
            {
                string[] newstring = data.Split(';');
                TransponderListe.Add(new TransponderData(newstring[0], newstring[1], newstring[2], newstring[3], newstring[4]));
                //System.Console.WriteLine($"Transponderdata {data}");
            }
        }
    }
}

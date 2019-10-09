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
            foreach (var data in e.TransponderData)
            {
                System.Console.WriteLine($"Transponderdata {data}");
            }
        }
    }
}

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

            rec.TransponderDataReady += ATM.Receive;

            while(true)
            {

            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATM
{
    class DataFormatter
    {
        public TransponderData StringToTransponderData(string str)
        {
            string[] input = str.Split(';');
            
            if(input.Length != 5)
            {
                throw new InvalidInputException("String was not of the expected format (Tag;X;Y;Altitude;Timestamp)");
            }

            string tag = input[0];
            int X = int.Parse(input[1]);
            int Y = int.Parse(input[2]);
            int altitude = int.Parse(input[3]);
            DateTime timeStamp = DateTime.ParseExact(input[4], "yyyyMMddHHmmssFFF", null);

            return new TransponderData(tag, X, Y, altitude, timeStamp);
        }
    }
}

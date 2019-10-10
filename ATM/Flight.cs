using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATM
{
    public class Flight
    {
        public TransponderData TData { get; set; }
        public double Speed { get; set; }
        public double Direction { get; set; }

        public Flight(TransponderData data)
        {
            TData = data;
            Speed = 0;
            Direction = 0;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATM.System
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

        private bool Equals(Flight compare)
        {
            if (this.TData.Tag == compare.TData.Tag &&
                this.TData.X == compare.TData.X &&
                this.TData.Y == compare.TData.Y &&
                this.TData.Altitude == compare.TData.Y &&
                this.TData.Time == compare.TData.Time &&
                this.Speed.Equals(compare.Speed) &&
                this.Direction.Equals(compare.Direction))
            {
                return true;
            }

            return false;
        }

        public override bool Equals(object obj)
        {
            return this.Equals(obj as Flight);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ATM.System;

namespace ATM.System
{
    class Console : IConsole
    {
        public void WriteLine(string Tag, int X, int Y, int Altitude, double Speed, double Direction)
        {
            global::System.Console.WriteLine("Flight: {0}\n\tPosition X:{1} Y:{2}\tAltitude: {3}\n\tSpeed: {4}\t\tDirection {5}\n", Tag, X, Y, Altitude, Speed, Direction);
        }

        public void Clear()
        {
            global::System.Console.Clear();
        }
    }
}

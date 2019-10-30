using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ATM.Interfaces;

namespace ATM
{
    class Console : IConsole
    {
        public void WriteLine(string Tag, int X, int Y, int Altitude, double Speed, double Direction)
        {
            global::System.Console.WriteLine("Flight: {0}, Position: {1}, {2}, Altitude: {3}, Speed: {4}, Direction {5}", Tag, X, Y, Altitude, Speed, Direction);
        }

        public void Clear()
        {
            global::System.Console.Clear();
        }
    }
}

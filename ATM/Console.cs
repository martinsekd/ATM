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
        public void WriteLine(string buffer)
        {
            global::System.Console.WriteLine(buffer);
        }

        public void Clear()
        {
            global::System.Console.Clear();
        }
    }
}

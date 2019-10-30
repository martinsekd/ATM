using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATM.Interfaces
{
    public interface IConsole
    {
        void WriteLine(string buffer);
        void Clear();
    }
}

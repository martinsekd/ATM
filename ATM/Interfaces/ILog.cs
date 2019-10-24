using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ATM.System;

namespace ATM.Interfaces
{
    /*class logger
    {
        public void Writelog(string strFileName, string strMessage) { }
    }*/
    interface ILog
    {
        void Logwriter(object sender, CollisionArgs e);
    }
}

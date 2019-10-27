using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ATM.System;

namespace ATM.System
{
    /*class logger
    {
        public void Writelog(string strFileName, string strMessage) { }
    }*/
    public interface ILog
    {
        void Logwriter(object sender, CollisionArgs e);
    }
}

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ATM.Interfaces;

namespace ATM
{
    class Log : ILog
    {
        public static bool WriteLog(string strFileName, string strMessage)
        {
            try
            {
                FileStream objFilestream = new FileStream(string.Format("{0}\\{1}", Path.GetTempPath(), strFileName), FileMode.Append, FileAccess.Write);
                StreamWriter objStreamWriter = new StreamWriter((Stream)objFilestream);
                objStreamWriter.WriteLine(strMessage);
                objStreamWriter.Close();
                objFilestream.Close();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        WriteLogFile.WriteLog("ConsoleLog.txt", 
        String.Format("{0} @ {1}", "Log is Created by Martinique and Magnus the mosquito", DateTime.Now));
    }
}

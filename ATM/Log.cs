using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ATM.System;
using NUnit.Framework.Internal;

namespace ATM
{
    class Log : ILog
    {

        public Log(CollisionDetector collisionDetector)
        {
            collisionDetector.newCollision += Logwriter;
        }

        public void newCollisionDetected(object sender, CollisionArgs e)
        {

        }
        // Fil angivelse på skrivebord.
        private static string LogFile =
            Environment.GetFolderPath(
                Environment.SpecialFolder.Desktop) + "\\Log.txt";

        // tid + txt
        public static void WriteLine(string txt)
        {
            File.AppendAllText(LogFile,
                DateTime.Now.ToString() + ": " + txt);
        }

        // skriver collision event til logfil.
        private void Logwriter(object sender, CollisionArgs e)
        {
            var fly = e.Collision;
            Log.WriteLine(txt:"fly" + fly);
        }

        void ILog.Logwriter(object sender, CollisionArgs e)
        {
            throw new NotImplementedException();
        }




        /*public static bool WriteLog(string strFileName, string strMessage)
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
            //Log.WriteLog("ConsoleLog.txt", String.Format("{0} @ {1}", "Log is Created by Martinique and Magnus the mosquito", DateTime.Now));
        }*/
    }
}

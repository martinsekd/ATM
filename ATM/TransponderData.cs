using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ATM
{
    class TransponderData
    {
        private string tag_;
        private int X_;
        private int Y_;
        private int altitude_;
        private DateTime stamp_;

        public TransponderData(string tag, string X, string Y, string altitude, string stamp)
        {
            tag_ = tag;
            X_ = int.Parse(X);
            Y_ = int.Parse(Y);
            altitude_ = int.Parse(altitude);
            stamp_ = DateTime.ParseExact(stamp, "yyyyMMddHHmmssFFF",null);
            Console.Clear();
            Console.WriteLine("{0}",stamp_);
            Console.Write("{0}", stamp_);
            //Console.WriteLine("."+stamp_.Millisecond);
            
        }

    }
}

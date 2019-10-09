using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test.ATM
{
    public class test
    {
        public bool metodeA()
        {
            int a = 0;
            a++;
            return true;
        }

        public bool metodeB()
        {
            int a = 0;
            if(a==1)
            {
                Console.WriteLine("tal er 1");
                return true;
            } else
            {
                Console.WriteLine("Tal er 0");
                return false;
            }
        }
    }
}

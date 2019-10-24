using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATM.System
{

    interface IRender
    {

        void RenderFlights(object sender, FlightArgs e);
    }
}

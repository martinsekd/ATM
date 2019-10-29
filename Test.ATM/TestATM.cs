using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ATM.System;
using Castle.Core.Smtp;
using NSubstitute;
using TransponderReceiver;

namespace Test.ATM
{

    [TestFixture]
    public class TestATM
    {

        //FAKES
        private ICollisionDetector fakeCollisionDetector;
        private IDataFormatter fakeDataFormatter;
        private IFlightCalculator fakeFlightCalculator;
        private IFlightCollection fakeFlightCollection;
        private IFlightFilter fakeFlightFilter;
        private ILog fakeLog;
        private IRender fakeRender;


        private FlightCalculator uut;
        private FlightCollection flightCollection;

        [SetUp]
        public void setUp()
        {
            uut = new FlightCalculator();
        }

        [Test]
        public void something()
        {
            Assert.That(2 + 2, Is.EqualTo(4));
        }

        [Test]
        public void something2()
        {
            test uut = new test();
            Assert.That(uut.metodeA(), Is.EqualTo(true));
        }

        [Test]
        public void something3()
        {
            test uut = new test();
            Assert.That(uut.metodeB(), Is.EqualTo(true));
        }


        #region flightCalculator
        [TestCase(10, 40, 70, -100, 156.80)]
        [TestCase(10, 40, 10, 80, 0)]
        [TestCase(10, 40, 10, 10, 180)]
        [TestCase(10, 40, 10, 40, 0)]
        [TestCase(10, 100, 11, 10, 179.36)]
        [TestCase(10, 100, 11, 200, 0.57)]
        [TestCase(0, 0, -1, -100, 180.57)]
        public void CalculateDirection_add2TransponderData_c(int x1, int y1, int x2, int y2, double c)
        {
            TransponderData t1 = new TransponderData("TEST", x1, y1, 100, new DateTime());
            var t2 = new TransponderData("TEST1", x2, y2, 100, new DateTime());

            double degree = uut.CalculateDirection(t1, t2);

            Assert.That(degree, Is.InRange(c, c + 0.01));


        }

        #endregion

        #region flightCollection
        [Test]
        public void tester()
        {
            //arrange
            fakeFlightCalculator = Substitute.For<IFlightCalculator>();
            fakeFlightFilter = Substitute.For<IFlightFilter>();
            flightCollection = new FlightCollection(fakeFlightCalculator, fakeFlightFilter);

            //act

            //assert

        }

        #endregion

        #region Render
        #endregion

        #region DataFormatter
        [Test]
        public void test3()
        {
            //arrange
            var fakeReceiver = Substitute.For<ITransponderReceiver>();
            var fakeDataFormatter = Substitute.For<IDataFormatter>();

            fakeReceiver.TransponderDataReady += fakeDataFormatter.StringToTransponderData;
            List<string> flightList = new List<string>();
            TransponderArgs args = new TransponderArgs();
            TransponderArgs receiveArgs = null;
            
            //act
            flightList.Add("TTT10;20000;30000;14000;20101006213456789");
            fakeReceiver.TransponderDataReady += Raise.EventWith(this,new RawTransponderDataEventArgs(flightList));

            //assert
            fakeDataFormatter.Received(1).StringToTransponderData(Arg.Any<object>(),Arg.Is<RawTransponderDataEventArgs>(arg => arg.TransponderData.Contains("TTT10;20000;30000;14000;20101006213456789")));
        }

        [TestCase("TTT10;10000;30000;14000;20101006213456789",10000)]
        [TestCase("TTT10;10001;30000;14000;20101006213456789", 10001)]
        [TestCase("TTT10;9999;30000;14000;20101006213456789", 9999)]
        [TestCase("TTT10;90000;30000;14000;20101006213456789", 90000)]
        [TestCase("TTT10;90001;30000;14000;20101006213456789", 90001)]
        [TestCase("TTT10;89999;30000;14000;20101006213456789", 89999)]
        public void StringToTransponderData_addStringaToGetXKoor_b(string a, int b)
        {
            //arrange
            var fakeReceiver = Substitute.For<ITransponderReceiver>();
            var fakeFlightFilter = Substitute.For<IFlightFilter>();
            var fakeDataFormatter = new DataFormatter(fakeReceiver,fakeFlightFilter);

            

            fakeReceiver.TransponderDataReady += fakeDataFormatter.StringToTransponderData;
            List<string> flightList = new List<string>();
            TransponderArgs args = new TransponderArgs();
            TransponderArgs receiveArgs = null;

            //act
            //flightList.Add("TTT10;20000;30000;14000;20101006213456789");
            flightList.Add(a);

            fakeReceiver.TransponderDataReady += Raise.EventWith(this, new RawTransponderDataEventArgs(flightList));
            //fakeDataFormatter.transponderChanged += Raise.EventWith(this, new TransponderArgs());
            //Thread.Sleep(1000);
            
            //assert
            //fakeDataFormatter.transponderChanged += fakeFlightFilter.FilterFlight;
            fakeDataFormatter.StringToTransponderData(this,new RawTransponderDataEventArgs(flightList));
            
            Assert.That(fakeDataFormatter.transponderList_[0].X, Is.EqualTo(b));

            //fakeFlightFilter.FilterFlight(Arg.Any<object>(), Arg.Is<TransponderArgs>(arg => arg.transponderData[0].X.Equals(20000)));
        }

        #endregion

        #region Log
        #endregion

        #region flightFilter
        #endregion

        #region collisionDetection
        #endregion


    }
}

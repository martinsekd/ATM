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

        private double x = 60000;
        private double y = 60000;
        [TestCase(0, 0, 3, 4,1, 5)]
        [TestCase(0, 0, 100, 100, 1, 141.42)]
        [TestCase(0, 0, 100, 100, 2, 70.71)]
        [TestCase(0, 0, -100, -100, 1, 141.42)]
        [TestCase(0, 0, -100, -100, 2, 70.71)]
        [TestCase(0, 0, 0, 0, 2, 0)]
        [TestCase(0, 0, 30000, 30000, 1, 42426.40)]
        [TestCase(0, 0, 60000, 60000, 1, 84852.81)]
        public void CalculateSpeed_add2TransponderData_c(int x1, int y1, int x2, int y2, int time, double c)
        {
            TransponderData t1 = new TransponderData("TEST", x1, y1, 100, new DateTime(2019, 10, 20, 10, 10, 0, 0));
            var t2 = new TransponderData("TEST1", x2, y2, 100, new DateTime(2019,10,20,10,10,time,0));

            double degree = uut.CalculateSpeed(t1, t2);

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
            var stubReceiver = Substitute.For<ITransponderReceiver>();
            var mockDataFormatter = Substitute.For<IDataFormatter>();
            var uutDataFormatter = new DataFormatter(stubReceiver);
            
            

            List<string> flightList = new List<string>();
            flightList.Add("TTT10;20000;30000;14000;20101006213456789");

            //act
            stubReceiver.TransponderDataReady += mockDataFormatter.StringToTransponderData;
            stubReceiver.TransponderDataReady += Raise.EventWith(this,new RawTransponderDataEventArgs(flightList));

            //assert
            mockDataFormatter.Received(1).StringToTransponderData(Arg.Any<object>(),Arg.Is<RawTransponderDataEventArgs>(arg => arg.TransponderData.Contains("TTT10;20000;30000;14000;20101006213456789")));
        }

        [Test]
        public void test5()
        {
            //arrange
            var stubReceiver = Substitute.For<ITransponderReceiver>();
            var stubFlightFilter = Substitute.For<IFlightFilter>();
            var uutDataFormatter = new DataFormatter(stubReceiver,stubFlightFilter);
            var mockFlightFilter = Substitute.For<FlightFilter>(uutDataFormatter);


            //var fakeDataFormatter = new DataFormatter(stubReceiver, MockFlightFilter);

            List<TransponderData> transponderList = new List<TransponderData>();
            TransponderData td = new TransponderData("TTT10",20000,30000,14000,new DateTime(2010,10,6,21,34,56,789));
            transponderList.Add(td);

            //act
            uutDataFormatter.transponderChanged += mockFlightFilter.FilterFlight;

            uutDataFormatter.transponderChanged += Raise.EventWith(this,new TransponderArgs{transponderData = transponderList});

            //assert

            mockFlightFilter.Received(1).FilterFlight(Arg.Any<object>(), Arg.Is<TransponderArgs>(arg => arg.transponderData.Contains(td)));
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
            var stubReceiver = Substitute.For<ITransponderReceiver>();
            var stubFlightFilter = Substitute.For<IFlightFilter>();
            var fakeDataFormatter = new DataFormatter(stubReceiver,stubFlightFilter);

            stubReceiver.TransponderDataReady += fakeDataFormatter.StringToTransponderData;
            List<string> flightList = new List<string>();
            flightList.Add(a);

            //act
            stubReceiver.TransponderDataReady += Raise.EventWith(this, new RawTransponderDataEventArgs(flightList));
            fakeDataFormatter.StringToTransponderData(this,new RawTransponderDataEventArgs(flightList));
            
            //assert
            Assert.That(fakeDataFormatter.transponderList_[0].X, Is.EqualTo(b));
            

            //fakeFlightFilter.FilterFlight(Arg.Any<object>(), Arg.Is<TransponderArgs>(arg => arg.transponderData[0].X.Equals(20000)));
        }

        #endregion

        #region Log
        #endregion

        #region flightFilter

        [Test]
        public void transponderFilterChanged_raiseEvent_FilterFlightCalled()
        {
            var stubCalculator = Substitute.For<IFlightCalculator>();
            var mockFlightFilter = Substitute.For<FlightFilter>();
            var stubDataFormatter = Substitute.For<IDataFormatter>();

            IFlightCollection uutFlightCollection = new FlightCollection(stubCalculator,mockFlightFilter);

            mockFlightFilter.transponderFilterChanged += Raise.EventWith(this, new TransponderArgs());

            //stubDataFormatter.transponderChanged += Raise.EventWith(this, new TransponderArgs());




        }
        #endregion

        #region collisionDetection
        #endregion


    }
}

using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ATM.System;
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
        [TestCase(10,40,70,-100,156.80)]
        [TestCase(10, 40, 10, 80, 0)]
        [TestCase(10, 40, 10, 10, 180)]
        [TestCase(10, 40, 10, 40, 0)]
        [TestCase(10, 100, 11, 10, 179.36)]
        [TestCase(10, 100, 11, 200, 0.57)]
        [TestCase(0, 0, -1, -100, 180.57)]
        public void CalculateDirection_add2TransponderData_c (int x1, int y1, int x2, int y2, double c)
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
            fakeDataFormatter = new DataFormatter(fakeReceiver);
            List<string> flightList = new List<string>();
            TransponderArgs args = new TransponderArgs { };

            //act
            flightList.Add("TTT10;30;50;14000;20101006213456789");
            fakeDataFormatter.transponderChanged += (sender,arg) => args = arg;
            fakeReceiver.TransponderDataReady += Raise.EventWith(new RawTransponderDataEventArgs(flightList));


            //assert
            
            Assert.That(actual: args.transponderData[0].X, expression: Is.EqualTo(30));
        }

        public 
        #endregion

        #region Log
        #endregion

        #region flightFilter
        #endregion

        #region collisionDetection
        #endregion
    }

    internal class TestTransponderReceiver : TransponderReceiver.ITransponderReceiver
    {
        public event EventHandler<RawTransponderDataEventArgs> TransponderDataReady;

        public void RaiseEvent(List<string> flightList)
        {
            TransponderDataReady?.Invoke(this, new RawTransponderDataEventArgs(flightList));
        }
    }
}

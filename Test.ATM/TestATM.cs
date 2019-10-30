using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ATM;
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


        #region flightCalculator

        public class FlightCalculatorUnitTest
        {
            [SetUp]
            public void SetUp()
            {

            }
        }
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
        public void CalculateSpeed_add2TransponderData_c(int x1, int y1, int x2, int y2, int time, double c)
        {
            TransponderData t1 = new TransponderData("TEST", x1, y1, 100, new DateTime(2019, 10, 20, 10, 10, 0, 0));
            var t2 = new TransponderData("TEST1", x2, y2, 100, new DateTime(2019,10,20,10,10,time,0));

            double degree = uut.CalculateSpeed(t1, t2);

            Assert.That(degree, Is.InRange(c, c + 0.01));


        }

        #endregion

        #region flightCollection
        public class FlightCollectionUnitTest
        {
            [SetUp]
            public void SetUp()
            {

            }
        }
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
        public class RenderUnitTest
        {
            [SetUp]
            public void SetUp()
            {

            }
        }
        #endregion

        #region DataFormatter
        public class DataFormatterUnitTest
        {
            [SetUp]
            public void SetUp()
            {

            }
        }

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
        [TestFixture]
        public class LogUnitTest
        {
            private Log uut;
            private ICollisionDetector _fakeCollisionDetector;
            private CollisionArgs _receivedargs;
            private int numberOfLogEvents;

            [SetUp]
            public void SetUp()
            {
                _fakeCollisionDetector = Substitute.For<ICollisionDetector>();
                uut = new Log(_fakeCollisionDetector);

                _receivedargs = null;
                numberOfLogEvents = 0;
            }

            [TearDown]
            public void TearDown()
            {
                if (File.Exists(Log.LogFile))
                {
                    File.Delete(Log.LogFile);
                }
            }

            [Test]
            public void LoggerPrintsToLastLineOfLogFile()
            {
                Flight flightA = new Flight(new TransponderData("ABC123", 0, 0, 0, DateTime.Now));
                Flight flightB = new Flight(new TransponderData("CAT234", 0, 0, 0, DateTime.Now));
                Collision newCollision = new Collision(flightA,flightB);

                _fakeCollisionDetector.NewCollision += Raise.EventWith(this,
                    new CollisionArgs(new Collision(flightA, flightB)));

                string lastLineInLog = File.ReadLines(Log.LogFile).Last();
                string expectedText = uut.CollisionToLogString(newCollision);

                Assert.That(lastLineInLog.Equals(expectedText));
            }

            [Test]
            public void LoggerCreatesLogFile()
            {
                Flight flightA = new Flight(new TransponderData("ABC123", 0, 0, 0, DateTime.Now));
                Flight flightB = new Flight(new TransponderData("CAT234", 0, 0, 0, DateTime.Now));
                Collision newCollision = new Collision(flightA, flightB);

                _fakeCollisionDetector.NewCollision += Raise.EventWith(this,
                    new CollisionArgs(new Collision(flightA, flightB)));

                Assert.That(File.Exists(Log.LogFile));
            }

        }
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
        public class FlightFilterUnitTest
        {
            [SetUp]
            public void SetUp()
            {

            }
        }
        #endregion

        #region collisionDetector
        [TestFixture]
        public class CollisionDetectorUnitTest
        {
            private CollisionDetector uut;
            private IFlightCollection _fakeFlightCollection;
            private CollisionArgs receivedArgs;
            private int numberOfCollisionEvents;

            [SetUp]
            public void SetUp()
            {

                _fakeFlightCollection = Substitute.For<IFlightCollection>();
                uut = new CollisionDetector(_fakeFlightCollection, new CollisionCollection());

                receivedArgs = null;
                numberOfCollisionEvents = 0;

                uut.NewCollision += (s, a) =>
                {
                    receivedArgs = a;
                    numberOfCollisionEvents++;
                };

            }

            [Test]
            public void CollisionEmits()
            {
                List<Flight> testFlights = new List<Flight>
            {
                new Flight(new TransponderData("ABC123", 4900, 5000, 2000, DateTime.Now)),
                new Flight(new TransponderData("BOB123", 5100, 5000, 2100, DateTime.Now))
            };

                _fakeFlightCollection.flightsChanged += Raise.EventWith(this, new FlightArgs() { flights = testFlights });

                Assert.That(numberOfCollisionEvents, Is.EqualTo(1));
            }

            [Test]
            public void CollisionEmitsOnlyOnce()
            {
                List<Flight> testFlights = new List<Flight>
            {
                new Flight(new TransponderData("ABC123", 4900, 5000, 2000, DateTime.Now)),
                new Flight(new TransponderData("BOB123", 5100, 5000, 2100, DateTime.Now))
            };

                _fakeFlightCollection.flightsChanged += Raise.EventWith(this, new FlightArgs() { flights = testFlights });
                _fakeFlightCollection.flightsChanged += Raise.EventWith(this, new FlightArgs() { flights = testFlights });

                Assert.That(numberOfCollisionEvents, Is.EqualTo(1));

            }

            [Test]
            public void PassingFlightsEmitOnlyOnce()
            {

                List<Flight> testFlights = new List<Flight>
            {
                new Flight(new TransponderData("ABC123", 4900, 5000, 2000, DateTime.Now)),
                new Flight(new TransponderData("BOB123", 5100, 5000, 2100, DateTime.Now))
            };

                _fakeFlightCollection.flightsChanged += Raise.EventWith(this, new FlightArgs() { flights = testFlights });

                testFlights.Clear();
                testFlights.Add(new Flight(new TransponderData("ABC123", 5100, 5000, 2000, DateTime.Now)));
                testFlights.Add(new Flight(new TransponderData("BOB123", 4900, 5000, 2100, DateTime.Now)));

                _fakeFlightCollection.flightsChanged += Raise.EventWith(this, new FlightArgs() { flights = testFlights });

                Assert.That(numberOfCollisionEvents, Is.EqualTo(1));
            }

            [Test]
            public void ThreeFlightsInCollisionEmitsThrice()
            {
                List<Flight> testFlights = new List<Flight>
            {
                new Flight(new TransponderData("ABC123", 4900, 5000, 2000, DateTime.Now)),
                new Flight(new TransponderData("BOB123", 5100, 5000, 2100, DateTime.Now)),
                new Flight(new TransponderData("KAT666", 5000, 5000, 2050, DateTime.Now))
            };

                _fakeFlightCollection.flightsChanged += Raise.EventWith(this, new FlightArgs() { flights = testFlights });

                Assert.That(numberOfCollisionEvents, Is.EqualTo(3));
            }

            [Test]
            public void PreviouslyInCollisionNoLongerInCollision()
            {
                List<Flight> testFlights = new List<Flight>
            {
                new Flight(new TransponderData("ABC123", 4900, 5000, 2000, DateTime.Now)),
                new Flight(new TransponderData("BOB123", 5100, 5000, 2100, DateTime.Now))
            };


                _fakeFlightCollection.flightsChanged += Raise.EventWith(this, new FlightArgs() { flights = testFlights });

                Collision oldCollision = receivedArgs.Collision;

                testFlights.Clear();
                testFlights.Add(new Flight(new TransponderData("ABC123", 2000, 5000, 2000, DateTime.Now)));
                testFlights.Add(new Flight(new TransponderData("BOB123", 18000, 5000, 2100, DateTime.Now)));

                _fakeFlightCollection.flightsChanged += Raise.EventWith(this, new FlightArgs() { flights = testFlights });

                Assert.That(uut.Collisions, Does.Not.Contain(oldCollision));

            }

            [Test]
            public void NoCollisions()
            {
                List<Flight> testFlights = new List<Flight>
            {
                new Flight(new TransponderData("ABC123", 10000, 45000, 8000, DateTime.Now)),
                new Flight(new TransponderData("BOB123", 90000, 45000, 6000, DateTime.Now)),
                new Flight(new TransponderData("BOB123", 45000, 10000, 4000, DateTime.Now)),
                new Flight(new TransponderData("BOB123", 45000, 90000, 2000, DateTime.Now))
            };

                _fakeFlightCollection.flightsChanged += Raise.EventWith(this, new FlightArgs() { flights = testFlights });

                Assert.That(numberOfCollisionEvents, Is.EqualTo(0));
                Assert.That(uut.Collisions, Is.Empty);
            }

            [Test]
            public void NoCollisionsEdgeCases()
            {
                List<Flight> testFlights = new List<Flight>
            {
                new Flight(new TransponderData("ABC123", 5000, 45000, 4000, DateTime.Now)),
                new Flight(new TransponderData("BOB123", 10000, 45000, 4000, DateTime.Now)),
                new Flight(new TransponderData("BOB123", 10000, 50000, 4000, DateTime.Now)),
                new Flight(new TransponderData("BOB123", 10000, 50000,4300, DateTime.Now))
            };

                _fakeFlightCollection.flightsChanged += Raise.EventWith(this, new FlightArgs() { flights = testFlights });

                Assert.That(numberOfCollisionEvents, Is.EqualTo(0));
                Assert.That(uut.Collisions, Is.Empty);
            }
        }
        #endregion


    }
}

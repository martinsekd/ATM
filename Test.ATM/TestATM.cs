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
            var fakeReceiver = Substitute.For<ITransponderReceiver>();
            fakeDataFormatter = new DataFormatter(fakeReceiver);
            List<string> flightList = new List<string>();
            TransponderArgs args = new TransponderArgs();

            //act
            flightList.Add("TTT10;30;50;14000;20101006213456789");

            fakeReceiver.TransponderDataReady += Raise.EventWith(new RawTransponderDataEventArgs(flightList));

            fakeDataFormatter.transponderChanged += (sender, arg) => {
                args.transponderData = arg.transponderData;
            };
            


            //assert

            Assert.That(args.transponderData[0].X, expression: Is.EqualTo(30));
        }


        #endregion

        #region Log
        #endregion

        #region flightFilter
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


        /*internal class TestTransponderReceiver : ITransponderReceiver
            {
                public event EventHandler<RawTransponderDataEventArgs> TransponderDataReady;

                public void RaiseEvent(List<string> flightList)
                {
                    TransponderDataReady?.Invoke(this, new RawTransponderDataEventArgs(flightList));
                }
            }*/

    }
}

using System;
using System.Collections.Generic;
using System.Diagnostics;
using ATM.System;
using NSubstitute;
using NSubstitute.Exceptions;
using NUnit.Framework;

namespace Test.ATM
{
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

            _fakeFlightCollection.flightsChanged += Raise.EventWith(this, new FlightArgs() {flights = testFlights});

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
            Assert.That(uut.Collisions,Is.Empty);
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
}
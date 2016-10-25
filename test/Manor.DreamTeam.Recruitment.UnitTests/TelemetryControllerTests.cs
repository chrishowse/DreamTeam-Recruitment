using Manor.DreamTeam.Recruitment.Controllers;
using Manor.DreamTeam.Recruitment.Domain;
using Manor.DreamTeam.Recruitment.Interfaces;
using Manor.DreamTeam.Recruitment.UnitOfWork;
using System;
using System.Linq;
using Xunit;

namespace Manor.DreamTeam.Recruitment.UnitTests
{
    public class TelemetryControllerTests
    {
        private TelemetryController _controller;
        private IRepository<Telemetry> _repo;

        public TelemetryControllerTests()
        {
            var context = new TelemetryJSONContext();
            _repo = new TelemetryRepository(context);
            _controller = new TelemetryController(_repo);
        }

        #region #Story MR-001 Tests - Implement Controller Methods

        [Fact]
        public void TelemetryController_GetAll()
        {
            const int expectedCount = 129;

            var entities = _controller.Get();

            Assert.NotNull(entities);
            Assert.Equal(expectedCount, entities.Count());
        }

        [Fact]
        public void TelemetryController_GetByChassis_CH1()
        {
            const int expectedCH1ChassisCount = 66;

            var entities = _controller.GetByChassis("CH1");

            Assert.NotNull(entities);
            Assert.Equal(expectedCH1ChassisCount, entities.Count());
        }

        [Fact]
        public void TelemetryController_GetByChassis_CH2()
        {
            const int expectedCH2ChassisCount = 63;

            var entities = _controller.GetByChassis("CH2");

            Assert.NotNull(entities);
            Assert.Equal(expectedCH2ChassisCount, entities.Count());
        }

        [Fact]
        public void TelemetryController_GetByLap_1()
        {
            const int expectedTelemetryCount = 2;

            var entities = _controller.GetByLap(1);

            Assert.NotNull(entities);
            Assert.Equal(expectedTelemetryCount, entities.Count());
        }

        [Fact]
        public void TelemetryController_GetfastestLapRecorded()
        {
            const int expectedFastestLap = 58;
            const string expectedFastestLapTime = "01:20.3";

            var entity = _controller.GetFastestLap();

            Assert.NotNull(entity);
            Assert.Equal(expectedFastestLap, entity.Lap.Number);
            Assert.Equal(expectedFastestLapTime, entity.Lap.Time);
        }

        #endregion #Story MR-001 Tests - Implement Controller Methods

        #region #Story MR-002 Tests - Method for accepting new lap to the collection

        [Fact]
        public void TelemetryController_Post_New()
        {
            int existingCount = _controller.Get().Count();

            var telemetry = new Telemetry
            {
                Car = new Car { Chassis = "CH1", Fuel = 98.58 },
                Lap = new Lap { Number = 67, Time = "01:22.1" }
            };

            _controller.Post(telemetry);

            int newCount = _controller.Get().Count();
            
            Assert.Equal(existingCount+1, newCount);
        }

        [Fact]
        public void TelemetryController_Post_Existing()
        {
            int existingCount = _controller.Get().Count();

            var telemetry = new Telemetry
            {
                Car = new Car { Chassis = "CH1", Fuel = 98.58 },
                Lap = new Lap { Number = 66, Time = "01:22.1" } // Entry exists for this Lap
            };

            try
            {
                _controller.Post(telemetry);
            }
            catch(Exception ex)
            {
                Assert.NotNull(ex);

                // Check nothing was added
                int newCount = _controller.Get().Count();
                Assert.Equal(existingCount, newCount);
                return;
            }

            Assert.True(false, "Error, this should not be reached");
        }

        #endregion #Story MR-002 Tests - Method for accepting new lap to the collection
    }
}

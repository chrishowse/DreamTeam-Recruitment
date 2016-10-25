using Manor.DreamTeam.Recruitment.Domain;
using Manor.DreamTeam.Recruitment.Interfaces;
using Manor.DreamTeam.Recruitment.UnitOfWork;
using System;
using System.Linq;
using Xunit;

namespace Manor.DreamTeam.Recruitment.UnitTests
{
    public class TelemetryRepositoryTests
    {
        private IRepository<Telemetry> _repo;

        public TelemetryRepositoryTests()
        {
            var context = new TelemetryJSONContext();
            _repo = new TelemetryRepository(context);
        }

        #region #Story MR-001 Tests - Implement IRepository

        [Fact]
        public void Get()
        {
            const int expectedEntityCount = 129;

            // Get All Entities
            var entities = _repo.Get();
            Assert.NotNull(entities);
            Assert.Equal(expectedEntityCount, entities.Count());
        }

        [Fact]
        public void GetById_CH1_First()
        {
            string id = string.Format("{0}_{1}", "CH1", "17/09/2016 13:02:55");

            // Get Existing Entity
            Telemetry entity = _repo.GetById(id);
            Assert.NotNull(entity);
            Assert.NotNull(entity.Car);
            
            Assert.Equal("CH1", entity.Car.Chassis);
        }

        [Fact]
        public void Create()
        {
            // Get Existing Count
            var existingCount = _repo.Get().Count();

            // Create new Entity
            var entity = new Telemetry();
            entity.TimeStamp = DateTime.Now;
            entity.Car = new Car { Chassis = "CH1" };
            entity.Lap = new Lap { Number = 68 }; // new lap
            _repo.Create(entity);

            // Get New Count
            var newCount = _repo.Get().Count();
            
            // Check new count has added the new entity
            Assert.Equal(existingCount + 1, newCount);
        }

        [Fact]
        public void Update_CH1_First()
        {
            const double existingWeight = 842.59;
            const double newWeight = 843.00;

            string id = string.Format("{0}_{1}", "CH1", "17/09/2016 13:02:55");

            // Get existing Entity
            Telemetry entity = _repo.GetById(id);

            // Check existing Weight
            Assert.Equal(existingWeight, entity.Car.Weight);

            // Change Weight
            entity.Car.Weight = newWeight;

            // Update Entity
            _repo.Update(id, entity);

            // Get updated Entity
            Telemetry updatedEntity = _repo.GetById(id);

            // Check updated Weight
            Assert.Equal(newWeight, entity.Car.Weight);
        }

        [Fact]
        public void Delete()
        {
            // Get Existing Count
            var existingCount = _repo.Get().Count();

            string id = string.Format("{0}_{1}", "CH1", "17/09/2016 13:02:55");

            // Get existing Entity
            Telemetry entity = _repo.GetById(id);

            // Delete Entity
            _repo.Delete(id);

            // Get New Count
            var newCount = _repo.Get().Count();

            // Check new count has removed the deleted entity
            Assert.Equal(existingCount-1, newCount);
        }

        #endregion #Story MR-001 Tests - Implement IRepository
    }
}

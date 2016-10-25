using Manor.DreamTeam.Recruitment.Interfaces;
using Manor.DreamTeam.Recruitment.Domain;
using System;
using System.Linq;

namespace Manor.DreamTeam.Recruitment.UnitOfWork
{
    public class TelemetryRepository : IRepository<Telemetry>
    {
        private ITelemetryContext _context;

        public TelemetryRepository(ITelemetryContext context)
        {
            this._context = context;
        }

        public void Create(Telemetry entity)
        {
            // Check entry has not already been made for this Chassis on this lap
            var existingEntity = Get().Where(t => t.Car.Chassis.Equals(entity.Car.Chassis) &&
                                             t.Lap.Number.Equals(entity.Lap.Number));

            if (existingEntity.Count() > 0)
                throw new Exception(string.Format("Telemetry already added for chassis {0} on lap {1}", entity.Car.Chassis, entity.Lap.Number));

            _context.List.Add(entity);
        }

        public void Delete(IComparable id)
        {
            var entity = GetById(id);
            _context.List.Remove(entity);
        }

        public IQueryable<Telemetry> Get()
        {
            IQueryable<Telemetry> telemetryQueryable = _context.List.AsQueryable();
            return telemetryQueryable;
        }

        public Telemetry GetById(IComparable id)
        {
            IQueryable<Telemetry> telemetryQueryable = _context.List.AsQueryable();
            return telemetryQueryable.FirstOrDefault(t => t.Id.Equals(id));
        }

        public void Update(IComparable id, Telemetry entity)
        {
            var existingEntity = GetById(id);

            existingEntity.Car = entity.Car;
            existingEntity.Lap = entity.Lap;
            existingEntity.TimeStamp = entity.TimeStamp;

            Delete(id);
            Create(existingEntity);
        }
    }
}

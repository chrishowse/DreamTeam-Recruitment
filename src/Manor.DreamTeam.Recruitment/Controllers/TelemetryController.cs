using System.Collections.Generic;
using Manor.DreamTeam.Recruitment.Domain;
using Manor.DreamTeam.Recruitment.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace Manor.DreamTeam.Recruitment.Controllers
{
    [Route("api/[controller]")]
    public class TelemetryController : Controller
    {
        private readonly IRepository<Telemetry> _telemetryRepo;

        public TelemetryController(IRepository<Telemetry> telemetryRepo)
        {
            _telemetryRepo = telemetryRepo;
        }

        // GET: api/telemetry
        [HttpGet]
        public IEnumerable<Telemetry> Get()
        {
            IQueryable<Telemetry> queryable = _telemetryRepo.Get();
            return queryable;
        }

        // GET api/telemetry/chassis/CH1
        [HttpGet("chassis/{chassis}")]
        public IEnumerable<Telemetry> GetByChassis(string chassis)
        {
            IQueryable<Telemetry> queryable = _telemetryRepo.Get();
            return queryable.Where(t => t.Car.Chassis.Equals(chassis));
        }

        // GET api/telemetry/lap/23
        [HttpGet("lap/{lap}")]
        public IEnumerable<Telemetry> GetByLap(int lap)
        {
            IQueryable<Telemetry> queryable = _telemetryRepo.Get();
            return queryable.Where(t => t.Lap.Number.Equals(lap));
        }

        // GET api/telemetry/fastestlap
        [HttpGet("fastestlap")]
        public Telemetry GetFastestLap()
        {
            IQueryable<Telemetry> queryable = _telemetryRepo.Get();
            var result = queryable.OrderBy(t => t.Lap.Time).First();

            return result;
        }

        // POST api/telemetry
        [HttpPost]
        public void Post(Telemetry telemetry)
        {
            _telemetryRepo.Create(telemetry);
        }
    }
}

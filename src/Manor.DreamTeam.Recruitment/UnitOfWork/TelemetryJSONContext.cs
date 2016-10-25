using Manor.DreamTeam.Recruitment.Domain;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;

namespace Manor.DreamTeam.Recruitment.UnitOfWork
{
    public class TelemetryJSONContext: ITelemetryContext
    {
        private IList<Telemetry> _telemetryList;

        public TelemetryJSONContext()
        {
            Initialise();
        }

        public IList<Telemetry> List
        {
            get { return _telemetryList; }
        }

        public void Initialise()
        {
            InitaliseTelemetryFromJSON();
            InitaliseIdValues();
        }

        private void InitaliseTelemetryFromJSON()
        {
            // Initialise by reading telemetry.json
            using (StreamReader file = File.OpenText(@"..\..\data\telemetry.json"))
            {
                string json = file.ReadToEnd();
                _telemetryList = JsonConvert.DeserializeObject<IList<Telemetry>>(json);
            }
        }

        private void InitaliseIdValues()
        {
            foreach (var item in _telemetryList)
            {
                item.Id = string.Format("{0}_{1}", item.Car.Chassis, item.TimeStamp);
            }
        }
    }
}
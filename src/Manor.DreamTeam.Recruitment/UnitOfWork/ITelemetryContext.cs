using Manor.DreamTeam.Recruitment.Domain;
using System.Collections.Generic;

namespace Manor.DreamTeam.Recruitment.UnitOfWork
{
    public interface ITelemetryContext
    {
        IList<Telemetry> List { get; }

        void Initialise();
    }
}
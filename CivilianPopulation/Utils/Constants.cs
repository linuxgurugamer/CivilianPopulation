
//namespace CivilianPopulation.Utils
namespace CivilianPopulation
{
    internal class Constants
    {
        //internal const double DAY_IN_SECONDS = 60 * 60 * 6;
        //internal const double MISSION_DURATION = DAY_IN_SECONDS * 85;

        internal const string INSPIRATION = "inspiration";
        internal const string EDU = "education";
        internal const string FLIGHTEXP = "flightExperience";

        static internal bool ValidVessel(Vessel vessel)
        {
            if (vessel != null)
            {
                if (vessel.vesselType == VesselType.Debris ||
                      vessel.vesselType == VesselType.SpaceObject ||
                      vessel.vesselType == VesselType.EVA ||
                      vessel.vesselType == VesselType.Flag ||
                      vessel.vesselType == VesselType.DeployedScienceController ||
                      vessel.vesselType == VesselType.DeployedSciencePart ||
                      vessel.vesselType == VesselType.Unknown
                      )
                    return false;
                return true;
            }
            return false;
        }
    }
}

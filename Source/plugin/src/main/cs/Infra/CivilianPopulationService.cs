using CivilianPopulation.Domain.Repository;
using UnityEngine;

namespace CivilianPopulation.Infra
{
    public class CivilianPopulationService
    {

        public void CreateKerbals(CivPopRepository repo, Vessel vessel)
        {
            foreach (var current in repo.GetLivingRosterForVessel(vessel.id.ToString()))
            {
                var crew = vessel.GetVesselCrew().Find(c => c.name.Equals(current.GetName()));
                if (crew == null)
                {
                    var houses = vessel.FindPartModulesImplementing<CivilianPopulationHousingModule>();
                    if (houses.Count > 0)
                    {
                        foreach (var house in houses)
                        {
                            if (house.part.CrewCapacity > house.part.protoModuleCrew.Count)
                            {
                                var kspRoster = HighLogic.CurrentGame.CrewRoster;
                                var newKerbal = kspRoster.GetNewKerbal(ProtoCrewMember.KerbalType.Crew);

                                var gender = ProtoCrewMember.Gender.Male;
                                if (current.GetGender().Equals(CivPopKerbalGender.FEMALE))
                                {
                                    gender = ProtoCrewMember.Gender.Female;
                                }

                                while (newKerbal.gender != gender || newKerbal.trait != "Civilian")
                                {
                                    kspRoster.Remove(newKerbal);
                                    newKerbal = kspRoster.GetNewKerbal(ProtoCrewMember.KerbalType.Crew);
                                }
                                newKerbal.ChangeName(current.GetName());

                                if (house.part.AddCrewmember(newKerbal))
                                {
                                    vessel.SpawnCrew();
                                    log("CreateKerbals : " + newKerbal.name + " has been placed successfully");
                                    break;
                                }
                            }
                        }
                    }
                }
                crew = vessel.GetVesselCrew().Find(c => c.name.Equals(current.GetName()));
                if (crew == null)
                {
                    current.SetDead(true);
                    log("CreateKerbals : " + current.GetName() + " died because of a lack of room");
                }
            }
        }

        public void KillKerbals(CivPopRepository repo, Vessel vessel)
        {
            foreach (var current in repo.GetDeadRosterForVessel(vessel.id.ToString()))
            {
                foreach (var part in vessel.parts)
                {
                    foreach (var crew in part.protoModuleCrew)
                    {
                        if (crew.name.Equals(current.GetName()))
                        {
                            part.RemoveCrewmember(crew);
                            vessel.RemoveCrew(crew);
                            crew.Die();
                        }
                    }
                }
                repo.Remove(current);
            }
        }

        private void log(string message)
        {
            Debug.Log(this.GetType().Name + " - " + message);
        }

    }
}
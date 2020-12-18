using CivilianPopulation.Domain.Repository;
using UnityEngine;

namespace CivilianPopulation.Infra
{
    public class CivilianPopulationService
    {

        public void CreateKerbals(CivPopRepository repo, Vessel vessel)
        {
            foreach (CivPopKerbal current in repo.GetLivingRosterForVessel(vessel.id.ToString()))
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
                                if (!newKerbal.ChangeName(current.GetName()))
                                {
                                    kspRoster.Remove(newKerbal);
                                    continue;
                                }
                                if (house.part.AddCrewmember(newKerbal))
                                {
                                    vessel.SpawnCrew();
                                    Log.Info("CreateKerbals : " + newKerbal.name + " has been placed successfully");
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
                    Log.Info("CreateKerbals : " + current.GetName() + " died because of a lack of room");
                }
            }
        }

        public void KillKerbals(CivPopRepository repo, Vessel vessel)
        {
            var deadList = repo.GetDeadRosterForVessel(vessel.id.ToString());

            for (int i = deadList.Count - 1; i > 0; i--)
            {
                var current = deadList[i];
                for (int p = vessel.parts.Count - 1; p > 0; p--)
                {
                    var part = vessel.parts[p];
                    for (int c = part.protoModuleCrew.Count - 1; c >0; c--)
                    {
                        var crew = part.protoModuleCrew[c];
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

#if false
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
#endif
        }

#if false
        private void log(string message)
        {
            Debug.Log(this.GetType().Name + " - " + message);
        }
#endif
    }
}
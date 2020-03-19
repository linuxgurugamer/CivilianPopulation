using CivilianPopulation.Domain;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace CivilianPopulation.Infra
{
    public class CivilianPopulationRecruitmentModule : PartModule
    {
        [KSPField(isPersistant = true, guiActive = false)]
        public string recruitmentTraits;

        int flightExperienceID, educationID, inspirationID;
        public void Start()
        {
            if (String.IsNullOrEmpty(recruitmentTraits))
            {
                recruitmentTraits = "Pilot,Engineer,Scientist";
            }
            flightExperienceID = PartResourceLibrary.Instance.GetDefinition(Constants.FLIGHTEXP).id;
            educationID = PartResourceLibrary.Instance.GetDefinition(Constants.EDU).id;
            inspirationID = PartResourceLibrary.Instance.GetDefinition(Constants.INSPIRATION).id;

        }

        public void Update()
        {
            // log(" - Update !");
        }

        public void FixedUpdate()
        {
            // log(" - FixedUpdate !");
        }

        [KSPEvent(guiName = "Recruit", active = true, guiActive = true)]
        public void recruit()
        {
            double inspirationRes, educationRes, flightExperienceRes, maxAmount;

            vessel.resourcePartSet.GetConnectedResourceTotals(inspirationID, out inspirationRes, out maxAmount, true);
            if (inspirationRes < HighLogic.CurrentGame.Parameters.CustomParams<CP>().inspirationCost)
                return;

           // repo.GetLivingRosterForVessel(vessel.GetId())

            CivilianPopulationRecruitmentService service = new CivilianPopulationRecruitmentService(recruitmentTraits);
            foreach (ProtoCrewMember crewMember in part.protoModuleCrew)
            {

                if (crewMember.trait == "Civilian")
                {
                    int engineer, pilot, scientist;

                    applyTheaterBonus(out engineer, out pilot, out scientist);

                    vessel.resourcePartSet.GetConnectedResourceTotals(educationID, out educationRes, out maxAmount, true);
                    vessel.resourcePartSet.GetConnectedResourceTotals(flightExperienceID, out flightExperienceRes, out maxAmount, true);

                    string trait = service.selectTrait(engineer, pilot, scientist, educationRes, flightExperienceRes, HighLogic.CurrentGame.Parameters.CustomParams<CP>().educationCost, HighLogic.CurrentGame.Parameters.CustomParams<CP>().flightExperienceCost);
                    if (trait != "")
                    {
                        crewMember.trait = trait;
                        log(crewMember.name + " is now a " + crewMember.trait + "!");
                        //RequestResource(Part part, int id, double demand, bool usePri)
                        //vessel.resourcePartSet.RequestResource(this.part, inspirationID, inspirationCost, true);
                        double d = this.part.RequestResource(inspirationID, HighLogic.CurrentGame.Parameters.CustomParams<CP>().inspirationCost);
                        switch (trait)
                        {
                            case "Engineer":
                                d = this.part.RequestResource(educationID, HighLogic.CurrentGame.Parameters.CustomParams<CP>().educationCost);
                                break;
                            case "Scientist":
                                d = this.part.RequestResource(educationID, HighLogic.CurrentGame.Parameters.CustomParams<CP>().educationCost);
                                break;
                            case "Pilot":
                                d = this.part.RequestResource(flightExperienceID, HighLogic.CurrentGame.Parameters.CustomParams<CP>().flightExperienceCost);
                                break;
                        }

                        vessel.resourcePartSet.GetConnectedResourceTotals(educationID, out educationRes, out maxAmount, true);
                        vessel.resourcePartSet.GetConnectedResourceTotals(flightExperienceID, out flightExperienceRes, out maxAmount, true);

                    }
                }
            }
        }

        private void applyTheaterBonus(out int engineer, out int pilot, out int scientist)
        {
            List<MovieTheater> list = this.vessel.FindPartModulesImplementing<MovieTheater>();
            Debug.Log((object)list.Count);
            engineer = 0;
            pilot = 0;
            scientist = 0;
            foreach (MovieTheater item in list)
            {
                Debug.Log((object)item.MovieType);
                if (item.MovieType == "Racing Movies")
                {
                    engineer++;
                }
                if (item.MovieType == "Scifi Movies")
                {
                    pilot++;
                }
                if (item.MovieType == "Documentaries" )
                {
                    scientist++;
                }
            }
        }

        private void log(string message)
        {
            Debug.Log(this.GetType().Name + " - " + message);
        }
    }
}

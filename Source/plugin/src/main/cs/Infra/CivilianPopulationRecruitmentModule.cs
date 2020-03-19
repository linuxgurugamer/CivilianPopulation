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

        [KSPField(isPersistant = false, guiActive = false)]
        public double flightExperienceCost = 5000;

        [KSPField(isPersistant = false, guiActive = false)]
        public double educationCost = 5000;

        [KSPField(isPersistant = false, guiActive = false)]
        public double inspirationCost = 50;


        int flightExperienceID, educationID, inspirationID;
        public void Start()
        {
            if (String.IsNullOrEmpty(recruitmentTraits))
            {
                recruitmentTraits = "Pilot,Engineer,Scientist";
            }
            flightExperienceID = PartResourceLibrary.Instance.GetDefinition("flightExperience").id;
            educationID = PartResourceLibrary.Instance.GetDefinition("education").id;
            inspirationID = PartResourceLibrary.Instance.GetDefinition("inspiration").id;

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

            SimpleLogger.fetch.Info("recruit");
            vessel.resourcePartSet.GetConnectedResourceTotals(inspirationID, out inspirationRes, out maxAmount, true);
            SimpleLogger.fetch.Info("recruit, inspirationRes: " + inspirationRes + ", inspirationCost: " + inspirationCost);
            if (inspirationRes < inspirationCost)
                return;

           // repo.GetLivingRosterForVessel(vessel.GetId())

            CivilianPopulationRecruitmentService service = new CivilianPopulationRecruitmentService(recruitmentTraits);
            foreach (ProtoCrewMember crewMember in part.protoModuleCrew)
            {
                SimpleLogger.fetch.Info("recruit, name: " + crewMember.name + ", " + crewMember.trait);

                if (crewMember.trait == "Civilian")
                {
                    int engineer, pilot, scientist;

                    SimpleLogger.fetch.Info("recruit, name: " + crewMember.name);

                    applyTheaterBonus(out engineer, out pilot, out scientist);

                    vessel.resourcePartSet.GetConnectedResourceTotals(educationID, out educationRes, out maxAmount, true);
                    vessel.resourcePartSet.GetConnectedResourceTotals(flightExperienceID, out flightExperienceRes, out maxAmount, true);

                    SimpleLogger.fetch.Info("recruit, available educationRes: " + educationRes);
                    SimpleLogger.fetch.Info("recruit, available flightExperienceRes: " + flightExperienceRes);

                    string trait = service.selectTrait(engineer, pilot, scientist, educationRes, flightExperienceRes, educationCost, flightExperienceCost);
                    if (trait != "")
                    {
                        crewMember.trait = trait;
                        log(crewMember.name + " is now a " + crewMember.trait + "!");
                        //RequestResource(Part part, int id, double demand, bool usePri)
                        //vessel.resourcePartSet.RequestResource(this.part, inspirationID, inspirationCost, true);
                        double d = this.part.RequestResource(inspirationID, inspirationCost);
                        SimpleLogger.fetch.Info("this.part.RequestResource, inspiration, returned" + d);
                        switch (trait)
                        {
                            case "Engineer":
                                d = this.part.RequestResource(educationID, educationCost);
                                SimpleLogger.fetch.Info("Engineer, this.part.RequestResource, education, returned" + d);
                                break;
                            case "Scientist":
                                d = this.part.RequestResource(educationID, educationCost);
                                SimpleLogger.fetch.Info("Scientist, this.part.RequestResource, education, returned" + d);
                                break;
                            case "Pilot":
                                d = this.part.RequestResource(flightExperienceID, flightExperienceCost);
                                SimpleLogger.fetch.Info("Pilot, this.part.RequestResource, flightExperience, returned" + d);
                                break;
                        }

                        vessel.resourcePartSet.GetConnectedResourceTotals(educationID, out educationRes, out maxAmount, true);
                        vessel.resourcePartSet.GetConnectedResourceTotals(flightExperienceID, out flightExperienceRes, out maxAmount, true);

                        SimpleLogger.fetch.Info("recruit, after, available educationRes: " + educationRes);
                        SimpleLogger.fetch.Info("recruit, after, available flightExperienceRes: " + flightExperienceRes);

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

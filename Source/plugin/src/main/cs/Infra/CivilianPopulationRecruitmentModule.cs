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

        public void Start()
        {
            if (String.IsNullOrEmpty(recruitmentTraits))
            {
                recruitmentTraits = "Pilot,Engineer,Scientist";
            }
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
            CivilianPopulationRecruitmentService service = new CivilianPopulationRecruitmentService(recruitmentTraits);
            foreach (ProtoCrewMember crewMember in part.protoModuleCrew)
            {
                if (crewMember.trait == "Civilian")
                {
                    int engineer, pilot, scientist;
                    applyTheaterBonus(out engineer, out pilot, out scientist);
                    string trait = service.selectTrait(engineer, pilot, scientist);
                    crewMember.trait = trait;
                    log(crewMember.name + " is now a " + crewMember.trait + "!");
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

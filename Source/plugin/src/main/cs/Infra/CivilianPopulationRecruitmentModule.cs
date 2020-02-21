using CivilianPopulation.Domain;
using System;
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
                    string trait = service.selectTrait();
                    crewMember.trait = trait;
                    log(crewMember.name + " is now a " + crewMember.trait + "!");
                }
            }
        }

        private void log(string message)
        {
            Debug.Log(this.GetType().Name + " - " + message);
        }
    }
}

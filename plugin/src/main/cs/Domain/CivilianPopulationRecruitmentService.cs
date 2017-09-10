using System;
namespace CivilianPopulation.Domain
{
    public class CivilianPopulationRecruitmentService
    {
        private string[] traits;
        private System.Random rng;

        public CivilianPopulationRecruitmentService(string traits)
        {
            this.traits = traits.Split(',');
			this.rng = new System.Random();
		}

        public string selectTrait()
        {
			return selectTrait(rng.Next());
        }

		public string selectTrait(int random)
		{
            return this.traits[random % traits.Length];
		}
	}
}

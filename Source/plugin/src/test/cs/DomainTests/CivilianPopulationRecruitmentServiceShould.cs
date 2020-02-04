using System;
using NUnit.Framework;

namespace CivilianPopulation.Domain
{
    public class CivilianPopulationRecruitmentServiceShould
    {
        private CivilianPopulationRecruitmentService service;

        [SetUp]
        public void SetUp()
        {
            service = new CivilianPopulationRecruitmentService("Pilot,Engineer,Scientist");
        }

		[Test()]
		public void select_pilot_trait_for_the_recruited_kerbal()
		{
            Assert.AreEqual(
                "Pilot",
                service.selectTrait(0)
            );
		}

		[Test()]
		public void select_engineer_trait_for_the_recruited_kerbal()
		{
			Assert.AreEqual(
				"Engineer",
				service.selectTrait(1)
			);
		}

		[Test()]
		public void select_scientist_trait_for_the_recruited_kerbal()
		{
			Assert.AreEqual(
				"Scientist",
				service.selectTrait(5)
			);
		}
	}
}

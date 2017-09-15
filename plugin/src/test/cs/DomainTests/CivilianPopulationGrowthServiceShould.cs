using System;
using NUnit.Framework;

namespace CivilianPopulation.Domain
{
    class CivilianPopulationGrowthServiceShould
    {
        private const double DAY_IN_SECONDS = 60 * 60 * 6;

        [SetUp]
        public void SetUp()
        {
        }

        [Test()]
        public void make_update_once_a_day()
        {
            CivilianPopulationGrowthServiceCount service = new CivilianPopulationGrowthServiceCount();

            double date = 45;
            CivilianVessel vessel = null;

            service.update(date, vessel);
            Assert.AreEqual(0, service.getNbUpdates());

            date = date + 1;
            service.update(date, vessel);
            Assert.AreEqual(0, service.getNbUpdates());

            date = date + 1;
            service.update(date, vessel);
            Assert.AreEqual(0, service.getNbUpdates());

            date = date + DAY_IN_SECONDS;
            service.update(date, vessel);
            Assert.AreEqual(1, service.getNbUpdates());

            date = date + 1;
            service.update(date, vessel);
            Assert.AreEqual(1, service.getNbUpdates());

            date = date + DAY_IN_SECONDS;
            service.update(date, vessel);
            Assert.AreEqual(2, service.getNbUpdates());
        }
    }

    class CivilianPopulationGrowthServiceCount : CivilianPopulationGrowthService
    {
        private int nbUpdates;

        public CivilianPopulationGrowthServiceCount() : base()
        {
            nbUpdates = 0;
        }

        protected override void doUpdate(CivilianVessel vessel)
        {
            nbUpdates++;
        }

        public int getNbUpdates()
        {
            return this.nbUpdates;
        }
    }
}

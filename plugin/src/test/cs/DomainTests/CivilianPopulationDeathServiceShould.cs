using System;
using CivilianPopulation.Domain;
using NUnit.Framework;

namespace CivilianPopulation.Domain
{
    public class CivilianPopulationDeathServiceShould
    {
        [Test()]
        public void calculate_chance_of_death_properly()
        {
            CivilianPopulationDeathService service = new CivilianPopulationDeathService(this.kill);
            Assert.AreEqual(0, service.getChanceOfDeath(0));
            Assert.AreEqual(0, service.getChanceOfDeath(60));
            Assert.AreEqual(0, service.getChanceOfDeath(75));
            Assert.AreEqual(TimeUnit.DAYS_PER_YEARS, service.getChanceOfDeath(76));
            Assert.AreEqual(TimeUnit.DAYS_PER_YEARS * 10, service.getChanceOfDeath(85));
        }

        public void kill(CivilianKerbal kerbal) {
            
        }
    }
}

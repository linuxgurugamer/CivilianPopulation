using System;
using NUnit.Framework;

namespace CivilianPopulation.Domain.Services
{
    public class CivPopServiceDeathShould
    {
        [Test()]
        public void calculate_chance_of_death_properly()
        {
            CivPopServiceDeath service = new CivPopServiceDeath();
            Assert.AreEqual(0, service.GetChanceOfDeath(0));
            Assert.AreEqual(0, service.GetChanceOfDeath(60));
            Assert.AreEqual(0, service.GetChanceOfDeath(75));
            Assert.AreEqual(TimeUnit.DAYS_PER_YEARS, service.GetChanceOfDeath(76));
            Assert.AreEqual(TimeUnit.DAYS_PER_YEARS * 10, service.GetChanceOfDeath(85));
        }
    }
}

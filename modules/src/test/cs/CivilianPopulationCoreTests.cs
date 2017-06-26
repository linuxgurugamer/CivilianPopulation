using NUnit.Framework;
using System;
using CivilianPopulation;

namespace CivilianPopulationShould
{
	[TestFixture()]
	public class CivilianPopulationCoreTests
    {
		[Test()]
		public void taxes_should_happen_every_6_hours()
        {
            double sixHours = 6 * 60 * 60;

            TestAdapter adapter = new TestAdapter();
            CivilianPopulationConfiguration configuration = new CivilianPopulationConfiguration();
            configuration.setRentAmountPerCivilian(200);
            configuration.setTimeBetweenRents(sixHours);

            adapter.setUniversalTime(0);
            CivilianPopulationCore core = new CivilianPopulationCore(adapter, configuration);

            adapter.setUniversalTime(1);
            double next = core.getTimeUntilTaxes();
            Assert.IsTrue(next == sixHours - 1);
        }
    }
}

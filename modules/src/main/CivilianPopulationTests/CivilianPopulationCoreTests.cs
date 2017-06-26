using Microsoft.VisualStudio.TestTools.UnitTesting;
using CivilianPopulation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CivilianPopulation.Tests
{
    [TestClass()]
    public class CivilianPopulationCoreTests
    {
        [TestMethod()]
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
            Assert.IsTrue(next == sixHours-1);
        }

}
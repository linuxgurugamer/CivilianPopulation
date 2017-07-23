using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace CivilianPopulation.Domain
{
    [TestFixture()]
    public class CivilianPopulationRentServiceShould
    {
		private const int ONE_HOUR = 60 * 60;

		[Test()]
        public void AddRentEveryDay()
        {
            FundsAccount fundsAccount = new FundsAccount();
            fundsAccount.setFunds(0);

            CivilianPopulationRentService service = new CivilianPopulationRentService(fundsAccount.addFunds);

			int time = 0;
            List<CivilianVessel> vessels = new List<CivilianVessel>();

            service.update(time, vessels);
			Assert.AreEqual(0, fundsAccount.getFunds());

            time += ONE_HOUR;
			service.update(time, vessels);
			Assert.AreEqual(0, fundsAccount.getFunds());

			time += ONE_HOUR;
            vessels.Add(new CivilianVesselBuilder().countingCivilian(2).build());
			service.update(time, vessels);
			Assert.AreEqual(0, fundsAccount.getFunds());

			time += ONE_HOUR;
			service.update(time, vessels);
            Assert.AreEqual(0, fundsAccount.getFunds());

			time += ONE_HOUR;
			service.update(time, vessels);
			Assert.AreEqual(0, fundsAccount.getFunds());

            time += ONE_HOUR;
			service.update(time, vessels);
			Assert.AreEqual(0, fundsAccount.getFunds());

			time += ONE_HOUR;
			service.update(time, vessels);
			Assert.AreEqual(400, fundsAccount.getFunds());

			time += ONE_HOUR;
			service.update(time, vessels);
			Assert.AreEqual(400, fundsAccount.getFunds());
		}

		[Test()]
        public void ComputeNextRentDate() {
			FundsAccount fundsAccount = new FundsAccount();
			fundsAccount.setFunds(0);

			CivilianPopulationRentService service = new CivilianPopulationRentService(fundsAccount.addFunds);

			Assert.AreEqual(ONE_HOUR *  6, service.getNextTaxesDate(0));
			Assert.AreEqual(ONE_HOUR *  6, service.getNextTaxesDate(ONE_HOUR * 5));
			Assert.AreEqual(ONE_HOUR * 12, service.getNextTaxesDate(ONE_HOUR * 6));

			Assert.AreEqual(123465600, service.getNextTaxesDate(123456789));
		}
	}
}

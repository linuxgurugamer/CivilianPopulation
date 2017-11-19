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
        public void AddRentEveryDayFor2Civilians()
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
            CivilianVessel vessel = new CivilianVesselBuilder().build();
            vessel.addCrew(new CivilianKerbal("male", "Civilian", true, false, -1, -1));
            vessel.addCrew(new CivilianKerbal("female", "Civilian", false, false, -1, -1));
			vessels.Add(vessel);
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
        public void AddRentEveryDayFor1Civilian()
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
            CivilianVessel vessel = new CivilianVesselBuilder().build();
            vessel.addCrew(new CivilianKerbal("male", "Pilot", true, false, -1, -1));
            vessel.addCrew(new CivilianKerbal("female", "Civilian", false, false, -1, -1));
            vessels.Add(vessel);
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
            Assert.AreEqual(200, fundsAccount.getFunds());

            time += ONE_HOUR;
            service.update(time, vessels);
            Assert.AreEqual(200, fundsAccount.getFunds());
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

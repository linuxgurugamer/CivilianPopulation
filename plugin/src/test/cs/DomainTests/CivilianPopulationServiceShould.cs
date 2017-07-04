using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace CivilianPopulation.Domain
{
    [TestFixture()]
    public class CivilianPopulationServiceShould
    {
		private const int ONE_HOUR = 60 * 60;

		[Test()]
        public void AddRentEveryDay()
        {
            FundsAccount fundsAccount = new FundsAccount();
            fundsAccount.setFunds(0);

            CivilianPopulationService service = new CivilianPopulationService(fundsAccount.addFunds);

			bool career = true;
			int time = 0;
            List<CivilianVessel> vessels = new List<CivilianVessel>();

            CivilianPopulationWorld world = new CivilianPopulationWorld(career, time, vessels);
            service.update(world);
			Assert.AreEqual(0, fundsAccount.getFunds());

            time += ONE_HOUR;
            world = new CivilianPopulationWorld(career, time, vessels);
			service.update(world);
			Assert.AreEqual(0, fundsAccount.getFunds());

			time += ONE_HOUR;
            vessels.Add(new CivilianVessel(new Guid(), "My vessel", 2, 0, 0));
			world = new CivilianPopulationWorld(career, time, vessels);
			service.update(world);
			Assert.AreEqual(0, fundsAccount.getFunds());

			time += ONE_HOUR;
			world = new CivilianPopulationWorld(career, time, vessels);
			service.update(world);
            Assert.AreEqual(0, fundsAccount.getFunds());

			time += ONE_HOUR;
			world = new CivilianPopulationWorld(career, time, vessels);
			service.update(world);
			Assert.AreEqual(0, fundsAccount.getFunds());

            time += ONE_HOUR;
			world = new CivilianPopulationWorld(career, time, vessels);
			service.update(world);
			Assert.AreEqual(0, fundsAccount.getFunds());

			time += ONE_HOUR;
			world = new CivilianPopulationWorld(career, time, vessels);
			service.update(world);
			Assert.AreEqual(0, fundsAccount.getFunds());

			time += ONE_HOUR;
			world = new CivilianPopulationWorld(career, time, vessels);
			service.update(world);
			Assert.AreEqual(400, fundsAccount.getFunds());
		}
    }
}

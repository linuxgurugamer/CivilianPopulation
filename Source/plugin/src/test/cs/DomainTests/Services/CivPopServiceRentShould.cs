using System;
using CivilianPopulation.Domain.Repository;
using NUnit.Framework;

namespace CivilianPopulation.Domain.Services
{
    [TestFixture()]
    public class CivPopServiceRentShould
    {
        [Test()]
        public void add_rent_every_day_for_2_civilians()
        {
            CivPopRepository repo = new CivPopRepository();

            repo.Add(new CivPopVessel("vessel"));

            CivPopKerbal valentina = new CivPopKerbal("Valentina", CivPopKerbalGender.FEMALE, 0, true);
            repo.Add(valentina);
            valentina.SetVesselId("vessel");

            CivPopKerbal bill = new CivPopKerbal("Bill", CivPopKerbalGender.MALE, 0, true);
            repo.Add(bill);
            bill.SetVesselId("vessel");

            CivPopServiceRent service = new CivPopServiceRent();

            double date = 0;

            service.Update(date, repo);
            Assert.AreEqual(0, repo.GetFunds());

            date += 60 * 60;
            service.Update(date, repo);
            Assert.AreEqual(0, repo.GetFunds());

            date += TimeUnit.DAY;
            service.Update(date, repo);
            Assert.AreEqual(400, repo.GetFunds());

            date += TimeUnit.DAY;
            service.Update(date, repo);
            Assert.AreEqual(800, repo.GetFunds());
        }

        [Test()]
        public void add_rent_every_day_for_1_civilian()
        {
            CivPopRepository repo = new CivPopRepository();

            repo.Add(new CivPopVessel("vessel"));

            CivPopKerbal valentina = new CivPopKerbal("Valentina", CivPopKerbalGender.FEMALE, 0, true);
            repo.Add(valentina);
            valentina.SetVesselId("vessel");

            CivPopKerbal bill = new CivPopKerbal("Bill", CivPopKerbalGender.MALE, 0, false);
            repo.Add(bill);
            bill.SetVesselId("vessel");

            CivPopServiceRent service = new CivPopServiceRent();

            double date = 0;

            service.Update(date, repo);
            Assert.AreEqual(0, repo.GetFunds());

            date += 60 * 60;
            service.Update(date, repo);
            Assert.AreEqual(0, repo.GetFunds());

            date += TimeUnit.DAY;
            service.Update(date, repo);
            Assert.AreEqual(200, repo.GetFunds());

            date += TimeUnit.DAY;
            service.Update(date, repo);
            Assert.AreEqual(400, repo.GetFunds());
        }

        [Test()]
        public void not_add_rent_for_1_dead_civilian()
        {
            CivPopRepository repo = new CivPopRepository();

            repo.Add(new CivPopVessel("vessel"));

            CivPopKerbal valentina = new CivPopKerbal("Valentina", CivPopKerbalGender.FEMALE, 0, true);
            repo.Add(valentina);
            valentina.SetVesselId("vessel");
            repo.Kill(valentina);

            CivPopKerbal bill = new CivPopKerbal("Bill", CivPopKerbalGender.MALE, 0, false);
            repo.Add(bill);
            bill.SetVesselId("vessel");

            CivPopServiceRent service = new CivPopServiceRent();

            double date = 0;

            service.Update(date, repo);
            Assert.AreEqual(0, repo.GetFunds());

            date += 60 * 60;
            service.Update(date, repo);
            Assert.AreEqual(0, repo.GetFunds());

            date += TimeUnit.DAY;
            service.Update(date, repo);
            Assert.AreEqual(0, repo.GetFunds());

            date += TimeUnit.DAY;
            service.Update(date, repo);
            Assert.AreEqual(0, repo.GetFunds());
        }

        [Test()]
        public void not_add_rent_for_1_civilians_on_KSC()
        {
            CivPopRepository repo = new CivPopRepository();
            repo.Add(new CivPopKerbal("Valentina", CivPopKerbalGender.FEMALE, 0, true));

            CivPopServiceRent service = new CivPopServiceRent();

            double date = 0;

            service.Update(date, repo);
            Assert.AreEqual(0, repo.GetFunds());

            date += 60 * 60;
            service.Update(date, repo);
            Assert.AreEqual(0, repo.GetFunds());

            date += TimeUnit.DAY;
            service.Update(date, repo);
            Assert.AreEqual(0, repo.GetFunds());

            date += TimeUnit.DAY;
            service.Update(date, repo);
            Assert.AreEqual(0, repo.GetFunds());
        }
    }
}

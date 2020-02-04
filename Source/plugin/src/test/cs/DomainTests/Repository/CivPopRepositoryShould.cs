using System;
using System.Linq;
using NUnit.Framework;

namespace CivilianPopulation.Domain.Repository
{
    [TestFixture()]
    public class CivPopRepositoryShould
    {
        [Test()]
        public void encode_and_decode_empty_repository()
        {
            CivPopRepository empty = new CivPopRepository();

            string json = empty.ToJson();
            CivPopRepository fromJson = new CivPopRepository(json);

            Assert.AreEqual(
                empty,
                fromJson
            );
        }

        [Test()]
        public void encode_and_decode_repository_having_roster_and_fleet()
        {
            CivPopRepository repo = new CivPopRepository();
            repo.Add(new CivPopKerbal("Valentina", CivPopKerbalGender.FEMALE, 0, true));
            repo.Add(new CivPopVessel(Guid.NewGuid().ToString()));

            string json = repo.ToJson();
            CivPopRepository fromJson = new CivPopRepository(json);

            Assert.AreEqual(
                repo.ToJson(),
                fromJson.ToJson()
            );
        }

        [Test()]
        public void encode_and_decode_repository_having_funds()
        {
            CivPopRepository repo = new CivPopRepository();
            repo.AddFunds(200);

            string json = repo.ToJson();
            CivPopRepository fromJson = new CivPopRepository(json);

            Assert.AreEqual(
                repo.ToJson(),
                fromJson.ToJson()
            );
        }

        [Test()]
        public void allow_update_on_vessels()
        {
            CivPopRepository repo = new CivPopRepository();

            CivPopVessel vessel = new CivPopVessel(Guid.NewGuid().ToString());
            repo.Add(vessel);

            string json = repo.ToJson();
            CivPopRepository fromJson = new CivPopRepository(json);

            fromJson.Add(vessel);

            Assert.AreEqual(
                repo.ToJson(),
                fromJson.ToJson()
            );
        }

        [Test()]
        public void allow_trait_change_on_kerbals()
        {
            CivPopRepository repo = new CivPopRepository();

            CivPopKerbal val = new CivPopKerbal("Valentina", CivPopKerbalGender.FEMALE, 0, true);
            repo.Add(val);

            string json = repo.ToJson();
            CivPopRepository fromJson = new CivPopRepository(json);

            val.SetCivilian(false);
            fromJson.Add(val);

            Assert.AreEqual(
                repo.ToJson(),
                fromJson.ToJson()
            );
        }

        [Test()]
        public void allow_removing_of_not_existing_kerbal()
        {
            CivPopRepository repo = new CivPopRepository();
            CivPopKerbal val = new CivPopKerbal("Valentina", CivPopKerbalGender.FEMALE, 0, true);
            
            repo.Remove(val);
            
            Assert.AreEqual(
                    repo.GetRoster().Count(),
                    0
            );
        }
    }
}

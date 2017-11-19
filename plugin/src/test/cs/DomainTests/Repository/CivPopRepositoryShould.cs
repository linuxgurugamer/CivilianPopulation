using System;
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
            repo.Add(new CivPopKerbal("Valentina", true));
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

    }
}

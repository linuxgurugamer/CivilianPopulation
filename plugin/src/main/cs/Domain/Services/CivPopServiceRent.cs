using System;
using System.Linq;
using CivilianPopulation.Domain.Repository;

namespace CivilianPopulation.Domain.Services
{
    public class CivPopServiceRent : CivPopOncePerDayService
    {
        private long rent;

        public CivPopServiceRent()
        {
            this.rent = 200;
        }

        protected override void DoUpdate(double date, CivPopRepository repo)
        {
            int count = repo.GetRoster()
                            .Where(kerbal => kerbal.IsCivilian())
                            .Where(kerbal => kerbal.GetVesselId() != null)
                            .Where(kerbal => !kerbal.IsDead())
                            .Count()
                            ;
            repo.AddFunds(this.rent * count);
        }
    }
}

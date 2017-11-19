using System;
using System.Linq;
using CivilianPopulation.Domain.Repository;

namespace CivilianPopulation.Domain.Services
{
    public class CivPopServiceRent : CivPopOncePerDayService
    {
        public CivPopServiceRent()
        {
        }

        protected override void DoUpdate(double date, CivPopRepository repo)
        {
            int count = repo.GetRoster()
                            .Where(kerbal => kerbal.IsCivilian())
                            .Where(kerbal => kerbal.GetVesselId() != null)
                            .Count()
                            ;
            repo.AddFunds(200 * count);
        }
    }
}

using CivilianPopulation.Domain.Repository;
using System;

namespace CivilianPopulation.Domain.Services
{
    public abstract class CivPopOncePerDayService
    {
        private double lastUpdate = -1;

        public CivPopOncePerDayService()
        {
        }



        public void Update(double date, CivPopRepository repo, bool now = false)
        {
            if (lastUpdate > -1 && GetDay(lastUpdate) < GetDay(date) || now)
            {
                DoUpdate(date, repo);
            }
            lastUpdate = date;
        }

        protected abstract void DoUpdate(double date, CivPopRepository repo);

        private double GetDay(double date) => Math.Floor(date / TimeUnit.DAY);
    }
}

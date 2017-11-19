using System;
using System.Linq;

namespace CivilianPopulation.Domain
{
    public class CivilianPopulationDeathService
    {
        private Action<CivilianKerbal> kill;

        protected System.Random rng;
        private double lastUpdate;

        public CivilianPopulationDeathService(Action<CivilianKerbal> kill)
        {
            this.kill = kill;
            this.lastUpdate = -1;
            this.rng = new System.Random();
        }

        public void update(double currentDate, CivilianKerbalRoster roster)
        {
            if (lastUpdate > -1 && getDay(lastUpdate) < getDay(currentDate))
            {
                doUpdate(currentDate, roster);
            }
            lastUpdate = currentDate;
        }

        protected void doUpdate(double currentDate, CivilianKerbalRoster roster)
        {
            foreach (CivilianKerbal kerbal in roster.list())
            {
                int age = (int)((currentDate - kerbal.getBirthDate()) / TimeUnit.YEAR);
                int chanceOfDeath = getChanceOfDeath(age);
                if (chanceOfDeath > 0)
                {
                    if (rng.Next() % chanceOfDeath == 0)
                    {
                        kill(kerbal);
                    }
                }
            }    
        }

        public int getChanceOfDeath(int age)
        {
            int res = 0;
            if (age > 75)
            {
                res = (int)(age - 75) * TimeUnit.DAYS_PER_YEARS;
            }
            return res;
        }

        private double getDay(double date)
        {
            return Math.Floor(date / TimeUnit.DAY);
        }
    }
}

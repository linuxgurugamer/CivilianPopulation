using CivilianPopulation.Domain.Repository;

namespace CivilianPopulation.Domain.Services
{
    public class CivPopServiceDeath : CivPopOncePerDayService
    {

        protected System.Random rng;

        public CivPopServiceDeath()
        {
            this.rng = new System.Random();
        }

        protected override void DoUpdate(double date, CivPopRepository repo)
        {
            foreach (CivPopKerbal kerbal in repo.GetRoster())
            {
                int age = (int)((date - kerbal.GetBirthdate()) / TimeUnit.YEAR);
                int chanceOfDeath = GetChanceOfDeath(age);
                if (chanceOfDeath > 0)
                {
                    if (rng.Next() % chanceOfDeath == 0)
                    {
                        repo.Kill(kerbal);
                    }
                }
            }
        }

        public int GetChanceOfDeath(int age)
        {
            int res = 0;
            if (age > 75)
            {
                res = (int)(age - 75) * TimeUnit.DAYS_PER_YEARS;
            }
            return res;
        }
    }
}

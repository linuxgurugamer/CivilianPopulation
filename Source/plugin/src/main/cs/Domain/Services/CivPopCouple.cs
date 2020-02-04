using CivilianPopulation.Domain.Repository;

namespace CivilianPopulation.Domain.Services
{
    public class CivPopCouple
    {
        private CivPopKerbal male;
        private CivPopKerbal female;

        public CivPopCouple(CivPopKerbal male, CivPopKerbal female)
        {
            this.male = male;
            this.female = female;
        }

        public CivPopKerbal GetFemale()
        {
            return this.female;
        }
    }
}

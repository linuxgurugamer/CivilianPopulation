using CivilianPopulation.Domain.Repository;
using System;

namespace CivilianPopulation.Domain.Services
{
    public class CivPopKerbalBuilder
    {
        private Func<CivPopKerbalGender, string> getName;
        private System.Random rng;

        public CivPopKerbalBuilder(Func<CivPopKerbalGender, string> getName)
        {
            this.getName = getName;
            this.rng = new System.Random();
        }

        public CivPopKerbalBuilder(Func<CivPopKerbalGender, string> getName, System.Random rng)
        {
            this.getName = getName;
            this.rng = rng;
        }

        public CivPopKerbal build(double date)
        {
            CivPopKerbalGender gender = CivPopKerbalGender.FEMALE;
            if (rng.Next() % 2 == 0)
            {
                gender = CivPopKerbalGender.MALE;
            }
            double age = date - 15 * TimeUnit.YEAR - rng.Next(15 * TimeUnit.YEAR);
            return new CivPopKerbal(getName(gender), gender, age, true);
        }
    }
}

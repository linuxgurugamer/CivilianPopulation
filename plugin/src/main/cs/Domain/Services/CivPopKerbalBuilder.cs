using System;
using CivilianPopulation.Domain.Repository;

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

        public CivPopKerbal build()
        {
            string name;
            if (rng.Next() % 2 == 0)
            {
                name = getName(CivPopKerbalGender.MALE);
            }
            else
            {
                name = getName(CivPopKerbalGender.FEMALE);
            }
            return new CivPopKerbal(name, true);
        }
    }
}

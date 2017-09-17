using System;

namespace CivilianPopulation.Domain
{
    public class CivilianKerbalCouple
    {
        private CivilianKerbal male;
        private CivilianKerbal female;

        public CivilianKerbalCouple(CivilianKerbal male, CivilianKerbal female)
        {
            this.male = male;
            this.female = female;
        }

        public CivilianKerbal getFemale()
        {
            return this.female;
        }
    }
}
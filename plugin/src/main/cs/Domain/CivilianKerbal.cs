using System;

namespace CivilianPopulation.Domain
{
    public class CivilianKerbal
    {
        private double expectingBirthAt = -1;
        private bool male;

        public CivilianKerbal(bool male)
        {
            this.male = male;
        }

        public double getExpectingBirthAt()
        {
            return expectingBirthAt;
        }

        public void setExpectingBirthAt(double date)
        {
            this.expectingBirthAt = date;
        }

        public Boolean isMale() 
        {
            return this.male;
        }

    }
}
namespace CivilianPopulation
{
    public class CivilianPopulationConfiguration
    {
        private double timeBetweenRents;
        private int rentAmountPerCivilian;

        public double getTimeBetweenRents()
        {
            return this.timeBetweenRents;
        }

        public void setTimeBetweenRents(double timeBetweenRents)
        {
            this.timeBetweenRents = timeBetweenRents;
        }

        public int getRentAmountPerCivilian()
        {
            return this.rentAmountPerCivilian;
        }

        public void setRentAmountPerCivilian(int rentAmountPerCivilian)
        {
            this.rentAmountPerCivilian = rentAmountPerCivilian;
        }

    }
}

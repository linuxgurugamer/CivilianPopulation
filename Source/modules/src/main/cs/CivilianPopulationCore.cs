namespace CivilianPopulation
{
    public class CivilianPopulationCore
    {
        private CivilianPopulationAdapter adapter;
        private CivilianPopulationConfiguration configuration;

        private double nextTaxesDate;

        public CivilianPopulationCore(CivilianPopulationAdapter adapter, CivilianPopulationConfiguration configuration)
        {
            this.adapter = adapter;
            this.configuration = configuration;
            nextTaxesDate = getNextTaxesDate();
        }

        public void update()
        {
            if (adapter.isCareer())
            {
                double next = getNextTaxesDate();
                if (nextTaxesDate < next)
                {
                    nextTaxesDate = next;
                    int numCivilians = 0;
                    foreach (CivilianVessel vessel in adapter.getVessels())
                    {
                        numCivilians += vessel.getCivilianCount();
                    }
                    adapter.addFunds(numCivilians * configuration.getRentAmountPerCivilian());
                }
            }
        }

        public double getTimeUntilTaxes()
        {
            return getNextTaxesDate() - adapter.getUniversalTime();
        }

        private double getNextTaxesDate()
        {
            double now = adapter.getUniversalTime();
            double next = 0;
            while (next < now)
            {
                next += configuration.getTimeBetweenRents();
            }
            return next;
        }
    }
}
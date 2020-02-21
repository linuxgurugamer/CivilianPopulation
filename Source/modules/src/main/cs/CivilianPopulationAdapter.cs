using System.Collections.Generic;

namespace CivilianPopulation
{
    public interface CivilianPopulationAdapter
    {
        double getUniversalTime();

        void addFunds(int numCivilians);

        bool isCareer();

        List<CivilianVessel> getVessels();

    }
}

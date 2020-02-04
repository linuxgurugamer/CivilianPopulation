using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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

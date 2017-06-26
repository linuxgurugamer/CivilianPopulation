using System;
using System.Collections.Generic;
using CivilianPopulation;

namespace CivilianPopulationShould
{
	internal class TestAdapter : CivilianPopulationAdapter
	{
		public double universalTime;

		public void addFunds(int amount)
		{
			System.Console.Write("add " + amount + " founds");
		}

		public double getUniversalTime()
		{
			return this.universalTime;
		}
		public void setUniversalTime(double universalTime)
		{
			this.universalTime = universalTime;
		}

		public List<CivilianVessel> getVessels()
		{
			return new List<CivilianVessel>();
		}

		public bool isCareer()
		{
			return true;
		}

	}
}

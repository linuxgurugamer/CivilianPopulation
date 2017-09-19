using System;
namespace CivilianPopulation.Infra
{
    public class CivilianPopulationHousingModule : PartModule
    {
		public void Start()
		{
			// log(" - Start !");
		}

		public void Update()
		{
			// log(" - Update !");
		}

		public void FixedUpdate()
		{
			if (vessel != null)
			{
				foreach (VesselModule module in vessel.vesselModules)
				{
					if (module.GetType() == typeof(CivilianPopulationGrowthVesselModule))
					{
						CivilianPopulationGrowthVesselModule civPopModule = (CivilianPopulationGrowthVesselModule)module;
						civPopModule.updateCapacity();
					}
				}
			}
		}
	}
}

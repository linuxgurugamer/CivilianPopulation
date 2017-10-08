using System;
using UnityEngine;

namespace CivilianPopulation.Infra
{
    public class CivilianPopulationHousingModule : PartModule
    {
		[KSPField(isPersistant = true, guiActive = false)]
		public bool activated;

		public void Start()
		{
			// log(" - Start !");
		}

		public void Update()
		{
			Events["activate"].active = !activated;
			Events["deactivate"].active = activated;
		}

		public void FixedUpdate()
		{
			// log(" - FixedUpdate !");
		}

		[KSPEvent(guiName = "Allow breeding", active = true, guiActive = true)]
		public void activate()
		{
			this.activated = true;
		}

		[KSPEvent(guiName = "Forbid breeding", active = false, guiActive = true)]
		public void deactivate()
		{
			this.activated = false;
		}

		public bool isActivated()
		{
			return this.activated;
		}

		private void log(string message)
		{
			Debug.Log(this.GetType().Name + message);
		}

	}
}

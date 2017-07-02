using System;
using UnityEngine;

namespace CivilianPopulation.Infra
{
    public class CivilianPopulationDockModule : BaseConverter
    {
		public void Start()
		{
            log(" - Start !");
		}

		public void Update()
		{
			log(" - Update !");
		}

        public override void FixedUpdate()
		{
            base.FixedUpdate();
			log(" - FixedUpdate !");
		}

		private void log(string message)
		{
			Debug.Log(this.GetType().Name + message);
		}
	}
}

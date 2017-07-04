using System;
using System.Collections.Generic;
using CivilianPopulation.Domain;
using UnityEngine;

namespace CivilianPopulation.Infra
{
    public class CivilianPopulationVesselModule : VesselModule
    {
		private const double DELIVERY_DELAY = 60 * 60 * 6;

        private TimeFormatter formatter = new TimeFormatter();

		[KSPField(isPersistant = true, guiActive = false)]
		public int capacity;
		[KSPField(isPersistant = true, guiActive = false)]
		public string delivery;
		[KSPField(isPersistant = true, guiActive = false)]
		public int waiting;

		public void Update()
        {
        }

        public void FixedUpdate()
        {
            if (this.delivery == null )
            {
                this.delivery = "0";
            }
            List<CivilianPopulationDockModule> docks = vessel.FindPartModulesImplementing<CivilianPopulationDockModule>();
            if (docks.Count > 0)
            {
                this.capacity = 0;
                foreach(CivilianPopulationDockModule dock in docks)
                {
                    if (dock.isActivated())
                    {
                        capacity += dock.part.CrewCapacity - dock.part.protoModuleCrew.Count;
					}
                }
            }

            if (this.capacity > 0 && delivery != "0")
            {
                double deliveryDate = Convert.ToDouble(this.delivery);
				if (deliveryDate < Planetarium.GetUniversalTime())
                {
					this.setDeliveryDate(Planetarium.GetUniversalTime() + DELIVERY_DELAY);
					this.addNewCivilian();
                }
            }
		}

        private void addNewCivilian()
        {
			this.waiting = this.waiting + 1;
			this.capacity = this.capacity - 1;
            log(" - addNewCivilian : capacity is now " + this.capacity + ", " +this.waiting + " civilians will spawn.");
		}

        public int getCapacity()
        {
            return this.capacity;
        }

		public double getDeliveryDate()
		{
            double res = 0;
            try
            {
                res = Convert.ToDouble(this.delivery);
            } catch (Exception e)
            {
                log(e.Message);
                this.delivery = "0";
            }
            return res;
		}

		public void setDeliveryDate(double date)
		{
            log(" - setDeliveryDate - next delivery : " + this.formatter.format(date)); 
            this.delivery = date.ToString();
		}

		private void log(string message)
        {
            Debug.Log(this.GetType().Name + message);
        }
    }
}

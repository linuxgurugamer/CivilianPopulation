﻿using System;
using System.Collections.Generic;
using UnityEngine;

namespace CivilianPopulation.Infra
{
	public class CivilianDockGrowth : BaseConverter
	{
		public override void OnStart(StartState state)
		{
			bool shouldCheckForUpdate = getCheckForUpdate();
			if (shouldCheckForUpdate)
			{   //if master/slaves not set, flight status...should only be run once
				Debug.Log(this.GetType().Name + this.name + " is running OnStart()!");
				List<CivilianDockGrowth> partsWithCivies = vessel.FindPartModulesImplementing<CivilianDockGrowth>();
				setMasterSlaves(partsWithCivies);
			}
			else
			{                    //if master/slave set or flight status fail.  Should be run n-1 times where n = #parts
				Debug.Log(this.GetType().Name + "WARNING: " + this.name + " is skipping OnStart!");
			}
		}


        public override void FixedUpdate()
		{
			if (!HighLogic.LoadedSceneIsFlight)
				return;

			List<CivilianDockGrowth> listOfCivilianParts = vessel.FindPartModulesImplementing<CivilianDockGrowth>();

			if ((!master && !slave) || (master && slave))
			{
				setMasterSlaves(listOfCivilianParts);
				return;
			}

			int civilianPopulation = 0;
			int nonCivilianPopulation = 0;
			int civilianPopulationSeats = 0;
			double percentCurrentCivilian = 0d;
			//Debug.Log (debuggingClass.modName + "Master Status:  " + master + "Slave Status:  " + slave);

			if (master == true)
			{                //master is set during OnStart()
				double dt = Time.deltaTime;

				//Section to calculate growth variables
				civilianPopulation = countCiviliansOnShip(listOfCivilianParts);//number of seats taken by civilians in parts using class
				nonCivilianPopulation = countNonCiviliansOnShip(listOfCivilianParts);//number of 
				civilianPopulationSeats = countCivilianSeatsOnShip(listOfCivilianParts);//total seats implementing class
				percentCurrentCivilian = getResourceBudget("CivilianGrowthCounter");//get current value of Civilian Counter (0.0-1.0)
				percentCurrentCivilianRate = calculateLinearGrowthRate() * getRecruitmentSoIModifier();
				//how much civilianCounter will change on iteration

				//Section to create Civilians
				part.RequestResource("CivilianGrowthCounter", -percentCurrentCivilianRate * dt * TimeWarp.CurrentRate);
				if ((percentCurrentCivilian > 1.0) && (civilianPopulationSeats > civilianPopulation + nonCivilianPopulation))
				{
					placeNewCivilian(listOfCivilianParts);
					part.RequestResource("CivilianGrowthCounter", 1.0);
				}//end if condition to create Civilians

				lastActiveTime = Planetarium.GetUniversalTime();
			}//end if master part
			 //Debug.Log (debuggingClass.modName + "Finished FixedUpdate!");
		}// end FixedUpdate

		/// <summary>
		/// Calculates the growth rate for civilians taking rides up to the station.
		/// TODO:  
		/// </summary>
		/// <returns>The linear growth rate.</returns>
		public double calculateLinearGrowthRate()
		{
			double myRate = 0d;//seems to be essential to create a middle variable, else rate does not update (returns to 0)
			myRate = populationGrowthModifier;
			return myRate;
		}

		/// <summary>
		/// Gets the recruitment modifier from being with Kerbin's, Mun's. or Minmus' SoI.  It is easier for competing
		/// programs to get astronauts into Kerbin orbit than it is to Mun/Minus.  None of them are as good as you are.
		/// </summary>
		/// <returns>The recruitment modifier due to.</returns>
		double getRecruitmentSoIModifier()
		{
			double recruitmentRateModifier = 0d;
			//Debug.Log (debuggingClass.modName + FlightGlobals.currentMainBody.orbit.referenceBody.isHomeWorld);
			if (!vessel.LandedOrSplashed)
			{
				////print(FlightGlobals.currentMainBody.name);
				if (FlightGlobals.currentMainBody.isHomeWorld)
				{
					recruitmentRateModifier = 1.0;//case for Kerbin/home world
				}
				else
				{
					try
					{
						if (FlightGlobals.currentMainBody.orbit.referenceBody.isHomeWorld)
						{//gives NullReference if in orbit around Kerbol
							recruitmentRateModifier = 0.5;//case for moon in orbit aroudn Kerbin/home world
						}
					}
					catch (NullReferenceException)
					{//case for if in orbit around home body's star
					 //Debug.Log (debuggingClass.modName + "Problem finding SoI body!");
						recruitmentRateModifier = 0.0;//case for orbit around central star (which Kerbin/home world orbits)
					}
				}
			}
			return recruitmentRateModifier;
		}

		/// <summary>
		/// The current rate at which a civilian is created.  Typically around 1E-8 to start.
		/// </summary>
		[KSPField(isPersistant = true, guiActive = true, guiName = "Current Growth Rate")]
		public double percentCurrentCivilianRate;

		[KSPField(isPersistant = true, guiActive = false)]
		public double populationGrowthModifier;

		/// <summary>
		/// The time until taxes; once each day
		/// </summary>
		[KSPField(isPersistant = true, guiActive = true, guiName = "Time until Rent payment")]
		public double TimeUntilTaxes = 21600.0;

		///<summary>
		/// The last time the ship was active, to approximately 1 day.  Used to calculate time step between when craft was 
		/// loaded and when when it was activated.
		/// </summary>
		[KSPField(isPersistant = true, guiActive = false)]
		public double lastActiveTime;

		//only one part with this can be the master on any vessel.
		//this prevents duplicating the population calculation
		public bool master = false;
		public bool slave = false;


		/// <summary>
		/// Gets the first part within the vessel implementing Civilian Population and assigns it as the master.  Also
		/// sets all other parts implementing Civilian Population as slaves.
		/// </summary>
		/// <returns>The master part.</returns>
		public growthRate getMaster<growthRate>(List<growthRate> partsWithCivies) where growthRate : CivilianPopulationRegulator
		{
			growthRate foundMaster = null;
			foreach (growthRate p in partsWithCivies)
			{
				if (p.master)
				{               //initially only executes if master is set in OnStart()
					if (foundMaster != null)
					{  //if this is NOT the first time executing; seems to never execute
						p.slave = true;
						p.master = false;
						Debug.Log(this.GetType().Name + "Master part found; set to slave");
					}
					else
					{
						foundMaster = p;
						Debug.Log(this.GetType().Name + "Master part set");
					}
				}
			}
			return foundMaster;//first part containing Civilian Population resource
		}

		/// <summary>
		/// Checks status of on scene, vessel, and pre-initiliazation of craft.
		/// </summary>
		/// <returns><c>true </c>, if active flight and no master/slave detected in part, <c>false</c> otherwise.</returns>
		public bool getCheckForUpdate()
		{
			if (!HighLogic.LoadedSceneIsFlight)
			{//only care about running on flights because master/slaves are being set once
				return false;
			}
			if (this.vessel == null)
			{          //Make sure vessel is not empty (likely will cause error)
				return false;
			}
			if (master || slave)
			{              //If (for whatever reason) master/slaves already assigned (such as previous flight)
				return false;
			}
			return true;
		}

		/// <summary>
		/// Sets the master part.  All other parts implementing Civilian Population class are set to slaves.
		/// </summary>
		/// <returns>The master slaves.</returns>
		/// <param name="listOfMembers">List of members.</param>
		/// <typeparam name="growthRate">The 1st type parameter.</typeparam>
		public void setMasterSlaves<growthRate>(List<growthRate> listOfMembers) where growthRate : CivilianDockGrowth
		{
			{   //if master/slaves not set, flight status...should only be run once
				List<growthRate> partsWithCivies = vessel.FindPartModulesImplementing<growthRate>();
				foreach (growthRate part in partsWithCivies)
				{//reset all master/slaves
					part.master = false;
					part.slave = true;
				}
				//assign this part as master
				master = true;
				slave = false;
			}
		}

		/// <summary>
		/// Counts Civilians within parts implementing CivilianPopulationRegulator class.  This should be limited to only
		/// Civilian Population Parts.  It also only counts Kerbals with Civilian Population trait.  Iterates first over each
		/// part implementing CivilianPopulationRegulator, and then iterates over each crew member within that part.      
		/// </summary>
		/// <returns>The number of civilians on vessel</returns>
		/// <param name="listOfMembers">List of members.</param>
		public int countCiviliansOnShip<growthRate>(List<growthRate> listOfMembers) where growthRate : CivilianDockGrowth
		{
			int numberCivilians = 0;
			foreach (growthRate myRegulator in listOfMembers)
			{//check for each part implementing CivilianPopulationRegulator
				if (myRegulator.part.protoModuleCrew.Count > 0)
				{
					foreach (ProtoCrewMember kerbalCrewMember in myRegulator.part.protoModuleCrew)
					{//check for each crew member within each part above
						if (kerbalCrewMember.trait == "Civilian")
						{
							numberCivilians++;
						}//end if civilian
					}//end foreach kerbalCrewMember
				}//end if crew capacity
			}//end foreach part implementing class
			return numberCivilians;//number of Kerbals with trait: "Civilian" -> Civilian
		}

		/// <summary>
		/// Counts non-Civilians within parts implementing CivilianPopulationRegulator class.  This should be limited to only
		/// Civilian Population Parts.  It also only counts Kerbals without Civilian Population trait.  Iterates first over each
		/// part implementing CivilianPopulationRegulator, and then iterates over each crew member within that part.      
		/// </summary>
		/// <returns>The number of non-civilians on vessel.</returns>
		/// <param name="listOfMembers">List of members.</param>
		/// <typeparam name="growthRate">The 1st type parameter.</typeparam>
		public int countNonCiviliansOnShip<growthRate>(List<growthRate> listOfMembers) where growthRate : CivilianDockGrowth
		{
			int numberNonCivilians = 0;
			foreach (growthRate myRegulator in listOfMembers)
			{//check for each part implementing CivilianPopulationRegulator
				if (myRegulator.part.protoModuleCrew.Count > 0)
				{
					foreach (ProtoCrewMember kerbalCrewMember in myRegulator.part.protoModuleCrew)
					{//check for each crew member within each part above
						if (kerbalCrewMember.trait != "Civilian")
						{
							numberNonCivilians++;
						}//end if nonCivilian
					}//end foreach kerbalCrewMember
				}//end if crew capacity
			}//end foreach part implementing class
			return numberNonCivilians;//number of Kerbals without trait: "Civilian" -> Civilian
		}

		/// <summary>
		/// Counts the civilian seats on ship.
		/// </summary>
		/// <returns>The civilian seats on ship.</returns>
		/// <param name="listOfMembers">List of members.</param>
		public int countCivilianSeatsOnShip<growthRate>(List<growthRate> listOfMembers) where growthRate : CivilianDockGrowth
		{
			int numberPossibleSeats = 0;
			foreach (growthRate myRegulator in listOfMembers)
			{
				numberPossibleSeats += myRegulator.part.CrewCapacity;
			}
			return numberPossibleSeats;
		}

		/// <summary>
		/// This method will place a new civilian in a part containing CivlianPopulationRegulator.  It should only
		/// be called when there are seat positions open in onesuch part.  Perhaps in the future, there will be a specific
		/// part that generates Civilians.
		/// </summary>
		/// <param name="listOfMembers">List of members.</param>
		public void placeNewCivilian<growthRate>(List<growthRate> listOfMembers) where growthRate : CivilianDockGrowth
		{
			ProtoCrewMember newCivilian = createNewCrewMember("Civilian");
			bool civPlaced = false;
			foreach (growthRate currentPart in listOfMembers)
			{
				if (currentPart.part.CrewCapacity > currentPart.part.protoModuleCrew.Count && !civPlaced)
				{
					if (currentPart.part.AddCrewmember(newCivilian))
					{
						vessel.SpawnCrew();
						Debug.Log(this.GetType().Name + newCivilian.name + " has been placed successfully by placeNewCivilian");
						civPlaced = true;
					}
				}
			}
			if (civPlaced == false)
				Debug.Log(this.GetType().Name + "ERROR:  " + newCivilian.name + " could not be placed in method placeNewCivilian");
		}

		/// <summary>
		/// Creates the new crew member of trait kerbalTraitName.  It must be of type Crew because they seem to be the only
		/// type of Kerbal that can keep a trait.
		/// </summary>
		/// <returns>The new crew member.</returns>
		/// <param name="kerbalTraitName">Kerbal trait name.</param>
		ProtoCrewMember createNewCrewMember(string kerbalTraitName)
		{
			KerbalRoster roster = HighLogic.CurrentGame.CrewRoster;
			ProtoCrewMember newKerbal = roster.GetNewKerbal(ProtoCrewMember.KerbalType.Crew);
			KerbalRoster.SetExperienceTrait(newKerbal, kerbalTraitName);//Set the Kerbal as the specified role (kerbalTraitName)
			Debug.Log(this.GetType().Name + "Created " + newKerbal.name + ", a " + newKerbal.trait);
			return newKerbal;//returns newly-generated Kerbal
		}

		/// <summary>
		/// Gets the delta time of the physics (?) update.  First it confirms the game is in a valid state.  Then it calculats
		/// the time between physics update by comparing with Planetarium.GetUniversalTime() and GetMaxDeltaTime().
		/// </summary>
		/// <returns>The delta time.</returns>
		protected double GetDeltaTimex()
		{
			if (Time.timeSinceLevelLoad < 1.0f || !FlightGlobals.ready)
			{
				//Error:  Not sure what this error is for...maybe not enough time since load?
				Debug.Log(this.GetType().Name + "WARNING:  check timeSinceLevelLoad/FlightGlobals");
				Debug.Log(this.GetType().Name + "timeSinceLevelLoad = " + Time.timeSinceLevelLoad);
				Debug.Log(this.GetType().Name + "FlightGlobals.ready = " + !FlightGlobals.ready);
				return -1;
			}

			if (Math.Abs(lastUpdateTime) < float.Epsilon)
			{
				//Error:  Just started running
				Debug.Log(this.GetType().Name + "ERROR:  check lastUpdateTime");
				Debug.Log(this.GetType().Name + "lastUpdateTime = " + lastUpdateTime);
				lastUpdateTime = Planetarium.GetUniversalTime();
				return -1;
			}

			var deltaTime = Math.Min(Planetarium.GetUniversalTime() - lastUpdateTime, ResourceUtilities.GetMaxDeltaTime());
			return deltaTime;

			//why is deltaTime == 0?
			//return deltaTime;
		}

		/// <summary>
		/// Looks over vessel to find amount of a given resource matching name.  In this project's scope, it is used
		/// in order to determine how far along the civilian growth counter is towards creating a new Kerbal.
		/// </summary>
		/// <returns>The amount of resource matching name.</returns>
		/// <param name="name">Name.</param>
		public double getResourceBudget(string name)
		{
			/* TODO broken since 1.3
              if (this.vessel != null) {
                var myVar = this.part.Resources.Get (name).info.id;
                double civilianResourceAmount;
                double maxCivilianResourceAmount;//not used but needed for vessel.GetConnectedResourceTotals
                this.vessel.GetConnectedResourceTotals (myVar, out civilianResourceAmount, out maxCivilianResourceAmount, true);
                return civilianResourceAmount;
              }
             */
			return 0;
		}

		//Anything below this, I don't know what it does but it is essential to keep from seeing
		//"No Resource definition found for RESOURCE" error message in OnFixedUpdate.

		[KSPField]
		public string RecipeInputs = "";

		[KSPField]
		public string RecipeOutputs = "";

		[KSPField]
		public string RequiredResources = "";


		public ConversionRecipe Recipe
		{
			get { return _recipe ?? (_recipe = LoadRecipe()); }
		}

		private ConversionRecipe _recipe;

		protected override ConversionRecipe PrepareRecipe(double deltatime)
		{

			if (_recipe == null)
				_recipe = LoadRecipe();
			UpdateConverterStatus();
			if (!IsActivated)
				return null;
			return _recipe;
		}

		private ConversionRecipe LoadRecipe()
		{
			var r = new ConversionRecipe();
			try
			{

				if (!String.IsNullOrEmpty(RecipeInputs))
				{
					var inputs = RecipeInputs.Split(',');
					for (int ip = 0; ip < inputs.Length; ip += 2)
					{
						print(String.Format("[REGOLITH] - INPUT {0} {1}", inputs[ip], inputs[ip + 1]));
						r.Inputs.Add(new ResourceRatio
						{
							ResourceName = inputs[ip].Trim(),
							Ratio = Convert.ToDouble(inputs[ip + 1].Trim())
						});
					}
				}

				if (!String.IsNullOrEmpty(RecipeOutputs))
				{
					var outputs = RecipeOutputs.Split(',');
					for (int op = 0; op < outputs.Length; op += 3)
					{
						print(String.Format("[REGOLITH] - OUTPUTS {0} {1} {2}", outputs[op], outputs[op + 1],
						  outputs[op + 2]));
						r.Outputs.Add(new ResourceRatio
						{
							ResourceName = outputs[op].Trim(),
							Ratio = Convert.ToDouble(outputs[op + 1].Trim()),
							DumpExcess = Convert.ToBoolean(outputs[op + 2].Trim())
						});
					}
				}

				if (!String.IsNullOrEmpty(RequiredResources))
				{
					var requirements = RequiredResources.Split(',');
					for (int rr = 0; rr < requirements.Length; rr += 2)
					{
						print(String.Format("[REGOLITH] - REQUIREMENTS {0} {1}", requirements[rr], requirements[rr + 1]));
						r.Requirements.Add(new ResourceRatio
						{
							ResourceName = requirements[rr].Trim(),
							Ratio = Convert.ToDouble(requirements[rr + 1].Trim()),
						});
					}
				}
			}
			catch (Exception)
			{
				print(String.Format("[REGOLITH] Error performing conversion for '{0}' - '{1}' - '{2}'", RecipeInputs, RecipeOutputs, RequiredResources));
			}
			return r;
		}
	}
}
﻿using System;
using System.Collections.Generic;
using UnityEngine;
using KSP;

namespace CivilianPopulation
{
  public class KerbalRecruitment : PartModule
  {
    [KSPEvent (guiName = "Recruit Kerbal", active = true, guiActive = true)]
    void recruitKerbal ()
    {
      Debug.Log (debuggingClass.modName + "Kerbal Recruitment Button pressed!");
      bool changedTrait = false;
      List<ProtoCrewMember> vesselCrew = vessel.GetVesselCrew ();
      foreach (ProtoCrewMember crewMember in vesselCrew) {
        if (crewMember.trait == debuggingClass.civilianTrait && changedTrait == false) {
          crewMember.trait = getRandomTrait ();
          changedTrait = true;
          KerbalRoster.SetExperienceTrait(pcm, crewMember.trait); // can be combined and simpliied, but thought would leave as is to show.
          Debug.Log (debuggingClass.modName + crewMember.name + " is now a " + crewMember.trait + "!");
        }
      }
    }

    private string getRandomTrait ()
    {
      int numberOfClasses = 3;
      string kerbalTrait = "";
      int randNum;
      System.Random newRand = new System.Random ();
      randNum = newRand.Next () % numberOfClasses;
      if (randNum == 0)
        kerbalTrait = "Pilot";
      if (randNum == 1)
        kerbalTrait = "Engineer";
      if (randNum == 2)
        kerbalTrait = "Scientist";
      Debug.Log (debuggingClass.modName + "Created trait:  " + kerbalTrait);
      return kerbalTrait;
    }
  }
}

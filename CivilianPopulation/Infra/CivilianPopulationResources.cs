using CivilianPopulation.Domain;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace CivilianPopulation.Infra
{

    namespace CivilianPopulation.Infra
    {
        class CivilianPopulationResources : PartModule
        {
            public void Start()
            {
                foreach (var r in part.Resources)
                {
                    if (r.resourceName == Constants.INSPIRATION)
                        r.maxAmount = HighLogic.CurrentGame.Parameters.CustomParams<CP>().inspirationCost;
                    if (r.resourceName == Constants.EDU)
                        r.maxAmount = HighLogic.CurrentGame.Parameters.CustomParams<CP>().educationCost;
                    if (r.resourceName == Constants.FLIGHTEXP)
                        r.maxAmount = HighLogic.CurrentGame.Parameters.CustomParams<CP>().flightExperienceCost;
                    if (r.amount > r.maxAmount)
                        r.amount = r.maxAmount;
                }
            }
        }
    }
}

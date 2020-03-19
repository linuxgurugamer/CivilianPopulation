using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

namespace CivilianPopulation.Domain
{
    public class CivilianPopulationRecruitmentService
    {
        private string[] traits;
        //System.Random rng;

        public CivilianPopulationRecruitmentService(string traits)
        {
            this.traits = traits.Split(',');
            //rng = new System.Random();
        }

        public string selectTrait(int engineer, int pilot, int scientist, double educationRes, double educationNeeded, double flightExperienceRes, double flightExpNeeded)
        {
            Dictionary<string, float> foo = new Dictionary<string, float>();
            if (educationRes >= educationNeeded )
                foo.Add("Engineer", engineer);
            if (flightExperienceRes >= flightExpNeeded)
                foo.Add("Pilot", pilot);
            if (educationRes >= educationNeeded)
                foo.Add("Scientist", scientist);
            if (foo.Count == 0)
                return "";
            return foo.RandomElementByWeight(e => e.Value).Key;
 
            //return selectTrait(rng.Next());
        }
#if false
        public string selectTrait(int random)
        {
            return this.traits[random % traits.Length];
        }
#endif
    }
    public static class IEnumerableExtensions
    {
        internal static System.Random rng = null;

        public static T RandomElementByWeight<T>(this IEnumerable<T> sequence, Func<T, float> weightSelector)
        {
            if (rng == null)
                rng = new System.Random();

            float totalWeight = sequence.Sum(weightSelector);
            // The weight we are after...
            double itemWeightIndex = rng.NextDouble() * totalWeight;
            float currentWeightIndex = 0;

            foreach (var item in from weightedItem in sequence select new { Value = weightedItem, Weight = weightSelector(weightedItem) })
            {
                currentWeightIndex += item.Weight;

                // If we've hit or passed the weight we are after for this item then it's the one we want....
                if (currentWeightIndex >= itemWeightIndex)
                    return item.Value;

            }

            return default(T);

        }

    }
}

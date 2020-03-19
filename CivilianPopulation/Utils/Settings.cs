using System.Collections;
using System.Reflection;

namespace CivilianPopulation
{
    // http://forum.kerbalspaceprogram.com/index.php?/topic/147576-modders-notes-for-ksp-12/#comment-2754813
    // search for "Mod integration into Stock Settings

    // HighLogic.CurrentGame.Parameters.CustomParams<CP>().useSimplePop
    public class CP : GameParameters.CustomParameterNode
    {
        public override string Title { get { return ""; } }
        public override GameParameters.GameMode GameMode { get { return GameParameters.GameMode.ANY; } }
        public override string Section { get { return "Civilian Population"; } }
        public override string DisplaySection { get { return "Civilian Population"; } }
        public override int SectionOrder { get { return 1; } }
        public override bool HasPresets { get { return true; } }

#if false
        [GameParameters.CustomParameterUI("Use alternate skin",
            toolTip = "Use a more minimiliast skin")]
        public bool useAlternateSkin = false;
#endif

        [GameParameters.CustomFloatParameterUI("Flight experience needed", minValue = 5000, maxValue = 500000, stepCount = 101, displayFormat = "F0",
            toolTip = "How much flight experience is needed to become a pilot.  Minimum value is approximiately equal to 3 Kerbin days\n" +
            "Flight experience is obtained in the Flight School")]
        public double flightExperienceCost = 5000f;

        [GameParameters.CustomFloatParameterUI("Education needed", minValue = 5000, maxValue = 500000, stepCount = 101, displayFormat = "F0",
            toolTip = "How much education is needed to become either an engineer or a scientist.  Minimum value is approximiately equal to 3 Kerbin days\n" +
            "Education is obtained in the University")]
        public double educationCost = 5000f;

        [GameParameters.CustomFloatParameterUI("Inspiration needed", minValue = 50, maxValue = 500, stepCount = 101, displayFormat = "F0",
            toolTip = "Inspiration needed to be recruited.  Minimum value is approximiately equal to 3 Kerbin days")]
        public double inspirationCost = 50f;


        public override void SetDifficultyPreset(GameParameters.Preset preset)
        {
            switch (preset)
            {
                case GameParameters.Preset.Easy:
                    flightExperienceCost = 2500;
                    educationCost = 2500;
                    inspirationCost = 50f;
                    break;
                case GameParameters.Preset.Normal:
                    flightExperienceCost = 5000f;
                    educationCost = 5000f;
                    inspirationCost = 50f;
                    break;
                case GameParameters.Preset.Moderate:
                    flightExperienceCost = 15000f;
                    educationCost = 15000f;
                    inspirationCost = 150f;
                    break;
                case GameParameters.Preset.Hard:
                    flightExperienceCost = 50000f;
                    educationCost = 50000f;
                    inspirationCost = 500f;
                    break;

            }
        }

        public override bool Enabled(MemberInfo member, GameParameters parameters) { return true; }

        public override bool Interactible(MemberInfo member, GameParameters parameters) { return true; }

        public override IList ValidValues(MemberInfo member) { return null; }
    }

}
using System;
using System.Linq;
using System.Text;

namespace CivilianPopulation.Domain
{

    public class MovieTheater : BaseConverter
    {
        [KSPField(isPersistant = false, guiActive = false)]
        public float pilotBonus = .10f;
        [KSPField(isPersistant = false, guiActive = false)]
        public float engineerBonus = .10f;
        [KSPField(isPersistant = false, guiActive = false)]
        public float scientistBonus = .10f;

        [KSPField(isPersistant = false, guiActive = false)]
        public float RomanceMovieBonus = .05f;

        [KSPField(isPersistant = false, guiActive = false)]
        public string InspirationResourceName = "inspiration";

        [KSPField(isPersistant = true, guiActive = true, guiName = "Movie Type Playing")]
        public string MovieType = "none";
        [KSPField(isPersistant = true, guiActive = true, guiName = "Bonus")]
        public string MovieBonus = "none";


        void resetPartInspiration()
        {
            /*  CHECK THIS OUT FOR BUG  */
        
            this.part.RequestResource(InspirationResourceName, 99999999, ResourceFlowMode.NO_FLOW); //reset this part's resource only
        
        }

#region regolith

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

        public override string GetInfo()
        {
            StringBuilder sb = new StringBuilder();
            var recipe = LoadRecipe();
            sb.Append(".");
            sb.Append("\n");
            sb.Append(ConverterName);
            sb.Append("\n\n<color=#99FF00>Inputs:</color>");
            foreach (var input in recipe.Inputs)
            {
                sb.Append("\n - ")
                    .Append(input.ResourceName)
                    .Append(": ");
                if (input.Ratio < 0.0001)
                {
                    sb.Append(String.Format("{0:0.00}", input.Ratio * 21600)).Append("/day");
                }
                else if (input.Ratio < 0.01)
                {
                    sb.Append(String.Format("{0:0.00}", input.Ratio * 3600)).Append("/hour");
                }
                else
                {
                    sb.Append(String.Format("{0:0.00}", input.Ratio)).Append("/sec");
                }

            }
            sb.Append("\n<color=#99FF00>Outputs:</color>");
            foreach (var output in recipe.Outputs)
            {
                sb.Append("\n - ")
                    .Append(output.ResourceName)
                    .Append(": ");
                if (output.Ratio < 0.0001)
                {
                    sb.Append(String.Format("{0:0.00}", output.Ratio * 21600)).Append("/day");
                }
                else if (output.Ratio < 0.01)
                {
                    sb.Append(String.Format("{0:0.00}", output.Ratio * 3600)).Append("/hour");
                }
                else
                {
                    sb.Append(String.Format("{0:0.00}", output.Ratio)).Append("/sec");
                }
            }
            if (recipe.Requirements.Any())
            {
                sb.Append("\n<color=#99FF00>Requirements:</color>");
                foreach (var output in recipe.Requirements)
                {
                    sb.Append("\n - ")
                        .Append(output.ResourceName)
                        .Append(": ");
                    sb.Append(String.Format("{0:0.00}", output.Ratio));
                }
            }
            sb.Append("\n");
            return sb.ToString();
        }



#endregion



        public override void OnUpdate()
        {
            Actions["StartResourceConverterAction"].active = false;
            Actions["StopResourceConverterAction"].active = false;
            base.OnUpdate();
        }
        public override void OnStart(PartModule.StartState state)
        {


            base.OnStart(state);

            Events["StartResourceConverter"].active = false;
            Events["StopResourceConverter"].active = false;


        }

        [KSPEvent(guiName = "Play Racing Movies (engineer bonus)", active = true, guiActive = true)]
        public void playRacingMovies()
        {
            resetPartInspiration();
            MovieType = "Racing Movies";
            MovieBonus = "-10% Engineer recruitment cost";
            StartResourceConverter();
            Events["StartResourceConverter"].active = false;
            Events["StopResourceConverter"].active = false;

        }
        [KSPEvent(guiName = "Play Scifi Movies (pilot bonus)", active = true, guiActive = true)]
        public void playScifi()
        {
            resetPartInspiration();
            MovieType = "Scifi Movies";
            MovieBonus = "-10% Pilot recruitment cost";
            StartResourceConverter();
            Events["StartResourceConverter"].active = false;
            Events["StopResourceConverter"].active = false;

        }
        [KSPEvent(guiName = "Play Documentaries (scientist bonus)", active = true, guiActive = true)]
        public void playDocumentaries()
        {
            resetPartInspiration();
            MovieType = "Documentaries";
            MovieBonus = "-10% Scientist recruitment cost";
            StartResourceConverter();
            Events["StartResourceConverter"].active = false;
            Events["StopResourceConverter"].active = false;

        }
        [KSPEvent(guiName = "Play Romance Movies", active = true, guiActive = true)]
        public void playRomance()
        {
            resetPartInspiration();
            MovieType = "Romance";
            MovieBonus = "10% Bonus to Civilian Reproduction";
            StartResourceConverter();
            Events["StartResourceConverter"].active = false;
            Events["StopResourceConverter"].active = false;
        }


        [KSPEvent(guiName = "Close Movie Theater", active = true, guiActive = true)]
        public void closeTheater()
        {
            resetPartInspiration();
            MovieType = "None";
            MovieBonus = "No bonus";
            StopResourceConverter();
            Events["StartResourceConverter"].active = false;
            Events["StopResourceConverter"].active = false;

        }


    }
}

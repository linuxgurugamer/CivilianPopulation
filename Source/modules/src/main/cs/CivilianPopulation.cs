using UnityEngine;

namespace CivilianPopulation
{
    [KSPScenario(ScenarioCreationOptions.AddToAllGames, GameScenes.FLIGHT, GameScenes.TRACKSTATION, GameScenes.SPACECENTER, GameScenes.EDITOR)]
    public class CivilianPopulation : ScenarioModule
    {
        private const int RENT_DELAY = 6 * 60 * 60;
        private const int RENT_PER_CIVILIAN = 200;

        private static CivilianPopulationCore core;
        private static CivilianPopulationUI ui;

        public void Start()
        {
            CivilianPopulationAdapter adapter = new KSPAdapter();
            CivilianPopulationConfiguration configuration = new CivilianPopulationConfiguration();
            configuration.setTimeBetweenRents(RENT_DELAY);
            configuration.setRentAmountPerCivilian(RENT_PER_CIVILIAN);

            core = new CivilianPopulationCore(adapter, configuration);
            ui = new CivilianPopulationUI(adapter, core);
        }

        public void OnGUI()
        {
            ui.draw();
        }

        public void FixedUpdate()
        {
            core.update();
        }

        private void log(string message)
        {
            Debug.Log(this.GetType().Name + message);
        }
    }
}

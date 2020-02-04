using UnityEngine;
using ToolbarControl_NS;

namespace CivilianPopulation.GUI
{
    [KSPAddon(KSPAddon.Startup.MainMenu, true)]
    public class RegisterToolbar : MonoBehaviour
    {
        void Start()
        {
            ToolbarControl.RegisterMod(CivilianPopulationGUI.MODID, CivilianPopulationGUI.MODNAME);
        }
    }
}

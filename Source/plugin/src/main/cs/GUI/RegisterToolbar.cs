using ToolbarControl_NS;
using UnityEngine;

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

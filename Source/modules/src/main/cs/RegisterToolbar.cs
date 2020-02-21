
using ToolbarControl_NS;
using UnityEngine;

namespace CivilianPopulation
{

    [KSPAddon(KSPAddon.Startup.MainMenu, true)]
    public class RegisterToolbar2 : MonoBehaviour
    {
        void Start()
        {
            ToolbarControl.RegisterMod(CivilianPopulationUI.MODID, CivilianPopulationUI.MODNAME);
        }
    }
}


using UnityEngine;
using ToolbarControl_NS;

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

using ToolbarControl_NS;
using UnityEngine;



namespace CivilianPopulation.GUI
{
    [KSPAddon(KSPAddon.Startup.MainMenu, true)]
    public class RegisterToolbar : MonoBehaviour
    {
        public static GUISkin DefGuiSkin;
        public static GUIStyle labelCentered;
        bool init = false;
        void Start()
        {
           
            ToolbarControl.RegisterMod(CivilianPopulationGUI.MODID, CivilianPopulationGUI.MODNAME);
        }
        void OnGUI()
        {
            if (!init)
            {
                init = true;
                DefGuiSkin = UnityEngine.GUI.skin;
                labelCentered = new GUIStyle(DefGuiSkin.label);
                labelCentered.alignment = TextAnchor.UpperCenter;
            }
        }
    }
}

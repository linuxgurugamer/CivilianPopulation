using UnityEngine;
using KSP;
using KSP.UI.Screens;
using System.Collections;

namespace CivilianPopulation
{
    class ModuleAnimateGenericSound : ModuleAnimateGeneric
    {
        #region KSPFields
        [KSPField]
        public string deployEffectName = "#autoLOC_6001354";

        [KSPField]
        public string retractEffectName = "#autoLOC_6001354";

        [KSPField(isPersistant = true)]
        new public float animSpeed = -99f;

        #endregion

        string currentEffect = "";
        
        public void Start()
        {
            OnMoving.Add(DoOnMovingEffect);
            OnStop.Add(StopEffect);
        }

        public void  DoOnMovingEffect (float f1, float f2)
        {
            if (anim[animationName].time == 0)
            {
                currentEffect = retractEffectName;
                if (animSpeed != -99)
                    anim[animationName].speed = animSpeed;
            }
            else
            {
                currentEffect = deployEffectName;
                if (animSpeed != -99)
                    anim[animationName].speed = -animSpeed;
            }
            base.part.Effect(currentEffect, 1f, -1);
        }

        public void StopEffect(float f1)
        {
            base.part.Effect(currentEffect, 0f, -1);
            if (deployPercent > 0)
                currentEffect = "deployed";
            else
                currentEffect = "retracted";
            base.part.Effect(currentEffect, 1f, -1);
            StartCoroutine(StopIt(0.5f));
        }
        IEnumerator StopIt(float time)
        {
            yield return new WaitForSeconds(time);
            base.part.Effect(currentEffect, 0f, -1);
            currentEffect = "";
        }

        public void OnDestroy()
        {
            OnMoving.Remove(DoOnMovingEffect);
            OnStop.Remove(StopEffect);
        }

        [KSPEvent(unfocusedRange = 5f, guiActiveUnfocused = true, guiActive = true, guiActiveEditor = true, guiName = "#autoLOC_6001329")]
        new public void Toggle()
        {
            if (!HighLogic.LoadedSceneIsEditor)
            {
                base.Toggle();
                int num = base.part.symmetryCounterparts.Count;
                while (num-- > 0)
                {
                    if ((Object)base.part.symmetryCounterparts[num] != (Object)base.part)
                    {
                        base.part.symmetryCounterparts[num].Modules.GetModule<ModuleAnimateGenericSound>(0).DoToggle();
                    }
                }
            }
        }
        void DoToggle()
        {
            base.Toggle();
        }
    }
}

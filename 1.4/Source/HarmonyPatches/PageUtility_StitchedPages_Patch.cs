using HarmonyLib;
using RimWorld;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace VFETribals
{
    [HarmonyPatch(typeof(Page_ChooseIdeoPreset), "PostOpen")]
    public static class Page_ChooseIdeoPreset_PostOpen_Patch
    {
        public static void Postfix(Page_ChooseIdeoPreset __instance)
        {
            if (Current.Game.Scenario == VFET_DefOf.VFET_WildMen.scenario)
            {
                if (Find.WindowStack.WindowOfType<Page_ChooseIdeoPreset>() != null)
                {
                    Find.IdeoManager.classicMode = true;
                    __instance.DoNext();
                }
            }
        }
    }
}

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
            if (Current.Game.Scenario?.playerFaction?.factionDef == VFET_DefOf.VFET_WildMen)
            {
                __instance.classicIdeo.memberName = VFET_DefOf.VFET_WildMen.basicMemberKind.label;
                var allPreceptsToRemove = __instance.classicIdeo.precepts.Where(x => x.def.defName.StartsWith("VFET_") is false
                && x.def != PreceptDefOf.AnimaTreeLinking && x.def != VFET_DefOf.TreeConnection).ToList();
                foreach (var precept in allPreceptsToRemove)
                {
                    __instance.classicIdeo.RemovePrecept(precept);
                }
                if (Find.WindowStack.WindowOfType<Page_ChooseIdeoPreset>() != null)
                {
                    __instance.DoNext();
                }
            }
        }
    }
}

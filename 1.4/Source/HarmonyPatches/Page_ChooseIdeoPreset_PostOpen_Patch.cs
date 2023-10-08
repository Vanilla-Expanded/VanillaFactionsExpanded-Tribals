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
                if (Find.WindowStack.WindowOfType<Page_ChooseIdeoPreset>() != null)
                {
                    __instance.DoNext();
                }
            }
        }
    }
}

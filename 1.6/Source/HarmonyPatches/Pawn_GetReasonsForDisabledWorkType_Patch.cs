using HarmonyLib;
using RimWorld;
using System.Collections.Generic;
using Verse;

namespace VFETribals
{
    [HarmonyPatch(typeof(Pawn), "GetReasonsForDisabledWorkType")]
    public static class Pawn_GetReasonsForDisabledWorkType_Patch
    {
        public static void Postfix(Pawn __instance, WorkTypeDef workType, List<string> __result)
        {
            if (__instance.Faction == Faction.OfPlayerSilentFail && workType.IsUnlocked(out var research) is false) 
            {
                var reason = "VFET.WorkDisabledByMissingResearch".Translate(research.label);
                if (__result.Contains(reason) is false)
                {
                    __result.Add(reason);
                }
            }
        }
    }
}

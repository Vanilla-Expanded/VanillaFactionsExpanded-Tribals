using HarmonyLib;
using RimWorld;
using System;
using System.Linq;
using Verse;

namespace VFETribals
{
    [HarmonyPatch(typeof(Pawn), "CombinedDisabledWorkTags", MethodType.Getter)]
    public static class Pawn_CombinedDisabledWorkTags_Patch
    {
        public static void Postfix(Pawn __instance, ref WorkTags __result)
        {
            if (__instance.Faction == Faction.OfPlayerSilentFail)
            {
                foreach (var workTag in Enum.GetValues(typeof(WorkTags)).Cast<WorkTags>())
                {
                    if (!workTag.IsUnlocked())
                    {
                        __result |= workTag;
                    }
                }
            }
        }
    }
}

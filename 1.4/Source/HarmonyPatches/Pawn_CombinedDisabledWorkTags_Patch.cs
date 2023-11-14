using HarmonyLib;
using RimWorld;
using System;
using System.Diagnostics;
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
                foreach (var workTag in Utils.workTags)
                {
                    if (workTag != WorkTags.None && !workTag.IsUnlocked(out _))
                    {
                        __result |= workTag;
                    }
                }
            }
        }
    }
}

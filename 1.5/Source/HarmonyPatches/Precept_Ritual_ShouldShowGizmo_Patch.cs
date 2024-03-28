﻿using HarmonyLib;
using RimWorld;

namespace VFETribals
{
    [HarmonyPatch(typeof(Precept_Ritual), "ShouldShowGizmo")]
    public static class Precept_Ritual_ShouldShowGizmo_Patch
    {
        public static void Postfix(ref bool __result, Precept_Ritual __instance)
        {
            if (__instance.def == VFET_DefOf.VFET_TribalGathering)
            {
                if (Faction.OfPlayer.def.techLevel > TechLevel.Neolithic)
                {
                    __result = false;
                }
            }
        }
    }
}

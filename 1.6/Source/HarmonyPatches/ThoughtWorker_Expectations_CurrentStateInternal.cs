using HarmonyLib;
using RimWorld;
using UnityEngine;
using Verse;

namespace VFETribals
{
    [HarmonyPatch(typeof(ThoughtWorker_Expectations), "CurrentStateInternal")]
    public static class VFETribals_ThoughtWorker_Expectations_CurrentStateInternal_Patch
    {
        public static void Postfix(ref ThoughtState __result)
        {
            if (Find.ResearchManager.GetProgress(VFET_DefOf.VFET_Culture) < 1)
            {
                __result = ThoughtState.Inactive;
            }
        }
    }
}

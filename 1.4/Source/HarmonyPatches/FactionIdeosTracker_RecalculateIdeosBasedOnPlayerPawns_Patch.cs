using HarmonyLib;
using RimWorld;
using Verse;

namespace VFETribals
{
    [HarmonyPatch(typeof(FactionIdeosTracker), "RecalculateIdeosBasedOnPlayerPawns")]
    public static class FactionIdeosTracker_RecalculateIdeosBasedOnPlayerPawns_Patch
    {
        public static bool Prefix()
        {
            if (Find.WindowStack.IsOpen<Page_ConfigureIdeo_Colonists>() 
                || Find.WindowStack.IsOpen<Page_ConfigureFluidIdeo_Colonists>())
            {
                return false;
            }
            return true;
        }
    }
}

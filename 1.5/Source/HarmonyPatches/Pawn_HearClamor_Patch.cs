using HarmonyLib;
using Verse;

namespace VFETribals
{
    [HarmonyPatch(typeof(Pawn), "HearClamor")]
    public static class Pawn_HearClamor_Patch
    {
        public static bool Prefix(Thing source, ClamorDef type)
        {
            if (source?.def == VFET_DefOf.VFET_LargeFireSpark)
            {
                return false;
            }
            return true;
        }
    }
}

using HarmonyLib;
using RimWorld;
using Verse;

namespace VFETribals
{
    [HarmonyPatch(typeof(SocialCardUtility), "DrawPawnRoleSelection")]
    public static class SocialCardUtility_DrawPawnRoleSelection_Patch
    {
        public static bool Prefix(Pawn pawn)
        {
            if (pawn.Faction == Faction.OfPlayer && VFET_DefOf.VFET_Culture.IsFinished is false)
            {
                return false;
            }
            return true;
        }
    }
}

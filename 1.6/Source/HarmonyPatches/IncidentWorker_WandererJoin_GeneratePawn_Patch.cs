using HarmonyLib;
using RimWorld;
using Verse;

namespace VFETribals
{
    [HarmonyPatch(typeof(IncidentWorker_WandererJoin), "GeneratePawn")]
    public static class IncidentWorker_WandererJoin_GeneratePawn_Patch
    {
        public static void Postfix(Pawn __result)
        {
            if (__result != null && Faction.OfPlayer.def == VFET_DefOf.VFET_WildMen && __result.Ideo != Faction.OfPlayer.ideos.PrimaryIdeo)
            {
                __result.ideo.SetIdeo(Faction.OfPlayer.ideos.PrimaryIdeo);
            }
        }
    }
}

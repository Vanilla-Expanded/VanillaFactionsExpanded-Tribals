using HarmonyLib;
using RimWorld;
using Verse;

namespace VFETribals
{
    [HarmonyPatch(typeof(IncidentWorker_WandererJoin), "TryExecuteWorker")]
    public static class IncidentWorker_WandererJoin_TryExecuteWorker_Patch
    {
        public static void Prefix(IncidentWorker_WandererJoin __instance, out string __state)
        {
            __state = __instance.def.letterLabel;
            if (Faction.OfPlayer.def == VFET_DefOf.VFET_WildMen && __instance.def == VFET_DefOf.StrangerInBlackJoin)
            {
                __instance.def.letterLabel = "VFET.ManInBlackLabelReplacement".Translate();
            }
        }

        public static void Postfix(IncidentWorker_WandererJoin __instance, string __state)
        {
            __instance.def.letterLabel = __state;

            foreach (var colonist in PawnsFinder.AllMapsCaravansAndTravelingTransportPods_Alive_Colonists)
            {
                Log.Message(colonist + " - " + colonist.Ideo + " - " + colonist.Ideo?.memberName);
            }
        }
    }
}

using HarmonyLib;
using RimWorld;
using Verse;

namespace VFETribals
{
    [HarmonyPatch(typeof(IdeoUIUtility), "DoIdeoListAndDetails")]
    public static class IdeoUIUtility_DoIdeoListAndDetails_Patch
    {
        public static void Prefix(ref bool showCreateIdeoButton)
        {
            if (Current.ProgramState != ProgramState.Playing)
            {
                if (Current.Game.Scenario?.playerFaction?.factionDef == VFET_DefOf.VFET_WildMen)
                {
                    showCreateIdeoButton = false;
                }
            }
        }
    }
}

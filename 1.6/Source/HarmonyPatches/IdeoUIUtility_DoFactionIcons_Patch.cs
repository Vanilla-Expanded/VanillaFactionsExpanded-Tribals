using HarmonyLib;
using RimWorld;
using Verse;

namespace VFETribals
{
    [HarmonyPatch(typeof(IdeoUIUtility), "DoFactionIcons")]
    public static class IdeoUIUtility_DoFactionIcons_Patch
    {
        public static void Prefix(ref bool showSelectedAsPlayerIdeo)
        {
            if (Current.ProgramState != ProgramState.Playing)
            {
                if (Current.Game.Scenario?.playerFaction?.factionDef == VFET_DefOf.VFET_WildMen)
                {
                    showSelectedAsPlayerIdeo = false;
                }
            }
        }
    }
}

using HarmonyLib;
using RimWorld;
using Verse;

namespace VFETribals
{
    [HarmonyPatch(typeof(IdeoUIUtility), "DrawIdeoRow")]
    public static class IdeoUIUtility_DrawIdeoRow_Patch
    {
        public static bool Prefix(Ideo ideo)
        {
            if (Current.ProgramState != ProgramState.Playing)
            {
                if (Current.Game.Scenario?.playerFaction?.factionDef == VFET_DefOf.VFET_WildMen)
                {
                    if (ideo.initialPlayerIdeo)
                    {
                        return false;
                    }
                }
            }
            return true;
        }
    }
}

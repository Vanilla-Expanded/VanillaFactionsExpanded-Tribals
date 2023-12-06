using HarmonyLib;
using RimWorld;
using Verse;

namespace VFETribals
{
    [HarmonyPatch(typeof(Page_ConfigureIdeo), "CanDoNext")]
    public static class Page_ConfigureIdeo_CanDoNext_Patch
    {
        public static bool Prefix(ref bool __result)
        {
            if (Current.ProgramState != ProgramState.Playing)
            {
                if (Current.Game.Scenario?.playerFaction?.factionDef == VFET_DefOf.VFET_WildMen)
                {
                    __result = true;
                    Find.Scenario.PostIdeoChosen();
                    return false;
                }
            }
            return true;
        }
    }
}

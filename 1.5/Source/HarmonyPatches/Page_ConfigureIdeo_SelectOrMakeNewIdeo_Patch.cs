using HarmonyLib;
using RimWorld;
using Verse;

namespace VFETribals
{
    [HarmonyPatch(typeof(Page_ConfigureIdeo), "SelectOrMakeNewIdeo")]
    public static class Page_ConfigureIdeo_SelectOrMakeNewIdeo_Patch
    {
        public static bool Prefix(Page_ConfigureIdeo __instance, Ideo newIdeo = null)
        {
            if (Current.ProgramState != ProgramState.Playing)
            {
                if (Current.Game.Scenario?.playerFaction?.factionDef == VFET_DefOf.VFET_WildMen)
                {
                    __instance.ideo = newIdeo ?? IdeoUtility.MakeEmptyIdeo();
                    IdeoUIUtility.SetSelected(__instance.ideo);
                    LessonAutoActivator.TeachOpportunity(ConceptDefOf.EditingMemes, OpportunityType.Critical);
                    LessonAutoActivator.TeachOpportunity(ConceptDefOf.EditingPrecepts, OpportunityType.Critical);
                    return false;
                }
            }
            return true;
        }
    }
}

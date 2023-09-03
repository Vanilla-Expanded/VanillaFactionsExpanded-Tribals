using HarmonyLib;
using RimWorld;
using Verse;

namespace VFETribals
{
    [HarmonyPatch(typeof(ResearchManager), "FinishProject")]
    public static class ResearchManager_FinishProject_Patch
    {
        private static void Postfix(ResearchProjectDef proj, bool doCompletionDialog = false, Pawn researcher = null)
        {
            if (proj is TribalResearchProjectDef project)
            {
                if (project.unlocksWorkTypes != null)
                {
                    foreach (var pawn in PawnsFinder.AllMapsCaravansAndTravelingTransportPods_Alive)
                    {
                        pawn.Notify_DisabledWorkTypesChanged();
                    }
                }

                if (project == VFET_DefOf.VFET_Culture)
                {
                    Find.IdeoManager.classicMode = GameComponent_Tribals.Instance.classicIdeologyMode.Value;

                }
            }
        }
    }
}

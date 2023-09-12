using HarmonyLib;
using RimWorld;
using System.Collections.Generic;
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
                if (project.unlocksWorkTypes != null || project.unlocksWorkTags != WorkTags.None)
                {
                    foreach (var pawn in PawnsFinder.AllMapsCaravansAndTravelingTransportPods_Alive)
                    {
                        pawn.Notify_DisabledWorkTypesChanged();
                    }
                }

                if (Faction.OfPlayer.def == VFET_DefOf.VFET_WildMen && project == VFET_DefOf.VFET_Culture 
                    && ModsConfig.IdeologyActive)
                {
                    Find.LetterStack.ReceiveLetter(LetterMaker.MakeLetter("VFET.FormIdeology".Translate(), 
                        "VFET.FormIdeologyDesc".Translate(), VFET_DefOf.VFET_ConfigureIdeo));
                }
            }
        }
    }
}

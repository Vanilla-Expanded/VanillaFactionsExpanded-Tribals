using HarmonyLib;
using RimWorld;
using System.Collections.Generic;
using System.Linq;
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

            if (Find.Storyteller?.def == VFET_DefOf.VFET_TalonTribal)
            {
                if (GameComponent_Tribals.Instance.lastTickResearchFinished > 0 && GenDate.TicksPerDay * 4 >= 
                    Find.TickManager.TicksGame - GameComponent_Tribals.Instance.lastTickResearchFinished)
                {
                    var map = Find.AnyPlayerHomeMap; 
                    if (map != null)
                    {
                        var parms = StorytellerUtility.DefaultParmsNow(IncidentDefOf.RaidEnemy.category, map);
                        IncidentDefOf.RaidEnemy.Worker.TryExecute(parms);
                    }
                }
            }

            GameComponent_Tribals.Instance.lastTickResearchFinished = Find.TickManager.TicksGame;
            GameComponent_Tribals.Instance.TryRegisterAdvancementObligation();
        }
    }
}

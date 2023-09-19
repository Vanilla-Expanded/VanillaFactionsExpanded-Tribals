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


            List<ResearchProjectDef> activeResearchesOfTechLevel = (from x in DefDatabase<ResearchProjectDef>.AllDefsListForReading
                                                               where x.CanStartNow && x.techLevel == proj.techLevel && x.techprintCount==0
                                                               select x).ToList();

            if (activeResearchesOfTechLevel.Count == 0)
            {
                switch (proj.techLevel)
                {
                    case TechLevel.Animal:
                        GameComponent_Tribals.Instance.allAnimalResearchCompleted = true;
                        break;
                    case TechLevel.Neolithic:
                        GameComponent_Tribals.Instance.allNeolithicResearchCompleted = true;
                        break;
                    case TechLevel.Medieval:
                        GameComponent_Tribals.Instance.allMedievalResearchCompleted = true;
                        break;
                    case TechLevel.Industrial:
                        GameComponent_Tribals.Instance.allIndustrialResearchCompleted = true;
                        break;
                    case TechLevel.Spacer:
                        GameComponent_Tribals.Instance.allSpacerResearchCompleted = true;
                        break;
                   
                }
            }

        }
    }
}

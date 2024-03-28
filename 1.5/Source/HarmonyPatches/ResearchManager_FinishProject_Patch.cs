﻿using HarmonyLib;
using LudeonTK;
using RimWorld;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace VFETribals
{
    [HarmonyPatch(typeof(ResearchManager), "FinishProject")]
    public static class ResearchManager_FinishProject_Patch
    {
        private static void Postfix(ResearchProjectDef proj)
        {
            if (GameComponent_Tribals.Instance.FinishedResearchProjects.Contains(proj) is false)
            {
                GameComponent_Tribals.Instance.FinishedResearchProjects.Add(proj);
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
                        IssueFormIdeologyLetter();
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

        [DebugAction("General", "Form ideology")]
        public static void IssueFormIdeologyLetter()
        {
            Find.LetterStack.ReceiveLetter(LetterMaker.MakeLetter("VFET.FormIdeology".Translate(),
                "VFET.FormIdeologyDesc".Translate(), VFET_DefOf.VFET_ConfigureIdeo));
        }
    }

}

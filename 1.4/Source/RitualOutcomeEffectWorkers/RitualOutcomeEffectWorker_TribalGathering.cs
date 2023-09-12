using System;
using System.Collections.Generic;
using System.Linq;
using Verse;
using RimWorld;
using Verse.Sound;
using Verse.AI.Group;
using Verse.AI;
using static HarmonyLib.Code;


namespace VFETribals
{
    public class RitualOutcomeEffectWorker_TribalGathering : RitualOutcomeEffectWorker_FromQuality
    {
        public RitualOutcomeEffectWorker_TribalGathering()
        {
        }

        public RitualOutcomeEffectWorker_TribalGathering(RitualOutcomeEffectDef def) : base(def)
        {
        }

        public override bool SupportsAttachableOutcomeEffect => false;

        public override void Apply(float progress, Dictionary<Pawn, int> totalPresence, LordJob_Ritual jobRitual)
        {

            float quality = GetQuality(jobRitual, progress);
            OutcomeChance outcome = this.GetOutcome(quality, jobRitual);
            LookTargets lookTargets = jobRitual.selectedTarget;
            string text = null;
            
            bool flag = false;
            foreach (Pawn pawn in totalPresence.Keys)
            {

                GiveMemoryToPawn(pawn, outcome.memory, jobRitual);
            }


            List<ResearchProjectDef>  activeResearches = (from x in DefDatabase<ResearchProjectDef>.AllDefsListForReading
                                                          where x.CanStartNow
                                                          select x).ToList();

            int totalResearchPoints = 1;

            switch (outcome.positivityIndex)
            {
                case -2:
                    totalResearchPoints = 2 * totalPresence.Count;
                    break;
                case -1:
                    totalResearchPoints = 4 * totalPresence.Count;
                    break;
                case 1:
                    totalResearchPoints = 8 * totalPresence.Count;
                    break;
                case 2:
                    totalResearchPoints = 12 * totalPresence.Count;
                    break;
            }

            int pointsPerReseach = totalResearchPoints / activeResearches.Count;

            foreach (ResearchProjectDef project in activeResearches)
            {
                ReflectionCache.progress(Find.ResearchManager)[project] += pointsPerReseach;


            }






            string text2 = outcome.description.Formatted(jobRitual.Ritual.Label).CapitalizeFirst() + "\n\n" + this.OutcomeQualityBreakdownDesc(quality, progress, jobRitual);
            string text3 = this.def.OutcomeMoodBreakdown(outcome);
            if (!text3.NullOrEmpty())
            {
                text2 = text2 + "\n\n" + text3;
            }
            if (flag)
            {
                text2 += "\n\n" + "RitualOutcomeExtraDesc_Execution".Translate();
            }
            if (text != null)
            {
                text2 = text2 + "\n\n" + text;
            }
            string text4;
            this.ApplyDevelopmentPoints(jobRitual.Ritual, outcome, out text4);
            if (text4 != null)
            {
                text2 = text2 + "\n\n" + text4;
            }
            Find.LetterStack.ReceiveLetter("OutcomeLetterLabel".Translate(outcome.label.Named("OUTCOMELABEL"), jobRitual.Ritual.Label.Named("RITUALLABEL")), text2, outcome.Positive ? LetterDefOf.RitualOutcomePositive : LetterDefOf.RitualOutcomeNegative, lookTargets, null, null, null, null);




        }


    }
}

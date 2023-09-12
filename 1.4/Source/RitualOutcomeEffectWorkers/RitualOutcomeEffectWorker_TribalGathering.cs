using System;
using System.Collections.Generic;
using System.Linq;
using Verse;
using RimWorld;
using Verse.Sound;
using Verse.AI.Group;
using Verse.AI;
using static HarmonyLib.Code;
using Verse.Noise;


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
            string text = this.Description;
            Random randomWildmanOrFarmAnimals = new Random();
            Random extraEventChance = new Random();

            bool flag = false;
            foreach (Pawn pawn in totalPresence.Keys)
            {

                GiveMemoryToPawn(pawn, outcome.memory, jobRitual);
            }

            List<ResearchProjectDef> activeAnimalResearches = (from x in DefDatabase<ResearchProjectDef>.AllDefsListForReading
                                                         where x.CanStartNow && x.techLevel == TechLevel.Animal
                                                         select x).ToList();


            List<ResearchProjectDef>  activeNeolithicResearches = (from x in DefDatabase<ResearchProjectDef>.AllDefsListForReading
                                                          where x.CanStartNow && x.techLevel == TechLevel.Neolithic
                                                          select x).ToList();

            int totalResearchPoints = 1;

            switch (outcome.positivityIndex)
            {
                case -2:
                    totalResearchPoints = 20 * totalPresence.Count;
                    break;
                case -1:
                    totalResearchPoints = 40 * totalPresence.Count;
                    break;
                case 1:
                    totalResearchPoints = 80 * totalPresence.Count;
                    break;
                case 2:
                    totalResearchPoints = 120 * totalPresence.Count;
                    break;
            }
          
            List<ResearchProjectDef> activeResearches = new List<ResearchProjectDef>();

            if (activeAnimalResearches.Count > 0)
            {
                activeResearches = activeAnimalResearches;
            }
            else if (activeNeolithicResearches.Count > 0)
            {
                activeResearches = activeNeolithicResearches;
            }
            if (activeResearches.Count > 0)
            {
                if (Prefs.DevMode)
                {
                    Log.Message("Total research points is " + totalResearchPoints);
                }
                for (int i = 0; i < activeResearches.Count; i++)
                {
                    int pointsToAllocate = new IntRange(0, totalResearchPoints).RandomInRange;
                    totalResearchPoints -= pointsToAllocate;
                    if (totalResearchPoints < 0)
                    {
                        totalResearchPoints = 0;
                    }
                    if (i == activeResearches.Count - 1 && totalResearchPoints != 0)
                    {
                        pointsToAllocate = totalResearchPoints;
                    }
                    if (Prefs.DevMode) {
                        Log.Message("Research project " + activeResearches[i].LabelCap + " was allocated " + pointsToAllocate + " research points.");
                    }
                    
                    Find.ResearchManager.progress[activeResearches[i]] += pointsToAllocate;
                }
            }
            
       


            if (randomWildmanOrFarmAnimals.NextDouble() > 0.5)
            {
                bool wildmanWanders = false;
                
                switch (outcome.positivityIndex)
                {                  
                    case 1:
                        if (extraEventChance.NextDouble() > 0.75)
                        {
                            wildmanWanders = true;
                        }
                        break;
                    case 2:
                        if (extraEventChance.NextDouble() > 0.5)
                        {
                            wildmanWanders = true;
                        }
                        break;
                }

                if (wildmanWanders)
                {
                    Pawn wildman = PawnGenerator.GeneratePawn(new PawnGenerationRequest(PawnKindDefOf.WildMan, Faction.OfPlayer, PawnGenerationContext.NonPlayer, 
                        -1, forceGenerateNewPawn: true, allowDead: false, allowDowned: false, canGeneratePawnRelations: true, 
                        false, 20f, forceAddFreeWarmLayerIfNeeded: false, allowGay: true, allowPregnant: false, allowFood: true, allowAddictions: true, 
                        inhabitant: false, certainlyBeenInCryptosleep: false, forceRedressWorldPawnIfFormerColonist: false, worldPawnFactionDoesntMatter: false,
                        0f, 0f, null, 1f, null, null, null, null, null, null, null, null, null, null, null));
                    TryFindEntryCell(jobRitual.Map, out IntVec3 cell);
                    GenSpawn.Spawn(wildman, cell, jobRitual.Map);
                    Find.LetterStack.ReceiveLetter("VFET_WildmanLetterLabel".Translate(), "VFET_WildmanLetter".Translate(), LetterDefOf.RitualOutcomePositive, lookTargets, null, null, null, null);

                }

            }
            else
            {

                bool farmAnimalsWander = false;

                switch (outcome.positivityIndex)
                {
                    case -1:
                        if (extraEventChance.NextDouble() > 0.9)
                        {
                            farmAnimalsWander = true;
                        }
                        break;
                    case 1:
                        if (extraEventChance.NextDouble() > 0.75)
                        {
                            farmAnimalsWander = true;
                        }
                        break;
                    case 2:
                        if (extraEventChance.NextDouble() > 0.5)
                        {
                            farmAnimalsWander = true;
                        }
                        break;
                }

                if (farmAnimalsWander)
                {
                    IncidentParms parms = StorytellerUtility.DefaultParmsNow(IncidentDefOf.FarmAnimalsWanderIn.category, jobRitual.Map);
                    IncidentDefOf.FarmAnimalsWanderIn.Worker.TryExecute(parms);
                    Find.LetterStack.ReceiveLetter("VFET_AnimalsLetterLabel".Translate(), "VFET_AnimalsLetter".Translate(), LetterDefOf.RitualOutcomePositive, lookTargets, null, null, null, null);

                }

            }


            List<ResearchProjectDef> completedAnimalResearches = (from x in activeAnimalResearches
                                                               where x.ProgressPercent>=1 && x.techLevel == TechLevel.Animal
                                                               select x).ToList();


            List<ResearchProjectDef> completedNeolithicResearches = (from x in activeNeolithicResearches
                                                                  where x.ProgressPercent >= 1 && x.techLevel == TechLevel.Neolithic
                                                                  select x).ToList();

            if (completedAnimalResearches.Count > 0)
            {               
                foreach (ResearchProjectDef project in completedAnimalResearches)
                {
                    Find.LetterStack.ReceiveLetter("VFET_DiscoveryLabel".Translate(project.LabelCap), "VFET_Discovery".Translate(project.LabelCap,project.description), LetterDefOf.NeutralEvent);
                }
            }
            if (completedNeolithicResearches.Count > 0)
            {
                foreach (ResearchProjectDef project in activeNeolithicResearches)
                {
                    Find.LetterStack.ReceiveLetter("VFET_DiscoveryLabel".Translate(project.LabelCap), "VFET_Discovery".Translate(project.LabelCap,project.description), LetterDefOf.NeutralEvent);
                }
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
           
            Find.LetterStack.ReceiveLetter("OutcomeLetterLabel".Translate(outcome.label.Named("OUTCOMELABEL"), jobRitual.Ritual.Label.Named("RITUALLABEL")), text2, outcome.Positive ? LetterDefOf.RitualOutcomePositive : LetterDefOf.RitualOutcomeNegative, lookTargets, null, null, null, null);




        }

        private bool TryFindEntryCell(Map map, out IntVec3 cell)
        {
            return CellFinder.TryFindRandomEdgeCellWith((IntVec3 c) => map.reachability.CanReachColony(c) && !c.Fogged(map), map, CellFinder.EdgeRoadChance_Neutral, out cell);
        }


    }
}

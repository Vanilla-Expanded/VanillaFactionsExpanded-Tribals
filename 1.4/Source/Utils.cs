using RimWorld;
using RimWorld.Planet;
using RimWorld.QuestGen;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Verse;

namespace VFETribals
{
    [StaticConstructorOnStartup]
    public static class Utils
    {
        public static List<WorkTags> workTags = Enum.GetValues(typeof(WorkTags)).Cast<WorkTags>().ToList();
        public static List<TribalResearchProjectDef> researchProjectsWithUnlockWorkTags = DefDatabase<TribalResearchProjectDef>
            .AllDefsListForReading.Where(x => x.unlocksWorkTags != WorkTags.None).ToList();
        public static List<TribalResearchProjectDef> researchProjectsWithDesignators = DefDatabase<TribalResearchProjectDef>
            .AllDefsListForReading.Where(x => x.unlocksDesignators != null).ToList();
        public static HashSet<PreceptDef> advancementPrecepts = new HashSet<PreceptDef>
        {
            VFET_DefOf.VFET_AdvanceToNeolithic,
            VFET_DefOf.VFET_AdvanceToMedieval,
            VFET_DefOf.VFET_AdvanceToIndustrial,
            VFET_DefOf.VFET_AdvanceToSpacer,
            VFET_DefOf.VFET_AdvanceToUltra
        };
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsUnlocked(this WorkTypeDef workTypeDef, out TribalResearchProjectDef research)
        {
            foreach (var tribalResearch in DefDatabase<TribalResearchProjectDef>.AllDefs)
            {
                if (tribalResearch.unlocksWorkTypes?.Contains(workTypeDef) ?? false)
                {
                    if (tribalResearch.IsFinished is false)
                    {
                        research = tribalResearch;
                        return false;
                    }
                }
            }
            research = null;
            return true;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsUnlocked(this WorkTags workTags, out TribalResearchProjectDef research)
        {
            foreach (var tribalResearch in researchProjectsWithUnlockWorkTags)
            {
                if (tribalResearch.unlocksWorkTags.HasFlag(workTags))
                {
                    if (tribalResearch.IsFinished is false)
                    {
                        research = tribalResearch;
                        return false;
                    }
                }
            }
            research = null;
            return true;
        }

        public static QuestPart_WildmenPostExpire WildmenPostExpire(MapParent mapParent, List<Pawn> wildmen, string inSignal = null, 
            QuestPart.SignalListenMode signalListenMode = QuestPart.SignalListenMode.OngoingOnly, Faction faction = null)
        {
            QuestPart_WildmenPostExpire postexpire = new QuestPart_WildmenPostExpire();
            postexpire.inSignal = QuestGenUtility.HardcodedSignalWithQuestID(inSignal) ?? QuestGen.slate.Get<string>("inSignal");
            postexpire.signalListenMode = signalListenMode;
            postexpire.mapParent = mapParent;
            postexpire.Pawns = wildmen;
            postexpire.faction = faction;
            QuestGen.quest.AddPart(postexpire);
            return postexpire;
        }


        public static void MakeWildmenRaid(List<Pawn> wildmen, Map map, TaggedString letterLabel = default, 
            TaggedString letterText = default)
        {
            var dict = new Dictionary<Pawn, int>();
            foreach (var w in wildmen)
            {
                dict.Add(w, 0);
            }
            IncidentParms parms = new IncidentParms
            {
                target = map,
                points = wildmen.Sum(x => x.kindDef.combatPower),
                faction = wildmen.First().Faction,
                pawnGroups = dict,
            };
            var worker = IncidentDefOf.RaidEnemy.Worker as IncidentWorker_RaidEnemy;
            TryGenerateRaidInfo(worker, parms, wildmen);
            if (letterLabel == default)
            {
                letterLabel = worker.GetLetterLabel(parms);
            }
            if (letterText == default)
            {
                letterText = worker.GetLetterText(parms, wildmen);
            }
            PawnRelationUtility.Notify_PawnsSeenByPlayer_Letter(wildmen, ref letterLabel, ref letterText,
                worker.GetRelatedPawnsInfoLetterText(parms), informEvenIfSeenBefore: true);
            List<TargetInfo> list = new List<TargetInfo>();
            if (parms.pawnGroups != null)
            {
                List<List<Pawn>> list2 = IncidentParmsUtility.SplitIntoGroups(wildmen, parms.pawnGroups);
                List<Pawn> list3 = list2.MaxBy((List<Pawn> x) => x.Count);
                if (list3.Any())
                {
                    list.Add(list3[0]);
                }
                for (int i = 0; i < list2.Count; i++)
                {
                    if (list2[i] != list3 && list2[i].Any())
                    {
                        list.Add(list2[i][0]);
                    }
                }
            }
            else if (wildmen.Any())
            {
                foreach (Pawn item in wildmen)
                {
                    list.Add(item);
                }
            }
            worker.SendStandardLetter(letterLabel, letterText, worker.GetLetterDef(), parms, list);
            if (parms.controllerPawn == null || parms.controllerPawn.Faction != Faction.OfPlayer)
            {
                parms.raidStrategy.Worker.MakeLords(parms, wildmen);
            }
        }

        public static void TryGenerateRaidInfo(IncidentWorker_RaidEnemy worker, IncidentParms parms, List<Pawn> pawns)
        {
            parms.raidStrategy = RaidStrategyDefOf.ImmediateAttack;
            parms.raidArrivalMode = PawnsArrivalModeDefOf.EdgeWalkIn;
            worker.ResolveRaidAgeRestriction(parms);
            parms.raidArrivalMode.Worker.TryResolveRaidSpawnCenter(parms);
            worker.GenerateRaidLoot(parms, parms.points, pawns);
            parms.raidArrivalMode.Worker.Arrive(pawns, parms);
        }
    }
}

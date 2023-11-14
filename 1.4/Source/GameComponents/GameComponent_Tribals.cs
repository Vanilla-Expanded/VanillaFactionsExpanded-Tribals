using RimWorld;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;
using Verse.Grammar;

namespace VFETribals
{
    [HotSwappable]
    public class GameComponent_Tribals : GameComponent
    {
        public int availableCornerstonePoints;

        public TechLevel? playerTechLevel;

        public string ethos;
        public bool ethosLocked;

        public List<CornerstoneDef> cornerstones = new List<CornerstoneDef>();

        public int largeFireActiveTicks;
        public int lastLargeFireUpdate;
        public int lastTickResearchFinished;
        private HashSet<ResearchProjectDef> finishedResearchProjects = new HashSet<ResearchProjectDef>();
        public HashSet<ResearchProjectDef> FinishedResearchProjects
        {
            get
            {
                finishedResearchProjects.RemoveWhere(x => x.IsFinished is false);
                return finishedResearchProjects;
            }
        }

        public static GameComponent_Tribals Instance;

        public GameComponent_Tribals()
        {
            Instance = this;
        }

        public GameComponent_Tribals(Game game)
        {
            Instance = this;
        }

        public void IncrementLargeFireCounter(LargeFire largeFire)
        {
            if (Find.TickManager.TicksGame != lastLargeFireUpdate)
            {
                largeFireActiveTicks++;
                lastLargeFireUpdate = Find.TickManager.TicksGame;
                if (largeFireActiveTicks >= GenDate.TicksPerDay * 10)
                {
                    SpawnWildMan(largeFire.Map);
                    ResetLargeFireCounter();
                }
            }
        }

        public void SpawnWildMan(Map map)
        {
            VFET_DefOf.WildManWandersIn.Worker.TryExecute(new IncidentParms
            {
                target = map,
            });
        }

        public void AdvanceToEra(EraAdvancementDef def)
        {
            playerTechLevel = Faction.OfPlayer.def.techLevel = def.newTechLevel;
            OffsetAvailableCornerstonePoints(def.cornerstonePoint);
            TryRegisterAdvancementObligation();
        }

        public void OffsetAvailableCornerstonePoints(int offset)
        {
            availableCornerstonePoints += offset;
        }

        public void AddCornerstone(CornerstoneDef def)
        {
            availableCornerstonePoints--;
            cornerstones.Add(def);
            if (ethosLocked is false)
            {
                ethos = GetNewEthos();
            }
        }

        public void TryRegisterAdvancementObligation()
        {
            foreach (Ideo ideo in Faction.OfPlayer.ideos.AllIdeos)
            {
                for (int i = 0; i < ideo.PreceptsListForReading.Count; i++)
                {
                    Precept_Ritual precept = ideo.PreceptsListForReading[i] as Precept_Ritual;
                    if (precept != null && Utils.advancementPrecepts.Contains(precept.def))
                    {
                        var trigger = precept.obligationTriggers.OfType<RitualObligationTrigger_TargetTechlevel>().First();
                        trigger.RegisterObligation();
                    }
                }
            }
        }

        public override void GameComponentTick()
        {
            base.GameComponentTick();
            if (playerTechLevel.HasValue is false)
            {
                playerTechLevel = Faction.OfPlayer.def.techLevel;
            }
            if (Faction.OfPlayer.def.techLevel > playerTechLevel.Value)
            {
                var diff = (int)(Faction.OfPlayer.def.techLevel - playerTechLevel.Value);
                for (var i = 1; i <= diff; i++)
                {
                    var newTechLevel = (TechLevel)((int)playerTechLevel.Value + i);
                    var eraAdvancementDef = DefDatabase<EraAdvancementDef>.AllDefsListForReading
                        .FirstOrDefault(x => x.newTechLevel == newTechLevel);
                    if (eraAdvancementDef != null)
                    {
                        AdvanceToEra(eraAdvancementDef);
                    }
                }
            }

            if (largeFireActiveTicks != 0 && lastLargeFireUpdate != Find.TickManager.TicksGame)
            {
                ResetLargeFireCounter();
            }
        }

        private void ResetLargeFireCounter()
        {
            lastLargeFireUpdate = largeFireActiveTicks = 0;
        }

        public void Notify_GameStarted()
        {
            if (Faction.OfPlayer.def == VFET_DefOf.VFET_WildMen)
            {
                Faction.OfPlayer.def.techLevel = TechLevel.Animal;
            }
            playerTechLevel = Faction.OfPlayer.def.techLevel;
        }

        public string GetNewEthos()
        {
            var ethos = "";
            foreach (var def in cornerstones)
            {
                ethos += def.blurb.Formatted(GetPlayerFactionName()).Resolve() + " ";
            }
            return ethos;
        }

        public string GetPlayerFactionName()
        {
            if (Faction.OfPlayer.HasName)
            {
                return Faction.OfPlayer.name.ApplyTag(Faction.OfPlayer).Resolve();
            }
            return Faction.OfPlayer.def.LabelCap.ToString().ApplyTag(Faction.OfPlayer).Resolve();
        }

        public override void FinalizeInit()
        {
            base.FinalizeInit();
            if (Faction.OfPlayer.def.techLevel > TechLevel.Animal)
            {
                ResearchAllAnimalProjects();
            }

            if (VFET_DefOf.VFET_Culture.IsFinished is false)
            {
                Find.IdeoManager.classicMode = true;
            }

            TryRegisterAdvancementObligation();
        }

        public static void ResearchAllAnimalProjects()
        {
            foreach (var def in DefDatabase<TribalResearchProjectDef>.AllDefsListForReading
                .Where(x => x.techLevel == TechLevel.Animal))
            {
                if (!def.IsFinished)
                {
                    Find.ResearchManager.FinishProject(def, doCompletionLetter: false);
                }
            }
        }

        public override void ExposeData()
        {
            base.ExposeData();
            Scribe_Values.Look(ref availableCornerstonePoints, "availableCornerstonePoints");
            Scribe_Values.Look(ref playerTechLevel, "playerTechLevel");
            Scribe_Values.Look(ref ethos, "ethos");
            Scribe_Values.Look(ref ethosLocked, "ethosLocked");
            Scribe_Values.Look(ref lastLargeFireUpdate, "lastLargeFireUpdate");
            Scribe_Values.Look(ref largeFireActiveTicks, "largeFireActiveTicks");
            Scribe_Values.Look(ref lastTickResearchFinished, "lastTickResearchFinished");
            Scribe_Collections.Look(ref cornerstones, "cornerstones", LookMode.Def);
            Scribe_Collections.Look(ref finishedResearchProjects, "finishedResearchProjects", LookMode.Def);
            if (Scribe.mode == LoadSaveMode.PostLoadInit)
            {
                cornerstones ??= new List<CornerstoneDef>();
                finishedResearchProjects ??= new HashSet<ResearchProjectDef>();
                if (playerTechLevel.HasValue)
                {
                    Faction.OfPlayer.def.techLevel = playerTechLevel.Value;
                }
            }
        }
    }
}

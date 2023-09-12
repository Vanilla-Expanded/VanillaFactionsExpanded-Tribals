using RimWorld;
using System.Collections.Generic;
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

        public List<LargeFire> largeFires = new List<LargeFire>();
        public string ethos;
        public bool ethosLocked;

        public List<CornerstoneDef> cornerstones = new List<CornerstoneDef>();

        public static GameComponent_Tribals Instance;

        public GameComponent_Tribals()
        {
            Instance = this;
        }

        public GameComponent_Tribals(Game game)
        {
            Instance = this;
        }

        public void AdvanceToEra(EraAdvancementDef def)
        {
            playerTechLevel = Faction.OfPlayer.def.techLevel = def.newTechLevel;
            OffsetAvailableCornerstonePoints(def.cornerstonePoint);
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

        public override void GameComponentTick()
        {
            base.GameComponentTick();
            if (playerTechLevel.HasValue)
            {
                if (Faction.OfPlayer.def.techLevel > playerTechLevel.Value)
                {
                    var diff = (int)(Faction.OfPlayer.def.techLevel - playerTechLevel.Value);
                    for (var i = 0; i < diff; i++)
                    {
                        var newTechLevel = (TechLevel)((int)playerTechLevel.Value + i);
                        var eraAdvancementDef = DefDatabase<EraAdvancementDef>.AllDefsListForReading
                            .FirstOrDefault(x => x.newTechLevel  == newTechLevel);
                        AdvanceToEra(eraAdvancementDef);
                    }
                }
            }
            else
            {
                playerTechLevel = Faction.OfPlayer.def.techLevel;
            }
        }

        public override void StartedNewGame()
        {
            base.StartedNewGame();
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
                foreach (var def in DefDatabase<TribalResearchProjectDef>.AllDefsListForReading)
                {
                    if (!def.IsFinished)
                    {
                        Find.ResearchManager.FinishProject(def, doCompletionLetter: false);
                    }
                }
            }

            if (VFET_DefOf.VFET_Culture.IsFinished is false)
            {
                Find.IdeoManager.classicMode = true;
            }
        }

        public override void ExposeData()
        {
            base.ExposeData();
            Scribe_Values.Look(ref availableCornerstonePoints, "availableCornerstonePoints");
            Scribe_Values.Look(ref playerTechLevel, "playerTechLevel");
            Scribe_Values.Look(ref ethos, "ethos");
            Scribe_Values.Look(ref ethosLocked, "ethosLocked");
            Scribe_Collections.Look(ref cornerstones, "cornerstones", LookMode.Def);
            if (Scribe.mode == LoadSaveMode.PostLoadInit)
            {
                cornerstones ??= new List<CornerstoneDef>();
            }
        }
    }
}

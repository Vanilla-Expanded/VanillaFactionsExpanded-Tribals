using RimWorld;
using System.Collections.Generic;
using Verse;

namespace VFETribals
{
    [HotSwappable]
    public class GameComponent_Tribals : GameComponent
    {
        public int availableCornerstonePoints;

        public TechLevel? playerTechLevel;

        public string ethos;
        public bool ethosLocked;

        public List<CornerstoneDef> cornerstones;

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
            return "";
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

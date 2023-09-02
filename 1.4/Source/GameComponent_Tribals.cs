using RimWorld;
using Verse;

namespace VFETribals
{
    [HotSwappable]
    public class GameComponent_Tribals : GameComponent
    {
        public int availableCornerstonePoints;

        public TechLevel? playerTechLevel;

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

        public override void FinalizeInit()
        {
            base.FinalizeInit();
            if (playerTechLevel.HasValue)
            {
                Faction.OfPlayer.def.techLevel = playerTechLevel.Value;
            }
        }

        public override void StartedNewGame()
        {
            base.StartedNewGame();
            playerTechLevel = Faction.OfPlayer.def.techLevel;
        }

        public override void ExposeData()
        {
            base.ExposeData();
            Scribe_Values.Look(ref availableCornerstonePoints, "availableCornerstonePoints");
        }
    }
}

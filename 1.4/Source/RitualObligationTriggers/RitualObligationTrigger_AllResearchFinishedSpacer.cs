
using RimWorld;
using System.Collections.Generic;
using System.Linq;
using Verse;
using static HarmonyLib.Code;

namespace VFETribals
{
    public class RitualObligationTrigger_AllResearchFinishedSpacer : RitualObligationTrigger
    {
        public const int counterPeriod = 1250;
        public int tickCounter = counterPeriod;

        public override void ExposeData()
        {
            base.ExposeData();
            Scribe_Values.Look(ref this.tickCounter, nameof(this.tickCounter));
        }

        public override void Tick()
        {
            base.Tick();
            tickCounter++;
            if (tickCounter > counterPeriod) // Half an hour ingame
            {
                if (GameComponent_Tribals.Instance.allSpacerResearchCompleted && Faction.OfPlayer.def.techLevel == TechLevel.Spacer)
                {
                    ritual.AddObligation(new RitualObligation(ritual));
                }

                tickCounter = 0;
            }

        }





    }
}
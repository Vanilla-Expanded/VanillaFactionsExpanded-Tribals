
using RimWorld;
using System.Collections.Generic;
using System.Linq;
using Verse;
using static HarmonyLib.Code;

namespace VFETribals
{
    public class RitualObligationTrigger_AllResearchFinishedAnimal : RitualObligationTrigger
    {
        public int tickCounter = 30000;
        public const int counterPeriod = 30000;

        public override void ExposeData()
        {
            base.ExposeData();
            Scribe_Values.Look(ref this.tickCounter, nameof(this.tickCounter));
        }

        public override void Tick()
        {
            base.Tick();
            tickCounter++;
            if (tickCounter > 30000)
            {
                List<ResearchProjectDef> activeAnimalResearches = (from x in DefDatabase<ResearchProjectDef>.AllDefsListForReading
                                                                   where x.CanStartNow && x.techLevel == TechLevel.Animal
                                                                   select x).ToList();

                if (activeAnimalResearches.Count == 0)
                {
                    ritual.AddObligation(new RitualObligation(ritual));
                }
                tickCounter = 0;
            }
           
        }



       

    }
}
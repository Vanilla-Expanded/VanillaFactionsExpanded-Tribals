
using RimWorld;
using System.Collections.Generic;
using Verse;

namespace VFETribals
{
    public class RitualObligationTrigger_TargetTechlevel : RitualObligationTrigger
    {
        public TechLevel targetTechLevel;

        public override void Init(RitualObligationTriggerProperties props)
        {
            this.targetTechLevel = (props as RitualObligationTrigger_TargetTechlevel_Props).targetTechLevel;
            base.Init(props);
        }

        public void RegisterObligation()
        {
            if (Faction.OfPlayer.def.techLevel + 1 == targetTechLevel)
            {
                var obligation = new RitualObligation(ritual, expires: false);
                ritual.AddObligation(obligation);
            }
        }

        public override void ExposeData()
        {
            base.ExposeData();
            Scribe_Values.Look(ref targetTechLevel, "targetTechLevel");
        }
    }
}
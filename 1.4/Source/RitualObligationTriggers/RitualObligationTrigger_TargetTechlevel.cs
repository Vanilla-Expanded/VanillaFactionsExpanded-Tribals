
using RimWorld;
using System.Collections.Generic;
using System.Linq;
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

        public override void CopyTo(RitualObligationTrigger other)
        {
            base.CopyTo(other);
            this.targetTechLevel = (other as RitualObligationTrigger_TargetTechlevel).targetTechLevel;
        }

        public void RegisterObligation()
        {
            if (targetTechLevel == TechLevel.Undefined)
            {
                var props = this.ritual?.sourcePattern?.ritualObligationTriggers?
                            .OfType<RitualObligationTrigger_TargetTechlevel_Props>().FirstOrDefault();
                if (props != null)
                {
                    targetTechLevel = props.targetTechLevel;
                }
            }
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
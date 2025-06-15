using RimWorld;
namespace VFETribals
{
    public class RitualObligationTrigger_TargetTechlevel_Props : RitualObligationTriggerProperties
    {
        public TechLevel targetTechLevel;
        public RitualObligationTrigger_TargetTechlevel_Props()
        {
            triggerClass = typeof(RitualObligationTrigger_TargetTechlevel);
        }
    }
}
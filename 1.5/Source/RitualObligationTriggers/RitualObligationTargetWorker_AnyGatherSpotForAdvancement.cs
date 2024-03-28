
using HarmonyLib;
using RimWorld;
using System.Linq;
using Verse;

namespace VFETribals
{
    public class RitualObligationTargetWorker_AnyGatherSpotForAdvancement : RitualObligationTargetWorker_AnyGatherSpot
    {
        public RitualObligationTargetWorker_AnyGatherSpotForAdvancement()
        {
        }

        public RitualObligationTargetWorker_AnyGatherSpotForAdvancement(RitualObligationTargetFilterDef def)
            : base(def)
        {
        }

        public override RitualTargetUseReport CanUseTargetInternal(TargetInfo target, RitualObligation obligation)
        {
            if (obligation?.precept != null)
            {
                obligation.precept.isAnytime = true;
            }
            var result = base.CanUseTargetInternal(target, obligation);
            if (result.ShouldShowGizmo)
            {
                var trigger = parent.obligationTriggers.OfType<RitualObligationTrigger_TargetTechlevel>().First();
                if (trigger != null) 
                {
                    if (trigger.targetTechLevel == TechLevel.Undefined)
                    {
                        var props = obligation.precept.sourcePattern.ritualObligationTriggers?
                            .OfType<RitualObligationTrigger_TargetTechlevel_Props>().FirstOrDefault();
                        if (props != null)
                        {
                            trigger.targetTechLevel = props.targetTechLevel;
                        }
                    }
                    if (trigger.targetTechLevel != Faction.OfPlayer.def.techLevel + 1)
                    {
                        result.canUse = false;
                        result.failReason = null;
                        return result;
                    }
                }
                if (DefDatabase<ResearchProjectDef>.AllDefsListForReading
                        .Where(x => x.techLevel == Faction.OfPlayer.def.techLevel
                        && x.techprintCount == 0 && x.CanStartNow).Any())
                {
                    return "VFET.NotAllReseachProjectsResearched".Translate();
                }
            }
            return result;
        }
    }
}

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
            var result = base.CanUseTargetInternal(target, obligation);
            if (result.ShouldShowGizmo)
            {
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
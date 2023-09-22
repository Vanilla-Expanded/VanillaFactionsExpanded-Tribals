
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
            if (DefDatabase<ResearchProjectDef>.AllDefsListForReading
    .Where(x => x.CanStartNow && x.techLevel == Faction.OfPlayer.def.techLevel
    && x.techprintCount == 0).Any())
            {
                return "VFET.NotAllReseachProjectsResearched".Translate();
            }
            return base.CanUseTargetInternal(target, obligation);
        }
    }
}
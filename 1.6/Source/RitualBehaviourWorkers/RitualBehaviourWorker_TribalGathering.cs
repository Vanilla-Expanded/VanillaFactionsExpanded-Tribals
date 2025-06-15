using System;
using Verse;
using RimWorld;
using System.Collections.Generic;
using System.Linq;
using Verse.AI;
using Verse.Sound;


namespace VFETribals
{
    public class RitualBehaviourWorker_TribalGathering : RitualBehaviorWorker
    {
        private Sustainer soundPlaying;
        public RitualBehaviourWorker_TribalGathering()
        {
        }

        public RitualBehaviourWorker_TribalGathering(RitualBehaviorDef def) : base(def)
        {
        }


       
        public override void Tick(LordJob_Ritual ritual)
        {
            base.Tick(ritual);
            if (ritual.StageIndex == 1)
            {
                if (ritual.PawnWithRole("organizer").IsHashIntervalTick(240))
                {
                    VFET_DefOf.VFET_RitualSounds_TribalGathering.PlayOneShot(new TargetInfo(ritual.selectedTarget.Cell, ritual.selectedTarget.Map));
                }

                if (this.soundPlaying == null || this.soundPlaying.Ended)
                {
                    TargetInfo selectedTarget = ritual.selectedTarget;
                    this.soundPlaying = VFET_DefOf.VFET_RitualSustainer_TribalGathering.TrySpawnSustainer(SoundInfo.InMap(new TargetInfo(selectedTarget.Cell, selectedTarget.Map, false), MaintenanceType.PerTick));
                }
                Sustainer sustainer = this.soundPlaying;
                if (sustainer == null)
                {
                    return;
                }
                sustainer.Maintain();
            }
        }
    }
}

using System;
using Verse;
using RimWorld;
using System.Collections.Generic;
using System.Linq;
using Verse.AI;
using Verse.Sound;


namespace VFETribals
{
    public class RitualBehaviourWorker_SpacerGathering : RitualBehaviorWorker
    {
        private Sustainer soundPlaying;
        public RitualBehaviourWorker_SpacerGathering()
        {
        }

        public RitualBehaviourWorker_SpacerGathering(RitualBehaviorDef def) : base(def)
        {
        }



        public override void Tick(LordJob_Ritual ritual)
        {
            base.Tick(ritual);
            if (ritual.StageIndex == 1)
            {


                if (this.soundPlaying == null || this.soundPlaying.Ended)
                {
                    TargetInfo selectedTarget = ritual.selectedTarget;
                    this.soundPlaying = VFET_DefOf.VFET_RitualSustainer_SpacerGathering.TrySpawnSustainer(SoundInfo.InMap(new TargetInfo(selectedTarget.Cell, selectedTarget.Map, false), MaintenanceType.PerTick));
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

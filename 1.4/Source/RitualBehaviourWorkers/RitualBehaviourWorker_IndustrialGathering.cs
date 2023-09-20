using System;
using Verse;
using RimWorld;
using System.Collections.Generic;
using System.Linq;
using Verse.AI;
using Verse.Sound;
using static TMPro.SpriteAssetUtilities.TexturePacker_JsonArray;
using Verse.Noise;

namespace VFETribals
{
    public class RitualBehaviourWorker_IndustrialGathering : RitualBehaviorWorker
    {
        private Sustainer soundPlaying;
        public RitualBehaviourWorker_IndustrialGathering()
        {
        }

        public RitualBehaviourWorker_IndustrialGathering(RitualBehaviorDef def) : base(def)
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
                    this.soundPlaying = VFET_DefOf.VFET_RitualSustainer_IndustrialGathering.TrySpawnSustainer(SoundInfo.InMap(new TargetInfo(selectedTarget.Cell, selectedTarget.Map, false), MaintenanceType.PerTick));
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

using Verse;

namespace VFETribals
{
    public class CompHeatPusher_LargeFire : CompHeatPusherPowered
    {
        public override bool ShouldPushHeatNow => base.ShouldPushHeatNow && (this.parent as LargeFire).lightOn;
    }
}

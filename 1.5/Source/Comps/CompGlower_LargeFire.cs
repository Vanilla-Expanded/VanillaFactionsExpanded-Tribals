using Verse;

namespace VFETribals
{
    public class CompGlower_LargeFire : CompGlower
    {
        public override bool ShouldBeLitNow => base.ShouldBeLitNow && (this.parent as LargeFire).lightOn;
    }
}

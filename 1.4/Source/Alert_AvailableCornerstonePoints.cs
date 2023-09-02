using RimWorld;
using Verse;

namespace VFETribals
{
    public class Alert_AvailableCornerstonePoints : Alert
    {
        public override string GetLabel()
        {
            return "VFET.AvailableCornerstonePoint".Translate();
        }

        public override TaggedString GetExplanation()
        {
            return "VFET.AvailableCornerstonePointDesc".Translate();
        }
        public override AlertReport GetReport()
        {
            return GameComponent_Tribals.Instance.availableCornerstonePoints > 0;
        }

        public override void OnClick()
        {
            base.OnClick();
            Find.WindowStack.Add(new Window_CustomizeCornerStones());
        }
    }
}

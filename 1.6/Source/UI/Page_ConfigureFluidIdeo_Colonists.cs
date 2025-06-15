using RimWorld;
using System.Linq;
using Verse;

namespace VFETribals
{
    public class Page_ConfigureFluidIdeo_Colonists : Page_ConfigureFluidIdeo
    {
        public ChoiceLetter_ConfigureIdeo letter;

        public override bool CanDoNext()
        {
            Page_ConfigureIdeo_Colonists.ConfigurePlayerIdeo(ideo);
            return true;
        }

        public override void DoNext()
        {
            letter.Close();
            this.Close();
            Find.FactionManager.OfPlayer.ideos.RecalculateIdeosBasedOnPlayerPawns();
        }
    }
}

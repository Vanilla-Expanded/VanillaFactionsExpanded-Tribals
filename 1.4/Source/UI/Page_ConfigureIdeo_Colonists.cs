using RimWorld;
using System.Linq;
using Verse;

namespace VFETribals
{
    public class Page_ConfigureIdeo_Colonists : Page_ConfigureIdeo
    {
        public ChoiceLetter_ConfigureIdeo letter;

        public override bool CanDoNext()
        {
            Find.IdeoManager.classicMode = false;
            Faction.OfPlayer.ideos.SetPrimary(ideo);
            foreach (Ideo item in Find.IdeoManager.IdeosListForReading)
            {
                item.initialPlayerIdeo = false;
            }
            ideo.initialPlayerIdeo = true;
            var colonists = PawnsFinder.AllMapsCaravansAndTravelingTransportPods_Alive_Colonists;
            foreach (var c in colonists)
            {
                if (c.Ideo != ideo)
                {
                    c.ideo.SetIdeo(ideo);
                }
            }
            Find.IdeoManager.RemoveUnusedStartingIdeos();
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

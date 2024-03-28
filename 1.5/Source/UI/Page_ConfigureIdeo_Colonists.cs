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
            ConfigurePlayerIdeo(ideo);
            return true;
        }

        public static void ConfigurePlayerIdeo(Ideo ideo)
        {
            Find.IdeoManager.classicMode = false;
            var prevIdeos = Find.IdeoManager.IdeosListForReading.ToList();
            Faction.OfPlayer.ideos.RemoveAll();
            foreach (var prevIdeo in prevIdeos)
            {
                if (prevIdeo.initialPlayerIdeo && ideo != prevIdeo)
                {
                    Find.IdeoManager.Remove(prevIdeo);
                }
            }
            Faction.OfPlayer.ideos.SetPrimary(ideo);
            ideo.initialPlayerIdeo = true;
            var colonists = PawnsFinder.AllMapsCaravansAndTravelingTransportPods_Alive_Colonists.ToList();
            foreach (var c in colonists)
            {
                if (c.Ideo != ideo)
                {
                    c.ideo.SetIdeo(ideo);
                }
            }
            Find.IdeoManager.RemoveUnusedStartingIdeos();
        }

        public override void DoNext()
        {
            letter.Close();
            this.Close();
            Find.FactionManager.OfPlayer.ideos.RecalculateIdeosBasedOnPlayerPawns();
        }
    }
}

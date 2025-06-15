using RimWorld;
using Verse;

namespace VFETribals
{
    public class PlaceWorker_OnFloorsOrStoneTerrains : PlaceWorker
    {
        public override AcceptanceReport AllowsPlacing(BuildableDef checkingDef, IntVec3 loc, Rot4 rot, Map map, Thing thingToIgnore = null, Thing thing = null)
        {
            var terrain = loc.GetTerrain(map);
            if (terrain.IsSoil || terrain.IsFloor is false && terrain.defName.Contains("_Rough") is false 
                && terrain.defName.Contains("_Smooth") is false)
            {
                return "VFET.MustPlaceOnFloorOrStonyTerrain".Translate();
            }
            return true;
        }
    }
}

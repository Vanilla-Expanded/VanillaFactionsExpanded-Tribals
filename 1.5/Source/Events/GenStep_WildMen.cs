using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace VFETribals
{
    public class GenStep_WildMen : GenStep
    {
        public override int SeedPart => 465168475;

        public override void Generate(Map map, GenStepParams parms)
        {
            if (!RCellFinder.TryFindRandomCellNearTheCenterOfTheMapWith(delegate (IntVec3 x)
            {
                return GenRadial.RadialDistinctThingsAround(x, map, 6, true).Any(x => x is Building) is false;

            }, map, out var result))
            {
                RCellFinder.TryFindRandomCellNearTheCenterOfTheMapWith(delegate (IntVec3 x)
                {
                    return x.Standable(map);
                }, map, out result);
            }

            GenSpawn.Spawn(ThingDefOf.Campfire, result, map);
            Faction faction = null;
            foreach (var wildman in parms.sitePart.things.ToList())
            {
                faction = wildman.Faction;
                if (RCellFinder.TryFindRandomCellNearWith(result, delegate (IntVec3 x)
                {
                    return x.Standable(map) && GenSight.LineOfSight(x, result, map);
                }, map, out var cell))
                {
                    GenSpawn.Spawn(wildman, cell, map);
                }
            }
            if (faction.HostileTo(Faction.OfPlayer) is false)
            {
                faction.SetRelationDirect(Faction.OfPlayer, FactionRelationKind.Hostile, false);
            }
        }
    }

}

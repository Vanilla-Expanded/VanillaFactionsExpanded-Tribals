using HarmonyLib;
using RimWorld;
using RimWorld.Planet;
using System.Linq;
using Verse;
using Verse.AI.Group;

namespace VFETribals
{
    [HarmonyPatch(typeof(MapGenerator), nameof(MapGenerator.GenerateMap))]
    public static class MapGenerator_GenerateMap_Patch
    {
        public static void Postfix(Map __result)
        {
            LongEventHandler.ExecuteWhenFinished(() =>
            {
                if (__result.Parent is Site site)
                {
                    var sitepart = site.MainSitePart;
                    if (sitepart?.def == VFET_DefOf.VFET_WildMenSite)
                    {
                        var colonists = __result.mapPawns.FreeColonistsSpawned;
                        var wildmen = __result.mapPawns.FreeHumanlikesSpawnedOfFaction(site.Faction);
                        if (colonists.Count >= wildmen.Count + 5)
                        {
                            foreach (var wildman in wildmen)
                            {
                                wildman.SetFaction(Faction.OfPlayer);
                            }
                            Messages.Message("VFET.WildMenJoin".Translate(), wildmen, MessageTypeDefOf.PositiveEvent);
                        }
                        else if (colonists.Count > wildmen.Count)
                        {
                            LordMaker.MakeNewLord(site.Faction, new LordJob_Flee(), __result, wildmen);
                            site.Faction.SetRelation(new FactionRelation(Faction.OfPlayer, FactionRelationKind.Hostile));
                            Messages.Message("VFET.WildMenRetreat".Translate(), wildmen, MessageTypeDefOf.PositiveEvent);
                        }
                        else
                        {
                            site.Faction.SetRelation(new FactionRelation(Faction.OfPlayer, FactionRelationKind.Hostile));
                            LordMaker.MakeNewLord(site.Faction, new LordJob_AssaultThings(site.Faction, colonists.Cast<Thing>().ToList()),
                                __result, wildmen);
                        }
                    }
                }
            });
        }
    }

}

using RimWorld;
using RimWorld.Planet;
using RimWorld.QuestGen;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;
using Verse.Grammar;

namespace VFETribals
{
    public class QuestNode_Root_WildMen : QuestNode
    {
        public static Site GenerateSite(float points, int aroundTile)
        {
            List<FactionRelation> list = new List<FactionRelation>();
            foreach (Faction item2 in Find.FactionManager.AllFactionsListForReading)
            {
                if (item2 == Faction.OfPlayer)
                {
                    list.Add(new FactionRelation
                    {
                        other = item2,
                        kind = FactionRelationKind.Hostile
                    });
                }
                else if (!item2.def.permanentEnemy)
                {
                    list.Add(new FactionRelation
                    {
                        other = item2,
                        kind = FactionRelationKind.Neutral
                    });
                }
            }
            FactionGeneratorParms parms = new FactionGeneratorParms(VFET_DefOf.VFET_WildMenGroup, default, true);
            parms.ideoGenerationParms = new IdeoGenerationParms(parms.factionDef);
            var faction = FactionGenerator.NewGeneratedFactionWithRelations(parms, list);
            faction.temporary = true;
            Find.FactionManager.Add(faction);
            Site site = QuestGen_Sites.GenerateSite(new SitePartDefWithParams[1]
            {
                new SitePartDefWithParams(VFET_DefOf.VFET_WildMenSite, new SitePartParams
                {
                    threatPoints = points
                })
            }, PotentialSiteTiles(aroundTile).RandomElement(), faction);
            site.desiredThreatPoints = site.ActualThreatPoints;
            return site;
        }

        public static List<PlanetTile> PotentialSiteTiles(PlanetTile root)
        {
            List<PlanetTile> tiles = new List<PlanetTile>();
            root.Layer.Filler.FloodFill(root, (PlanetTile p) => !Find.World.Impassable(p) && Find.WorldGrid.ApproxDistanceInTiles(p, root) <= 9f, delegate (PlanetTile p)
            {
                if (Find.WorldGrid.ApproxDistanceInTiles(p, root) >= 3f)
                {
                    tiles.Add(p);
                }
            });
            return tiles;
        }
        public override void RunInt()
        {
            Quest quest = QuestGen.quest;
            Slate slate = QuestGen.slate;
            float points = StorytellerUtility.DefaultSiteThreatPointsNow();
            points *= new FloatRange(0.8f, 1.2f).RandomInRange;
            if (points < 60)
            {
                points = 60;
            }
            slate.Set("points", points);
            Map map = slate.Get<Map>("map");
            Site site = GenerateSite(points, map.Tile);
            quest.SpawnWorldObject(site);
            quest.ReserveFaction(site.Faction);
            int num2 = (int)(GenDate.TicksPerDay * 30);
            quest.WorldObjectTimeout(site, num2);

            PawnGroupMakerParms pawnGroupMakerParms = new PawnGroupMakerParms();
            pawnGroupMakerParms.faction = site.Faction;
            pawnGroupMakerParms.groupKind = PawnGroupKindDefOf.Combat;
            pawnGroupMakerParms.points = points;
            List<Pawn> wildmen = PawnGroupMakerUtility.GeneratePawns(pawnGroupMakerParms).ToList();
            quest.Delay(num2, delegate
            {
                Utils.WildmenPostExpire(map.Parent, wildmen, faction: site.Faction);
            });
            SitePart sitePart = site.parts[0];
            sitePart.things = new ThingOwner<Thing>(sitePart);
            sitePart.things.TryAddRangeOrTransfer(wildmen);
            slate.Set("campSite", site);
            slate.Set("faction", site.Faction);
            slate.Set("timeout", num2);
            string inSignal = QuestGenUtility.HardcodedSignalWithQuestID("campSite.AllEnemiesDefeated");
            string inSignalEnabled = QuestGenUtility.HardcodedSignalWithQuestID("campSite.MapGenerated");
            string inSignal2 = QuestGenUtility.HardcodedSignalWithQuestID("campSite.MapRemoved");
            quest.Notify_PlayerRaidedSomeone(null, site, inSignal);
            quest.End(QuestEndOutcome.Success, 0, null, inSignal2);
            QuestGen.AddQuestDescriptionRules(new List<Rule>
            {
                new Rule_String("ListofPawns", PawnUtility.PawnKindsToLineList(wildmen.Select(x => x.kindDef), "",
                ColoredText.FactionColor_Neutral))
            });
        }

        public override bool TestRunInt(Slate slate)
        {
            return Faction.OfPlayer.def.techLevel == TechLevel.Animal;
        }
    }
}

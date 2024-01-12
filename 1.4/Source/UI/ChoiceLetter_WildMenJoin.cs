using RimWorld;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace VFETribals
{
    public class ChoiceLetter_WildMenJoin : ChoiceLetter
    {
        public List<Pawn> wildmen;
        public Map map;

        public override bool CanDismissWithRightClick => false;

        public override IEnumerable<DiaOption> Choices
        {
            get
            {
                yield return new DiaOption("Accept".Translate())
                {
                    action = () =>
                    {
                        foreach (var pawn in wildmen)
                        {
                            pawn.SetFaction(Faction.OfPlayer);
                        }
                        IncidentParms parms = new IncidentParms
                        {
                            target = map
                        };
                        PawnsArrivalModeDef edgeWalkIn = PawnsArrivalModeDefOf.EdgeWalkIn;
                        edgeWalkIn.Worker.TryResolveRaidSpawnCenter(parms);
                        edgeWalkIn.Worker.Arrive(wildmen, parms);
                        Close();
                        this.quest.End(QuestEndOutcome.Success, false);
                    },
                    resolveTree = true
                };
                yield return new DiaOption("VFET.Reject".Translate())
                {
                    action = () =>
                    {
                        if (Rand.Chance(0.2f))
                        {
                            Utils.MakeWildmenRaid(wildmen, map);
                            this.quest.End(QuestEndOutcome.Fail, false);
                        }
                        else
                        {
                            this.quest.End(QuestEndOutcome.Success, false);
                        }
                        Close();
                    },
                    resolveTree = true
                };
            }
        }

        public void Close()
        {
            Find.Archive.Remove(this);
            Find.LetterStack.RemoveLetter(this);
        }

        public override void ExposeData()
        {
            base.ExposeData();
        
            Scribe_References.Look(ref map, "map");
            Scribe_Collections.Look(ref wildmen, "wildmen", LookMode.Reference);
        }
    }
}

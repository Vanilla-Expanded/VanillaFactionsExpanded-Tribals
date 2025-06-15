using RimWorld;
using RimWorld.Planet;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace VFETribals
{
    public class QuestPart_WildmenPostExpire : QuestPart
    {
        public string inSignal;
        public MapParent mapParent;
        private List<Pawn> wildmen = new List<Pawn>();
        public Faction faction;
        public override bool IncreasesPopulation => false;

        public IEnumerable<Pawn> Pawns
        {
            get
            {
                return wildmen;
            }
            set
            {
                wildmen.Clear();
                if (value == null)
                {
                    return;
                }
                foreach (Pawn pawn in value)
                {
                    if (pawn.Destroyed)
                    {
                        Log.Error("Tried to add a destroyed thing to QuestPart_WildmenPostExpire: " + pawn.ToStringSafe());
                        continue;
                    }
                    wildmen.Add(pawn);
                }
            }
        }

        public override IEnumerable<GlobalTargetInfo> QuestLookTargets
        {
            get
            {
                foreach (GlobalTargetInfo questLookTarget in base.QuestLookTargets)
                {
                    yield return questLookTarget;
                }
                if (mapParent != null)
                {
                    yield return mapParent;
                }
                foreach (Pawn questLookTarget2 in PawnsArriveQuestPartUtility.GetQuestLookTargets(wildmen))
                {
                    yield return questLookTarget2;
                }
            }
        }

        public override void Notify_QuestSignalReceived(Signal signal)
        {
            base.Notify_QuestSignalReceived(signal);
            if (!(signal.tag == inSignal) || mapParent == null || !mapParent.HasMap)
            {
                return;
            }
            Map map = mapParent.Map;
            wildmen.RemoveAll((Pawn x) => x.Destroyed);
            var colonistCount = PawnsFinder.AllMapsCaravansAndTravellingTransporters_Alive_Colonists.Count;
            if (colonistCount >= wildmen.Count + 5 && wildmen.Any())
            {
                var letter = LetterMaker.MakeLetter("VFET.WildMenJoinTitle".Translate(),
                        "VFET.WildMenJoinDesc".Translate(map.Parent.Label), VFET_DefOf.VFET_WildMenJoin, 
                        this.wildmen.First().Faction, this.quest) as ChoiceLetter_WildMenJoin;
                letter.wildmen = wildmen;
                letter.map = map;
                Find.LetterStack.ReceiveLetter(letter);
            }
            else
            {
                if (wildmen.Any())
                {
                    Utils.MakeWildmenRaid(wildmen, map, "VFET.WildMenRaid".Translate(), "VFET.WildMenRaidDesc".Translate(map.Parent.Label));
                }
                this.quest.End(QuestEndOutcome.Fail, false);
            }
        }

        public override bool QuestPartReserves(Pawn p)
        {
            return wildmen.Contains(p);
        }

        public override void ReplacePawnReferences(Pawn replace, Pawn with)
        {
            wildmen.Replace(replace, with);
        }

        public override void Notify_FactionRemoved(Faction f)
        {
            if (faction == f)
            {
                faction = null;
            }
        }

        public override void ExposeData()
        {
            base.ExposeData();
            Scribe_Values.Look(ref inSignal, "inSignal");
            Scribe_References.Look(ref mapParent, "mapParent");
            Scribe_Collections.Look(ref wildmen, "wildmen", LookMode.Reference);
            Scribe_References.Look(ref faction, "faction");
            if (Scribe.mode == LoadSaveMode.PostLoadInit)
            {
                wildmen.RemoveAll((Pawn x) => x == null);
            }
        }
    }
}


using System.Collections.Generic;
using System.Linq;
using RimWorld;
using Verse;

namespace VFETribals
{
    public class RitualOutcomeEffectWorker_AdvanceToSpacer : RitualOutcomeEffectWorker
    {
        public override bool SupportsAttachableOutcomeEffect => false;

        public RitualOutcomeEffectWorker_AdvanceToSpacer()
        {
        }

        public RitualOutcomeEffectWorker_AdvanceToSpacer(RitualOutcomeEffectDef def)
            : base(def)
        {
        }

        public override void Apply(float progress, Dictionary<Pawn, int> totalPresence, LordJob_Ritual jobRitual)
        {
            GameComponent_Tribals comp = Current.Game.GetComponent<GameComponent_Tribals>();
            LookTargets lookTargets = jobRitual.selectedTarget;

            if (comp != null)
            {
                comp.AdvanceToEra(VFET_DefOf.VFET_FormCollective);
                Find.LetterStack.ReceiveLetter(VFET_DefOf.VFET_FormCollective.label, VFET_DefOf.VFET_FormCollective.description, LetterDefOf.RitualOutcomePositive, lookTargets, null, null, null, null);
                jobRitual.ritual.completedObligations ??= new List<RitualObligation>();
                jobRitual.ritual.RemoveObligation(jobRitual.obligation, completed: true);
                jobRitual.ritual.activeObligations.Clear();
            }


        }

       

        
    }
}
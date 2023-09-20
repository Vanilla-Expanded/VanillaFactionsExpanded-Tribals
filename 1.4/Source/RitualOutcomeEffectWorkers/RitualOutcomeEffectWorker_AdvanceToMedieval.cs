
using System.Collections.Generic;
using System.Linq;
using RimWorld;
using Verse;

namespace VFETribals
{
    public class RitualOutcomeEffectWorker_AdvanceToMedieval : RitualOutcomeEffectWorker
    {
        public override bool SupportsAttachableOutcomeEffect => false;

        public RitualOutcomeEffectWorker_AdvanceToMedieval()
        {
        }

        public RitualOutcomeEffectWorker_AdvanceToMedieval(RitualOutcomeEffectDef def)
            : base(def)
        {
        }

        public override void Apply(float progress, Dictionary<Pawn, int> totalPresence, LordJob_Ritual jobRitual)
        {
            GameComponent_Tribals comp = Current.Game.GetComponent<GameComponent_Tribals>();
            LookTargets lookTargets = jobRitual.selectedTarget;

            if (comp != null)
            {
                comp.AdvanceToEra(VFET_DefOf.VFET_FormTown);
                Find.LetterStack.ReceiveLetter(VFET_DefOf.VFET_FormTown.label, VFET_DefOf.VFET_FormTown.description, LetterDefOf.RitualOutcomePositive, lookTargets, null, null, null, null);
                GameComponent_Tribals.Instance.allNeolithicResearchCompleted = false;
                jobRitual.ritual.RemoveObligation(jobRitual.obligation, completed: true);
                jobRitual.ritual.activeObligations.Clear();
            }


        }

       

        
    }
}
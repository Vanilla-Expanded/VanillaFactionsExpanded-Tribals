
using System.Collections.Generic;
using System.Linq;
using RimWorld;
using Verse;

namespace VFETribals
{
    public class RitualOutcomeEffectWorker_AdvanceToNeolithic : RitualOutcomeEffectWorker
    {
        public override bool SupportsAttachableOutcomeEffect => false;

        public RitualOutcomeEffectWorker_AdvanceToNeolithic()
        {
        }

        public RitualOutcomeEffectWorker_AdvanceToNeolithic(RitualOutcomeEffectDef def)
            : base(def)
        {
        }

        public override void Apply(float progress, Dictionary<Pawn, int> totalPresence, LordJob_Ritual jobRitual)
        {
            GameComponent_Tribals comp = Current.Game.GetComponent<GameComponent_Tribals>();
            LookTargets lookTargets = jobRitual.selectedTarget;

            if (comp != null)
            {
                comp.AdvanceToEra(VFET_DefOf.VFET_FormTribe);
                Find.LetterStack.ReceiveLetter(VFET_DefOf.VFET_FormTribe.label, VFET_DefOf.VFET_FormTribe.description, LetterDefOf.RitualOutcomePositive, lookTargets, null, null, null, null);
                GameComponent_Tribals.Instance.allAnimalResearchCompleted = false;
                jobRitual.ritual.RemoveObligation(jobRitual.obligation, completed: true);
                jobRitual.ritual.activeObligations.Clear();
            }


        }

       

        
    }
}
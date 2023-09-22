
using System.Collections.Generic;
using System.Linq;
using RimWorld;
using Verse;

namespace VFETribals
{
    public class RitualOutcomeEffectWorker_AdvanceToUltra : RitualOutcomeEffectWorker
    {
        public override bool SupportsAttachableOutcomeEffect => false;

        public RitualOutcomeEffectWorker_AdvanceToUltra()
        {
        }

        public RitualOutcomeEffectWorker_AdvanceToUltra(RitualOutcomeEffectDef def)
            : base(def)
        {
        }

        public override void Apply(float progress, Dictionary<Pawn, int> totalPresence, LordJob_Ritual jobRitual)
        {
            GameComponent_Tribals comp = Current.Game.GetComponent<GameComponent_Tribals>();
            LookTargets lookTargets = jobRitual.selectedTarget;

            if (comp != null)
            {
                comp.AdvanceToEra(VFET_DefOf.VFET_FormNexus);
                Find.LetterStack.ReceiveLetter(VFET_DefOf.VFET_FormNexus.label, VFET_DefOf.VFET_FormNexus.description, LetterDefOf.RitualOutcomePositive, lookTargets, null, null, null, null);
                jobRitual.ritual.RemoveObligation(jobRitual.obligation, completed: true);
                jobRitual.ritual.activeObligations.Clear();
            }


        }

       

        
    }
}
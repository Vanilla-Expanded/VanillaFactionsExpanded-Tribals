
using System.Collections.Generic;
using System.Linq;
using RimWorld;
using Verse;

namespace VFETribals
{
    public class RitualOutcomeEffectWorker_AdvanceToIndustrial : RitualOutcomeEffectWorker
    {
        public override bool SupportsAttachableOutcomeEffect => false;

        public RitualOutcomeEffectWorker_AdvanceToIndustrial()
        {
        }

        public RitualOutcomeEffectWorker_AdvanceToIndustrial(RitualOutcomeEffectDef def)
            : base(def)
        {
        }

        public override void Apply(float progress, Dictionary<Pawn, int> totalPresence, LordJob_Ritual jobRitual)
        {
            GameComponent_Tribals comp = Current.Game.GetComponent<GameComponent_Tribals>();
            LookTargets lookTargets = jobRitual.selectedTarget;

            if (comp != null)
            {
                comp.AdvanceToEra(VFET_DefOf.VFET_FormCity);
                Find.LetterStack.ReceiveLetter(VFET_DefOf.VFET_FormCity.label, VFET_DefOf.VFET_FormCity.description, LetterDefOf.RitualOutcomePositive, lookTargets, null, null, null, null);
                GameComponent_Tribals.Instance.allMedievalResearchCompleted = false;
                jobRitual.ritual.RemoveObligation(jobRitual.obligation, completed: true);

            }


        }

       

        
    }
}

using System;
using System.Collections.Generic;
using RimWorld;
using Verse;

namespace VFETribals
{
    public class RitualOutcomeComp_TotalPawnSkill : RitualOutcomeComp_Quality
    {


        public SkillDef skillDef;

        public override float QualityOffset(LordJob_Ritual ritual, RitualOutcomeComp_Data data)
        {
            return curve.Evaluate(Count(ritual, data));
        }

      
        public float TotalSkill(List<Pawn> pawns)
        {
            float totalSkill = 0;
            foreach (Pawn ParticipantPawn in pawns)
            {
                if (ParticipantPawn != null)
                {
                    totalSkill += ParticipantPawn.skills.GetSkill(skillDef).Level;
                }
            }
            return totalSkill;
        }


        public override float Count(LordJob_Ritual ritual, RitualOutcomeComp_Data data)
        {
        
            return TotalSkill(ritual.lord.ownedPawns);
        }

        public override string GetDesc(LordJob_Ritual ritual = null, RitualOutcomeComp_Data data = null)
        {
            if (ritual == null)
            {
                return labelAbstract;
            }
            return label.CapitalizeFirst();
        }

        public override QualityFactor GetQualityFactor(Precept_Ritual ritual, TargetInfo ritualTarget, RitualObligation obligation, RitualRoleAssignments assignments, RitualOutcomeComp_Data data)
        {
            float x = TotalSkill(assignments.Participants);
            float num = curve.Evaluate(x);
            return new QualityFactor
            {
                label = label.CapitalizeFirst(),
                count = x.ToString(),
                present = true,
                qualityChange = ((Math.Abs(num) > float.Epsilon) 
                ? "OutcomeBonusDesc_QualitySingleOffset".Translate(num.ToStringWithSign("0.#%")).Resolve() : " - "),
                quality = num,
                positive = (num >= 0f),
                priority = 0f,
            };
        }

        public override bool Applies(LordJob_Ritual ritual)
        {
            return true;
        }
    }
}
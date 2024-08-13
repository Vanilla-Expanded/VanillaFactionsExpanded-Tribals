using RimWorld;
using System;
using Verse;

namespace VFETribals
{
    public class StatPart_FloorPainting_TechLevel : StatPart
    {
        private float animal = 1f;

        private float neolithic = 1f;

        private float medieval = 1f;

        private float industrial = 1f;

        private float spacer = 1f;

        private float ultra = 1f; 

        public override void TransformValue(StatRequest req, ref float val)
        {
            if (req.BuildableDef == VFET_DefOf.VFET_FloorPainting)
            {
                float a = val * BeautyMultiplier(Faction.OfPlayer.def.techLevel) - val;
                val += a;
            }
        }

        public override string ExplanationPart(StatRequest req)
        {
            if (req.BuildableDef == VFET_DefOf.VFET_FloorPainting)
            {
                string text = "VFET.StatsReport_BeautyMultiplier".Translate(Faction.OfPlayer.def.techLevel.ToStringHuman()) + ": x" 
                    + BeautyMultiplier(Faction.OfPlayer.def.techLevel).ToStringPercent();
                return text;
            }
            return null;
        }

        private float BeautyMultiplier(TechLevel techlevel)
        {
            return techlevel switch
            {
                TechLevel.Animal => animal,
                TechLevel.Neolithic => neolithic,
                TechLevel.Medieval => medieval,
                TechLevel.Industrial => industrial,
                TechLevel.Spacer => spacer,
                TechLevel.Ultra => ultra,
                TechLevel.Archotech => ultra,
                _ => throw new ArgumentOutOfRangeException(),
            };
        }
    }
}


using RimWorld;
using Verse;

namespace VFETribals
{
    public class ThoughtWorker_NoExpectations : ThoughtWorker
    {
        public override ThoughtState CurrentStateInternal(Pawn p)
        {
            if (p.IsSlave)
            {
                return ThoughtState.Inactive;
            }

            if(Find.ResearchManager.GetProgress(VFET_DefOf.VFET_Culture) >= 1)
            {
                return ThoughtState.Inactive;
            }
           
            return ThoughtState.ActiveAtStage(0);
        }

      
    }
}

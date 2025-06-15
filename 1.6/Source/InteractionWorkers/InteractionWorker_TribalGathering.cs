using RimWorld;
using Verse;
namespace VFETribals
{
    public class InteractionWorker_TribalGathering : InteractionWorker
    {
        public override float RandomSelectionWeight(Pawn initiator, Pawn recipient)
        {
            if(initiator.CurJobDef==VFET_DefOf.VFET_StandAndBeSociallyActive &&
                recipient.CurJobDef == VFET_DefOf.VFET_StandAndBeSociallyActive) { return 1f; }

            return 0f;
        }
    }
}

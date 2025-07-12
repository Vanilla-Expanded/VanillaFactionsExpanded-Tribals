
using RimWorld;
using System.Collections.Generic;
using Verse;
using Verse.AI;
namespace VFETribals
{
    public class JobDriver_StandSociallyActive : JobDriver
    {
        public override bool TryMakePreToilReservations(bool errorOnFailed)
        {
            return true;
        }

        public override IEnumerable<Toil> MakeNewToils()
        {
            var toil = ToilMaker.MakeToil();
            toil.tickIntervalAction = delegate(int delta)
            {
                if (pawn.IsHashIntervalTick(240, delta))
                {
                    pawn.Rotation = Rot4.Random;
                }

                pawn.GainComfortFromCellIfPossible(delta);
            };
            toil.socialMode = RandomSocialMode.SuperActive;
            toil.defaultCompleteMode = ToilCompleteMode.Never;
            toil.handlingFacing = true;
            yield return toil;
        }

    }
}
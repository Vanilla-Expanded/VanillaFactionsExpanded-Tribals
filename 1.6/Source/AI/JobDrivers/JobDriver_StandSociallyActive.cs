
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

        public int AgeTicks => Find.TickManager.TicksGame - startTick;

        public override IEnumerable<Toil> MakeNewToils()
        {
            Toil toil = ToilMaker.MakeToil("MakeNewToils");
            toil.tickAction = delegate
            {
                if (AgeTicks % 240 == 0)
                {
                    pawn.Rotation = Rot4.Random;
                }

                pawn.GainComfortFromCellIfPossible(1);
            };
            toil.socialMode = RandomSocialMode.SuperActive;
            toil.defaultCompleteMode = ToilCompleteMode.Never;
            toil.handlingFacing = true;
            yield return toil;
        }

    }
}
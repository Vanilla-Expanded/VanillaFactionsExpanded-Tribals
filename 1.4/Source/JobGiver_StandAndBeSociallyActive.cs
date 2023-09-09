using RimWorld;
using Verse;
using Verse.AI;

namespace VFETribals
{
    public class JobGiver_StandAndBeSociallyActive : ThinkNode_JobGiver
    {
        public IntRange ticksRange = new IntRange(300, 600);



        public override ThinkNode DeepCopy(bool resolve = true)
        {
            JobGiver_StandAndBeSociallyActive obj = (JobGiver_StandAndBeSociallyActive)base.DeepCopy(resolve);
            obj.ticksRange = ticksRange;          

            return obj;
        }

        public override Job TryGiveJob(Pawn pawn)
        {
            Job job = JobMaker.MakeJob(VFET_DefOf.VFET_StandAndBeSociallyActive);
            job.expiryInterval = ticksRange.RandomInRange;

            return job;
        }
    }
}
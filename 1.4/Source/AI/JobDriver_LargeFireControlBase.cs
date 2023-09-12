using System.Collections.Generic;
using Verse.AI;

namespace VFETribals
{
    public abstract class JobDriver_LargeFireControlBase : JobDriver
    {
        public LargeFire LargeFire => job.targetA.Thing as LargeFire;
        public override bool TryMakePreToilReservations(bool errorOnFailed)
        {
            return pawn.Reserve(job.targetA, job, 1, -1, null, errorOnFailed);
        }
        public override IEnumerable<Toil> MakeNewToils()
        {
            this.FailOnDespawnedOrNull(TargetIndex.A);
            this.FailOn(() => FailCondition());
            yield return Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.Touch);
            yield return Toils_General.Wait(60).FailOnCannotTouch(TargetIndex.A, PathEndMode.Touch);
            var finalize = new Toil
            {
                initAction = delegate
                {
                    DoWork();
                },
                defaultCompleteMode = ToilCompleteMode.Instant
            };
            yield return finalize;
        }

        public abstract bool FailCondition();

        public abstract void DoWork();
    }

    public class JobDriver_LightLargeFire : JobDriver_LargeFireControlBase
    {
        public override void DoWork()
        {
            LargeFire.lightOn = true;
            var des = base.Map.designationManager.DesignationOn(base.TargetThingA, VFET_DefOf.VFET_LightLargeFire);
            if (des != null)
            {
                base.Map.designationManager.RemoveDesignation(des);
            }

        }

        public override bool FailCondition()
        {
            return base.Map.designationManager.DesignationOn(base.TargetThingA, VFET_DefOf.VFET_LightLargeFire) is null;
        }
    }

    public class JobDriver_ExtinguishLargeFire : JobDriver_LargeFireControlBase
    {
        public override void DoWork()
        {
            LargeFire.lightOn = false;
            var des = base.Map.designationManager.DesignationOn(base.TargetThingA, VFET_DefOf.VFET_ExtinguishLargeFire);
            if (des != null)
            {
                base.Map.designationManager.RemoveDesignation(des);
            }
        }

        public override bool FailCondition()
        {
            return base.Map.designationManager.DesignationOn(base.TargetThingA, VFET_DefOf.VFET_ExtinguishLargeFire) is null;
        }
    }
}

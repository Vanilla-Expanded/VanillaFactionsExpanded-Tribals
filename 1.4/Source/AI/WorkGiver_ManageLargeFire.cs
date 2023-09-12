using RimWorld;
using System.Collections.Generic;
using System.Linq;
using Verse;
using Verse.AI;

namespace VFETribals
{
    public abstract class WorkGiver_ManageLargeFire : WorkGiver_Scanner
    {
        public override PathEndMode PathEndMode => PathEndMode.Touch;
        public override Danger MaxPathDanger(Pawn pawn)
        {
            return Danger.Deadly;
        }
        public override IEnumerable<Thing> PotentialWorkThingsGlobal(Pawn pawn)
        {
            foreach (var largeFire in LargeFire.largeFires.Where(x => x?.Map == pawn.Map))
            {
                yield return largeFire;
            }
        }
    }

    public class WorkGiver_LightLargeFire : WorkGiver_ManageLargeFire
    {
        public override bool HasJobOnThing(Pawn pawn, Thing t, bool forced = false)
        {
            if (!pawn.CanReserveAndReach(t, PathEndMode, MaxPathDanger(pawn), 1, -1, null, forced))
            {
                return false;
            }
            if (t.Map.designationManager.DesignationOn(t, VFET_DefOf.VFET_LightLargeFire) is null)
            {
                return false;
            }
            return true;
        }

        public override Job JobOnThing(Pawn pawn, Thing t, bool forced = false)
        {
            return JobMaker.MakeJob(VFET_DefOf.VFET_LightLargeFireJob, t);
        }
    }

    public class WorkGiver_ExtinguishLargeFire : WorkGiver_ManageLargeFire
    {
        public override bool HasJobOnThing(Pawn pawn, Thing t, bool forced = false)
        {
            if (!pawn.CanReserve(t, 1, -1, null, forced))
            {
                return false;
            }
            var LargeFire = t as LargeFire;
            if (LargeFire.Map.designationManager.DesignationOn(LargeFire, VFET_DefOf.VFET_ExtinguishLargeFire) is null)
            {
                return false;
            }
            return true;
        }

        public override Job JobOnThing(Pawn pawn, Thing t, bool forced = false)
        {
            return JobMaker.MakeJob(VFET_DefOf.VFET_ExtinguishLargeFireJob, t);
        }
    }

}

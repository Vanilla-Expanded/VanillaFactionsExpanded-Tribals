using HarmonyLib;
using RimWorld;
using System.Linq;
using System.Reflection;
using Verse;
using Verse.AI;

namespace VFETribals
{
    [HarmonyPatch]
    public static class JobDriver_ConstructFinishFrame_MakeNewToils_TickAction_Patch
    {
        [HarmonyTargetMethod]
        public static MethodBase TargetMethod()
        {
            return typeof(JobDriver_ConstructFinishFrame).GetNestedTypes(AccessTools.all).SelectMany(x => x.GetMethods(AccessTools.all)
                            .Where(x => x.Name.Contains("<MakeNewToils>") && x.ReturnType == typeof(void))).ToList()[1];
        }

        public static void Postfix(Toil ___build)
        {
            Pawn actor = ___build.actor;
            var jobDriver = actor.jobs.curDriver as JobDriver_ConstructFinishFrame;
            Frame frame = jobDriver.Frame;
            if (frame.def.entityDefToBuild == VFET_DefOf.VFET_FloorPainting && actor.skills != null)
            {
                actor.skills.Learn(SkillDefOf.Artistic, 0.25f);
            }
        }
    }
}

using HarmonyLib;
using RimWorld;
using Verse;

namespace VFETribals
{
    [HarmonyPatch(typeof(Designator), "CreateReverseDesignationGizmo")]
    public static class Designator_CreateReverseDesignationGizmo_Patch
    {
        public static void Postfix(ref Command_Action __result, Designator __instance)
        {
            if (__result != null)
            {
                foreach (var def in Utils.researchProjectsWithDesignators)
                {
                    if (def.unlocksDesignators.Contains(__instance.GetType()))
                    {
                        if (!def.IsFinished)
                        {
                            var reason = "VFET.WorkDisabledByMissingResearch".Translate(def.label);
                            __result.Disable(reason);
                        }
                    }
                }
            }
        }
    }
}

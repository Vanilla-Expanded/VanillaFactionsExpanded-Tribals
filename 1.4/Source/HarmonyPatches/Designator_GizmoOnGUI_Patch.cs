using HarmonyLib;
using RimWorld;
using Verse;

namespace VFETribals
{
    [HarmonyPatch(typeof(Designator), "GizmoOnGUI")]
    public static class Designator_GizmoOnGUI_Patch
    {
        public static void Postfix(Designator __instance)
        {
            foreach (var def in Utils.researchProjectsWithDesignators)
            {
                if (def.unlocksDesignators.Contains(__instance.GetType()))
                {
                    var reason = "VFET.WorkDisabledByMissingResearch".Translate(def.label);
                    if (!def.IsFinished)
                    {
                        __instance.Disable(reason);
                    }
                    else if (__instance.disabledReason == reason)
                    {
                        __instance.disabled = false;
                        __instance.disabledReason = null;
                    }
                }
            }
        }
    }
}

using HarmonyLib;
using RimWorld;
using System.Collections.Generic;
using System.Reflection;
using Verse;

namespace VFETribals
{
    [HarmonyPatch]
    public static class Designator_DesignateThing_Patch
    {
        [HarmonyTargetMethods]
        public static IEnumerable<MethodBase> TargetMethods()
        {
            foreach (var type in typeof(Designator).AllSubclasses())
            {
                var method = AccessTools.DeclaredMethod(type, "DesignateThing");
                if (method != null)
                {
                    yield return method;
                }
            }
        }

        public static bool Prefix(Designator __instance)
        {
            foreach (var def in Utils.researchProjectsWithDesignators)
            {
                if (def.unlocksDesignators.Contains(__instance.GetType()))
                {
                    if (!def.IsFinished)
                    {
                        var reason = "VFET.WorkDisabledByMissingResearch".Translate(def.label);
                        Messages.Message(reason, MessageTypeDefOf.NeutralEvent);
                        return false;
                    }
                }
            }
            return true;
        }
    }
}

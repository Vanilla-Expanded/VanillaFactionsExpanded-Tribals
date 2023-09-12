using HarmonyLib;
using RimWorld;
using Verse;

namespace VFETribals
{
    [HarmonyPatch(typeof(PawnColumnWorker_Designator), "SetValue")]
    public static class PawnColumnWorker_Designator_SetValue_Patch
    {
        public static bool Prefix(PawnColumnWorker_Designator __instance)
        {
            foreach (var def in Utils.researchProjectsWithDesignators)
            {
                foreach (var type in def.unlocksDesignators)
                {
                    if (DesignatorUtility.StandaloneDesignators.TryGetValue(type, out var designator) is false)
                    {
                        designator = Find.ReverseDesignatorDatabase.AllDesignators.FirstOrDefault(x => x.GetType() == type);
                    }
                    if (designator != null)
                    {
                        if (designator.Designation == __instance.DesignationType && !def.IsFinished)
                        {
                            var reason = "VFET.WorkDisabledByMissingResearch".Translate(def.label);
                            Messages.Message(reason, MessageTypeDefOf.NeutralEvent);
                            return false;
                        }
                    }
                }
            }
            return true;
        }
    }
}

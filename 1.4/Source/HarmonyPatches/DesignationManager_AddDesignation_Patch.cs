using HarmonyLib;
using RimWorld;
using Verse;

namespace VFETribals
{
    [HarmonyPatch(typeof(DesignationManager), "AddDesignation")]
    public static class DesignationManager_AddDesignation_Patch
    {
        public static bool Prefix(Designation newDes)
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
                        if (designator.Designation == newDes.def && !def.IsFinished)
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

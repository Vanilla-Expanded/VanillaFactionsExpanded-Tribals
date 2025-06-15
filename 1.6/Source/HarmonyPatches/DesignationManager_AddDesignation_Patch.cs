using HarmonyLib;
using RimWorld;
using Verse;
using Verse.Profile;

namespace VFETribals
{
    [HarmonyPatch(typeof(DesignationManager), "AddDesignation")]
    public static class DesignationManager_AddDesignation_Patch
    {
        public static bool Prefix(Designation newDes)
        {
            foreach (var def in Utils.researchProjectsWithDesignators)
            {
                if (def.IsFinished is false)
                {
                    foreach (var type in def.unlocksDesignators)
                    {
                        if ((Find.ReverseDesignatorDatabase.desList is not null || UnityData.IsInMainThread))
                        {
                            if (DesignatorUtility.StandaloneDesignators.TryGetValue(type, out var designator) is false)
                            {
                                designator = Find.ReverseDesignatorDatabase.AllDesignators.FirstOrDefault(x => x.GetType() == type);
                            }
                            if (designator != null)
                            {
                                if (designator.Designation == newDes.def)
                                {
                                    var reason = "VFET.WorkDisabledByMissingResearch".Translate(def.label);
                                    Messages.Message(reason, MessageTypeDefOf.NeutralEvent);
                                    return false;
                                }
                            }
                        }
                    }
                }
            }
            return true;
        }
    }
}

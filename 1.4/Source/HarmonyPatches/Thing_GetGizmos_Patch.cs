using HarmonyLib;
using RimWorld;
using System.Collections.Generic;
using Verse;

namespace VFETribals
{
    [HarmonyPatch(typeof(Thing), "GetGizmos")]
    public static class Thing_GetGizmos_Patch
    {
        public static IEnumerable<Gizmo> Postfix(IEnumerable<Gizmo> __result, Thing __instance)
        {
            foreach (var g in __result)
            {
                yield return g;
            }

            if (ModsConfig.IdeologyActive is false)
            {
                Thing.ShowingGizmosForRitualsTmp.Clear();
                foreach (Ideo ideo in Faction.OfPlayer.ideos.AllIdeos)
                {
                    for (int i = 0; i < ideo.PreceptsListForReading.Count; i++)
                    {
                        Precept precept = ideo.PreceptsListForReading[i];
                        if (precept.def == VFET_DefOf.VFET_TribalGathering)
                        {
                            Precept_Ritual precept_Ritual;
                            Precept_Ritual ritual = (precept_Ritual = precept as Precept_Ritual);
                            if (precept_Ritual == null || (precept.def.mergeRitualGizmosFromAllIdeos
                                && Thing.ShowingGizmosForRitualsTmp.Contains(ritual.sourcePattern))
                                || !ritual.ShouldShowGizmo(__instance))
                            {
                                continue;
                            }
                            foreach (Gizmo item in ritual.GetGizmoFor(__instance))
                            {
                                yield return item;
                                Thing.ShowingGizmosForRitualsTmp.Add(ritual.sourcePattern);
                            }
                        }
                    }
                }

                List<LordJob_Ritual> activeRituals = Find.IdeoManager.GetActiveRituals(__instance.MapHeld);
                foreach (LordJob_Ritual item2 in activeRituals)
                {
                    if (item2.ritual.def == VFET_DefOf.VFET_TribalGathering && item2.selectedTarget == __instance)
                    {
                        yield return item2.GetCancelGizmo();
                    }
                }
            }
        }
    }
}

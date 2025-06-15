using HarmonyLib;
using RimWorld;
using System.Linq;
using Verse;

namespace VFETribals
{
    [HarmonyPatch(typeof(CharacterCardUtility), "GetWorkTypeDisabledCausedBy")]
    public static class CharacterCardUtility_GetWorkTypeDisabledCausedBy_Patch
    {
        public static void Postfix(Pawn pawn, WorkTags workTag, ref string __result)
        {
            if (pawn.Faction == Faction.OfPlayerSilentFail && workTag.IsUnlocked(out var research) is false)
            {
                var reason = "VFET.WorkDisabledByMissingResearch".Translate(research.label);
                if (__result.Contains(reason) is false)
                {
                    if (__result.Any())
                    {
                        __result += "\n" + reason + "\n";
                    }
                    else
                    {
                        __result += reason + "\n";
                    }
                }
            }
        }
    }
}

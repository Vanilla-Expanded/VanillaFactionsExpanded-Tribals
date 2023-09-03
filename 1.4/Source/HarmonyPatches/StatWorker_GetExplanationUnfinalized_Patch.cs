using HarmonyLib;
using RimWorld;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;

namespace VFETribals
{
    [HarmonyPatch(typeof(StatWorker), "GetExplanationUnfinalized")]
    public static class StatWorker_GetExplanationUnfinalized_Patch
    {
        public static void Postfix(ref string __result, StatWorker __instance, StatRequest req, ToStringNumberSense numberSense)
        {
            if (GameComponent_Tribals.Instance != null)
            {
                bool hasCornerstoneModifiers = false;
                var stringBuilder = new StringBuilder();
                var explanation = new StringBuilder();
                foreach (var cornerstone in GameComponent_Tribals.Instance.cornerstones)
                {
                    if (cornerstone.statOffsets != null && cornerstone.statOffsets.Exists(x => x.stat == __instance.stat))
                    {
                        hasCornerstoneModifiers = true;
                        string valueToStringAsOffset = cornerstone.statOffsets.First((StatModifier se) => se.stat == __instance.stat).ValueToStringAsOffset;
                        explanation.AppendLine("    " + cornerstone.LabelCap + ": " + valueToStringAsOffset);
                    }
                    if (cornerstone.statFactors != null && cornerstone.statFactors.Exists(x => x.stat == __instance.stat))
                    {
                        hasCornerstoneModifiers = true;
                        string toStringAsFactor = cornerstone.statFactors.First((StatModifier se) => se.stat == __instance.stat).ToStringAsFactor;
                        explanation.AppendLine("    " + cornerstone.LabelCap + ": " + toStringAsFactor);
                    }
                }

                if (hasCornerstoneModifiers)
                {
                    stringBuilder.AppendLine("VFET.StatsReport_Cornerstones".Translate());
                    stringBuilder.AppendLine(explanation.ToString());
                    __result += "\n" + stringBuilder.ToString();
                }
            }
        }
    }
}

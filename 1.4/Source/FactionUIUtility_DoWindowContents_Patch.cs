using HarmonyLib;
using RimWorld;
using UnityEngine;
using Verse;

namespace VFETribals
{
    [HarmonyPatch(typeof(FactionUIUtility), nameof(FactionUIUtility.DoWindowContents))]
    public static class FactionUIUtility_DoWindowContents_Patch
    {
        public static void Postfix()
        {
            if (ModCompatibility.VFEClassicalActive)
            {
                if (Widgets.ButtonText(new Rect(120f + 15, 10f, 120f, 30f), "VFET.Cornerstones".Translate())) Find.WindowStack.Add(new Window_CustomizeCornerStones());
            }
            else
            {
                if (Widgets.ButtonText(new Rect(0, 10f, 120f, 30f), "VFET.Cornerstones".Translate())) Find.WindowStack.Add(new Window_CustomizeCornerStones());
            }
        }
    }
}

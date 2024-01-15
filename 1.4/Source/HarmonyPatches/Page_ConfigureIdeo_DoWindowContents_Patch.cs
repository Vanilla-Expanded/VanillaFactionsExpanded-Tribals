using HarmonyLib;
using RimWorld;
using UnityEngine;
using Verse;

namespace VFETribals
{
    [HarmonyPatch(typeof(Page_ConfigureIdeo), "DoWindowContents")]
    public static class Page_ConfigureIdeo_DoWindowContents_Patch
    {
        public static void Postfix(Rect rect)
        {
            var label = new Rect(rect.x + 275, rect.y, 500, Page.BottomButSize.y);
            Widgets.Label(label, "VFET.CustomizeNPCIdeologionDesc".Translate());
        }
    }
}

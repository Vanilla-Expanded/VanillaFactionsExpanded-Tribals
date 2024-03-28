using HarmonyLib;
using UnityEngine;
using Verse;

namespace VFETribals
{
    [HarmonyPatch(typeof(Widgets), "DefIcon")]
    public static class Widgets_DefIcon_Patch
    {
        public static void Postfix(Rect rect, Def def, ThingDef stuffDef = null, float scale = 1f, ThingStyleDef thingStyleDef = null, bool drawPlaceholder = false, Color? color = null, Material material = null, int? graphicIndexOverride = null)
        {
            if (def is FakeDef fakeDef && fakeDef.icon != null)
            {
                Widgets.DrawTextureFitted(rect, fakeDef.icon, scale, material);
            }
        }
    }
}

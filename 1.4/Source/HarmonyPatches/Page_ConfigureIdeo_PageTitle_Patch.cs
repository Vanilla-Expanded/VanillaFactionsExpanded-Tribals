using HarmonyLib;
using RimWorld;
using Verse;

namespace VFETribals
{
    [HarmonyPatch(typeof(Page_ConfigureIdeo), "PageTitle", MethodType.Getter)]
    public static class Page_ConfigureIdeo_PageTitle_Patch
    {
        public static bool Prefix(Page_ConfigureIdeo __instance, ref string __result)
        {
            if (Current.Game.Scenario?.playerFaction?.factionDef == VFET_DefOf.VFET_WildMen)
            {
                __result = "VFET.CustomizeNPCIdeologion".Translate();
                return false;
            }
            return true;
        }
    }
}

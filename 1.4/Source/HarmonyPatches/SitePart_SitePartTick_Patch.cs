using HarmonyLib;
using RimWorld.Planet;

namespace VFETribals
{
    [HarmonyPatch(typeof(SitePart), "SitePartTick")]
    public static class SitePart_SitePartTick_Patch
    {
        public static bool Prefix(SitePart __instance)
        {
            if (__instance.def == VFET_DefOf.VFET_WildMenSite)
            {
                return false;
            }
            return true;
        }
    }
}

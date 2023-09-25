using HarmonyLib;
using Verse;

namespace VFETribals
{
    [StaticConstructorOnStartup]
    public static class Startup
    {
        static Startup()
        {
            new Harmony("VFETribalsMod").PatchAll();
        }
    }
}

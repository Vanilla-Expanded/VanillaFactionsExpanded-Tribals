using HarmonyLib;
using RimWorld;
using System.Collections.Generic;
using Verse;
using Verse.Profile;

namespace VFETribals
{
    [StaticConstructorOnStartup]
    public static class Startup
    {
        public static Dictionary<FactionDef, TechLevel> originalTechLevels = new Dictionary<FactionDef, TechLevel>();
        static Startup()
        {
            new Harmony("VFETribalsMod").PatchAll();
            foreach (var faction in DefDatabase<FactionDef>.AllDefs)
            {
                originalTechLevels[faction] = faction.techLevel;
            }
        }
    }

    [HarmonyPatch(typeof(MemoryUtility), "UnloadUnusedUnityAssets")]
    public static class MemoryUtility_UnloadUnusedUnityAssets_Patch
    {
        public static void Postfix()
        {
            foreach (var kvp in Startup.originalTechLevels)
            {
                kvp.Key.techLevel = kvp.Value;
            }
        }
    }
}

using HarmonyLib;
using RimWorld;

namespace VFETribals
{
    [HarmonyPatch(typeof(StorytellerComp), "IncidentChanceFinal")]
    public static class StorytellerComp_IncidentChanceFinal_Patch
    {
        public static void Postfix(IncidentDef def, ref float __result)
        {
            if (typeof(IncidentWorker_RaidEnemy).IsAssignableFrom(def.workerClass) 
                && Faction.OfPlayer.def.techLevel == TechLevel.Animal)
            {
                __result *= 0.25f;
            }
        }
    }
}

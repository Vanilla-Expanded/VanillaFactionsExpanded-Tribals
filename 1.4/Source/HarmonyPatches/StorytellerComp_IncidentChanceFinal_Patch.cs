using HarmonyLib;
using RimWorld;
using Verse;

namespace VFETribals
{
    [HarmonyPatch(typeof(StorytellerComp), "IncidentChanceFinal")]
    public static class StorytellerComp_IncidentChanceFinal_Patch
    {
        public static void Postfix(IncidentDef def, ref float __result)
        {
            if (typeof(IncidentWorker_RaidEnemy).IsAssignableFrom(def.workerClass))
            {
                if (Faction.OfPlayer.def.techLevel == TechLevel.Animal)
                {
                    __result *= 0.25f;
                }
                if (GameComponent_Tribals.Instance.lastLargeFireUpdate != 0 
                    && GameComponent_Tribals.Instance.lastLargeFireUpdate + 150 >= Find.TickManager.TicksGame)
                {
                    __result *= 1.1f;
                }
            }
        }
    }
}

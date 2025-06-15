using HarmonyLib;
using RimWorld;

namespace VFETribals
{
    [HarmonyPatch(typeof(ScenPart_ConfigPage_ConfigureStartingPawnsBase), "PostIdeoChosen")]
    public static class ScenPart_ConfigPage_ConfigureStartingPawnsBase_PostIdeoChosen_Patch
    {
        public static void Prefix()
        {
            if (Faction.OfPlayer?.def != VFET_DefOf.VFET_WildMen)
            {
                GameComponent_Tribals.ResearchAllAnimalProjects();
            }
        }
    }
}

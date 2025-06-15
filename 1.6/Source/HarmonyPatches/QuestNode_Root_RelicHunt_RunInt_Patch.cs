using HarmonyLib;
using RimWorld;
using RimWorld.QuestGen;

namespace VFETribals
{
    [HarmonyPatch(typeof(QuestNode_Root_RelicHunt), "RunInt")]
    public static class QuestNode_Root_RelicHunt_RunInt_Patch
    {
        public static bool Prefix()
        {
            if (Faction.OfPlayer.def == VFET_DefOf.VFET_WildMen && Faction.OfPlayer.def.techLevel == TechLevel.Animal)
            {
                return false;
            }
            return true;
        }
    }
}

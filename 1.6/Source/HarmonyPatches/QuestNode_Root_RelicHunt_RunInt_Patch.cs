using HarmonyLib;
using RimWorld;
using RimWorld.QuestGen;

namespace VFETribals
{
    [HarmonyPatch(typeof(QuestNode_Root_RelicHunt), "TestRunInt")]
    public static class QuestNode_Root_RelicHunt_TestRunInt_Patch
    {
        public static void Postfix(ref bool __result)
        {
            if (Faction.OfPlayer.def == VFET_DefOf.VFET_WildMen && Faction.OfPlayer.def.techLevel == TechLevel.Animal)
            {
                __result= false;
            }
         
        }
    }
}

using HarmonyLib;
using RimWorld;
using Verse;

namespace VFETribals
{
    [HarmonyPatch(typeof(Thing), "TakeDamage")]
    public static class Thing_TakeDamage_Patch
    {
        public static void Prefix(Thing __instance, ref DamageInfo dinfo)
        {
            if (dinfo.Instigator is Pawn attacker && attacker.equipment?.Primary?.def == VFET_DefOf.VFET_Stake 
                && dinfo.Weapon == VFET_DefOf.VFET_Stake)
            {
                attacker.equipment.Primary.HitPoints--;
                if (attacker.equipment.Primary.HitPoints <= 0)
                {
                    attacker.equipment.Primary.Destroy();
                }
                if (ModsConfig.BiotechActive && __instance is Pawn victim && victim.genes != null 
                    && victim.genes.HasActiveGene(GeneDefOf.Deathless))
                {
                    dinfo.SetAmount(dinfo.Amount * 5f);
                }
            }
        }
    }

}

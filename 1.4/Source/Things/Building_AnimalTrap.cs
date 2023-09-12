using RimWorld;
using System.Linq;
using Verse;
using Verse.Sound;

namespace VFETribals
{
    public class Building_AnimalTrap : Building_TrapDamager
    {
        public override float SpringChance(Pawn p)
        {
            if (p.Faction == Faction.OfPlayer)
            {
                return 0.04f;
            }
            else if (p.Faction != null)
            {
                if (p.Faction.HostileTo(Faction.OfPlayer))
                {
                    return 0.15f;
                }
                else
                {
                    return 0f;
                }
            }
            else if (p.RaceProps.Animal)
            {
                return 1f;
            }
            return base.SpringChance(p);
        }

        public override void SpringSub(Pawn p)
        {
            SoundDefOf.TrapSpring.PlayOneShot(new TargetInfo(Position, Map));
            if (p == null)
            {
                return;
            }
            float num = this.GetStatValue(StatDefOf.TrapMeleeDamage) * DamageRandomFactorRange.RandomInRange;
            float armorPenetration = num * 0.015f;
            var part = p.health.hediffSet.GetNotMissingParts().Where(x => x.IsCorePart
            || x.IsInGroup(BodyPartGroupDefOf.FullHead)).RandomElement();
            DamageInfo dinfo = new DamageInfo(DamageDefOf.Stab, num, armorPenetration, -1f, this, part);
            DamageWorker.DamageResult damageResult = p.TakeDamage(dinfo);
            BattleLogEntry_DamageTaken battleLogEntry_DamageTaken = new BattleLogEntry_DamageTaken(p, RulePackDefOf.DamageEvent_TrapSpike);
            Find.BattleLog.Add(battleLogEntry_DamageTaken);
            damageResult.AssociateWithLog(battleLogEntry_DamageTaken);
            if (p.RaceProps.Animal)
            {
                Messages.Message("VFET.AnimalTrapSprung".Translate(p.Label), new TargetInfo(this.Position, this.Map), MessageTypeDefOf.NeutralEvent);
            }
        }
    }
}

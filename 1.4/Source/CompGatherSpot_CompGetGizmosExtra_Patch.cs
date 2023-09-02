using HarmonyLib;
using RimWorld;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace VFETribals
{
    [HarmonyPatch(typeof(CompGatherSpot), "CompGetGizmosExtra")]
    public static class CompGatherSpot_CompGetGizmosExtra_Patch
    {
        public static IEnumerable<Gizmo> Postfix(IEnumerable<Gizmo> __result)
        {
            foreach (var g in __result)
            {
                yield return g;
            }

            yield return new Command_Action
            {
                defaultLabel = "DEV: customize cornerstones",
                action = () =>
                {
                    Find.WindowStack.Add(new Window_CustomizeCornerStones());
                }
            };

            var eraAdvancementDef = DefDatabase<EraAdvancementDef>.AllDefsListForReading
                .FirstOrDefault(x => x.newTechLevel - 1 == Faction.OfPlayer.def.techLevel);
            if (eraAdvancementDef != null)
            {
                yield return new Command_Action
                {
                    defaultLabel = eraAdvancementDef.label,
                    defaultDesc = eraAdvancementDef.description,
                    disabled = DefDatabase<ResearchProjectDef>.AllDefs.Where(x => x.techLevel == Faction.OfPlayer.def.techLevel)
                        .Any(x => x.IsFinished is false),
                    action = () =>
                    {
                        GameComponent_Tribals.Instance.AdvanceToEra(eraAdvancementDef);
                    }
                };
            }

            yield return new Command_Action
            {
                defaultLabel = "DEV: increase cornerstone point",
                action = () =>
                {
                    GameComponent_Tribals.Instance.OffsetAvailableCornerstonePoints(1);
                }
            };
        }
    }
}

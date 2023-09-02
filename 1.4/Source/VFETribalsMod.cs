using HarmonyLib;
using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace VFETribals
{
    public class VFETribalsMod : Mod
    {
        public VFETribalsMod(ModContentPack pack) : base(pack)
        {
			new Harmony("VFETribalsMod").PatchAll();
        }
    }

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
                    GameComponent_Tribals.Instance.availableCornerStonePoints++;
                }
            };
        }
    }

    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct)]
    public class HotSwappableAttribute : Attribute
    {
    }

    public class EraAdvancementDef : Def
    {
        public TechLevel newTechLevel;
        public int cornerstonePoint;
    }

    [HotSwappable]
    public class Window_CustomizeCornerStones : Window
    {
        public override void DoWindowContents(Rect inRect)
        {
        }
    }

    [HotSwappable]
    public class GameComponent_Tribals : GameComponent
    {
        public int availableCornerStonePoints;

        public TechLevel? playerTechLevel;

        public static GameComponent_Tribals Instance;

        public GameComponent_Tribals()
        {
            Instance = this;
        }

        public GameComponent_Tribals(Game game)
        {
            Instance = this;
        }

        public void AdvanceToEra(EraAdvancementDef def)
        {
            playerTechLevel = Faction.OfPlayer.def.techLevel = def.newTechLevel;
            availableCornerStonePoints += def.cornerstonePoint;
        }

        public override void FinalizeInit()
        {
            base.FinalizeInit();
            if (playerTechLevel.HasValue)
            {
                Faction.OfPlayer.def.techLevel = playerTechLevel.Value;
            }
        }

        public override void StartedNewGame()
        {
            base.StartedNewGame();
            playerTechLevel = Faction.OfPlayer.def.techLevel;
        }

        public override void ExposeData()
        {
            base.ExposeData();
            Scribe_Values.Look(ref availableCornerStonePoints, "availableCornerStonePoints");
        }
    }
}

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
            yield return new Command_Action
            {
                defaultLabel = "DEV: increase cornerstone point",
                action = () =>
                {
                    GameComponent_Tribals.Instance.IncrementCornerstonePoint();
                }
            };
        }
    }

    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct)]
    public class HotSwappableAttribute : Attribute
    {
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

        public TechLevel playerTechLevel;

        public static GameComponent_Tribals Instance;

        public GameComponent_Tribals()
        {
            Instance = this;
        }

        public GameComponent_Tribals(Game game)
        {
            Instance = this;
        }

        public void IncrementCornerstonePoint()
        {
            availableCornerStonePoints++;
        }

        public override void FinalizeInit()
        {
            base.FinalizeInit();
            Log.Message("FinalizeInit: " + Faction.OfPlayer.def.techLevel);
        }

        public override void StartedNewGame()
        {
            base.StartedNewGame();
            Log.Message("StartedNewGame: " + Faction.OfPlayer.def.techLevel);
            playerTechLevel = Faction.OfPlayer.def.techLevel;
        }

        public override void ExposeData()
        {
            base.ExposeData();
            Scribe_Values.Look(ref availableCornerStonePoints, "availableCornerStonePoints");
        }
    }
}

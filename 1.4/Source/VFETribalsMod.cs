using HarmonyLib;
using RimWorld;
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
}

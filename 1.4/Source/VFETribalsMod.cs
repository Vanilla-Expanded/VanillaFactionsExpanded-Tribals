using HarmonyLib;
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

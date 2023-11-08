using HarmonyLib;
using RimWorld;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using Verse;

namespace VFETribals
{
    [HarmonyPatch(typeof(StatWorker), "GetValueUnfinalized")]
    public static class StatWorker_GetValueUnfinalized_Patch
    {
        public static void Postfix(ref float __result, StatWorker __instance, StatRequest req)
        {
            SetFactors(ref __result, __instance, req);
        }

        public static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> codeInstructions)
        {
            var codes = codeInstructions.ToList();
            var setOffsets = AccessTools.Method(typeof(StatWorker_GetValueUnfinalized_Patch), "SetOffsets");
            bool patched = false;
            for (var i = 0; i < codes.Count; i++)
            {
                var code = codes[i];
                yield return code;
                if (!patched && i > 1 && codes[i].opcode == OpCodes.Brfalse && codes[i - 1].opcode == OpCodes.Ldloc_1)
                {
                    yield return new CodeInstruction(OpCodes.Ldloca_S, 0);
                    yield return new CodeInstruction(OpCodes.Ldarg_0);
                    yield return new CodeInstruction(OpCodes.Ldarg_S, 1);
                    yield return new CodeInstruction(OpCodes.Call, setOffsets);
                    patched = true;
                }
            }
        }

        public static void SetOffsets(ref float __result, StatWorker __instance, StatRequest req)
        {
            if (CanApplyCornerstones(req))
            {
                foreach (var cornerstone in GameComponent_Tribals.Instance.cornerstones)
                {
                    if (cornerstone.statOffsets != null)
                    {
                        __result += cornerstone.statOffsets.GetStatOffsetFromList(__instance.stat);
                    }
                }
            }
        }

        private static void SetFactors(ref float __result, StatWorker __instance, StatRequest req)
        {
            if (CanApplyCornerstones(req))
            {
                foreach (var cornerstone in GameComponent_Tribals.Instance.cornerstones)
                {
                    if (cornerstone.statFactors != null)
                    {
                        __result *= cornerstone.statFactors.GetStatFactorFromList(__instance.stat);
                    }
                }
            }
        }

        public static bool CanApplyCornerstones(StatRequest req)
        {
            var playerFaction = Faction.OfPlayerSilentFail;
            if (playerFaction != null && GameComponent_Tribals.Instance?.cornerstones != null && req.Thing is Thing thing && thing.Faction == playerFaction)
            {
                return true;
            }
            return false;
        }
    }
}

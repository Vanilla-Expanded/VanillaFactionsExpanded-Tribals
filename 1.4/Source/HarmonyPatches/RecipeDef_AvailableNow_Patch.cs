using HarmonyLib;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using Verse;

namespace VFETribals
{
    [HarmonyPatch(typeof(RecipeDef), "AvailableNow", MethodType.Getter)]
    public static class RecipeDef_AvailableNow_Patch
    {
        public static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
        {
            var codes = instructions.ToList();
            for (var i = 0; i < codes.Count; i++)
            {
                var code = codes[i];
                yield return code;
                if (codes[i].opcode == OpCodes.Brtrue_S && codes[i - 1].ToString().Contains("unlockedByIdeo"))
                {
                    yield return new CodeInstruction(OpCodes.Ldarg_0);
                    yield return new CodeInstruction(OpCodes.Call, AccessTools.Method(typeof(RecipeDef_AvailableNow_Patch), nameof(BybassUnlocked)));
                    yield return new CodeInstruction(OpCodes.Brtrue_S, code.operand);
                }
            }
        }

        public static bool BybassUnlocked(RecipeDef recipe)
        {
            if (recipe.ProducedThingDef != null && DefDatabase<ResearchProjectDef>.AllDefs
                .Any(x => x.IsFinished && x.UnlockedDefs.Contains(recipe.ProducedThingDef)))
            {
                return true;
            }
            return false;
        }
    }
}

using HarmonyLib;
using RimWorld;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using Verse;

namespace VFETribals
{
    [HarmonyPatch(typeof(IdeoUIUtility), "DoIdeoList")]
    public static class IdeoUIUtilityDoIdeoList_Patch
    {
        public static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> codeInstructions)
        {
            var codes = codeInstructions.ToList();
            for (var i = 0; i < codes.Count; i++)
            {
                var code = codes[i];
                yield return code;
                if (code.opcode == OpCodes.Stfld && code.operand.ToString().Contains("gameStartConfig"))
                {
                    yield return new CodeInstruction(OpCodes.Ldloc_0);
                    yield return new CodeInstruction(OpCodes.Ldflda, code.operand);
                    yield return new CodeInstruction(OpCodes.Call, AccessTools.Method(typeof(IdeoUIUtilityDoIdeoList_Patch), nameof(SetGameStartConfig)));
                }
            }
        }
    
        public static void SetGameStartConfig(ref Page_ConfigureIdeo gameStartConfig)
        {
            if (Current.ProgramState != ProgramState.Playing)
            {
                if (Current.Game.Scenario?.playerFaction?.factionDef == VFET_DefOf.VFET_WildMen)
                {
                    gameStartConfig = null;
                }
            }
        }
    }
}

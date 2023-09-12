using HarmonyLib;
using RimWorld;
using System;
using System.Collections.Generic;
using System.Reflection.Emit;
using UnityEngine;
using Verse;

namespace VFETribals
{
    [HarmonyPatch(typeof(ResearchProjectDef), "UnlockedDefs", MethodType.Getter)]
    public static class ResearchProjectDef_UnlockedDefs_Patch
    {
        public static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
        {
            var field = AccessTools.Field(typeof(ResearchProjectDef), "cachedUnlockedDefs");
            foreach (var instruction in instructions)
            {
                yield return instruction;
                if (instruction.StoresField(field))
                {
                    yield return new CodeInstruction(OpCodes.Ldarg_0);
                    yield return new CodeInstruction(OpCodes.Call, 
                        AccessTools.Method(typeof(ResearchProjectDef_UnlockedDefs_Patch), "AddUnlockedDefs"));
                }
            }
        }
        public static void AddUnlockedDefs(ResearchProjectDef __instance)
        {
            if (__instance is TribalResearchProjectDef tribalResearch)
            {
                if (tribalResearch.unlocksDesignators != null)
                {
                    foreach (var type in tribalResearch.unlocksDesignators)
                    {
                        Designator designator = DesignatorUtility.StandaloneDesignators.TryGetValue(type);
                        if (designator == null)
                        {
                            designator = Activator.CreateInstance(type) as Designator;
                        }
                        __instance.cachedUnlockedDefs.Add(new FakeDef
                        {
                            defName = type.Name,
                            label = designator.LabelCap,
                            description = designator.Desc,
                            icon = designator.icon as Texture2D
                        });
                    }
                }

                if (tribalResearch.unlocksWorkTypes != null)
                {
                    foreach (var def in tribalResearch.unlocksWorkTypes)
                    {
                        __instance.cachedUnlockedDefs.Add(new FakeDef
                        {
                            defName = def.defName,
                            label = "VFET.WorkTypeName".Translate(def.pawnLabel),
                            description = def.description
                        });
                    }
                }
            }
        }
    }
}

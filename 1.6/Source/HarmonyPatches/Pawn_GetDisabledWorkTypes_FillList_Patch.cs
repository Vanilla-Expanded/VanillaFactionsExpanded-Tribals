using HarmonyLib;
using RimWorld;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using Verse;

namespace VFETribals
{
    [HarmonyPatch]
    public static class Pawn_GetDisabledWorkTypes_FillList_Patch
    {
        [HarmonyTargetMethod]
        public static MethodInfo TargetMethod()
        {
            foreach (var method in AccessTools.GetDeclaredMethods(typeof(Pawn)))
                if (method.Name.Contains("GetDisabledWorkTypes") && method.Name.Contains("FillList"))
                    return method;
            return null;
        }

        public static void Postfix(Pawn __instance, List<WorkTypeDef> list)
        {
            if (UnityData.IsInMainThread)
            {
                DisableWorkTypes(__instance, list);
            }
            else
            {
                LongEventHandler.ExecuteWhenFinished(delegate
                {
                    DisableWorkTypes(__instance, list);
                });
            }
        }

        private static void DisableWorkTypes(Pawn __instance, List<WorkTypeDef> list)
        {
            if (__instance.Faction == Faction.OfPlayerSilentFail)
            {
                foreach (var def in DefDatabase<WorkTypeDef>.AllDefs)
                {
                    if (list.Contains(def) is false && def.IsUnlocked(out _) is false)
                    {
                        list.Add(def);
                    }
                }
            }
        }
    }
}

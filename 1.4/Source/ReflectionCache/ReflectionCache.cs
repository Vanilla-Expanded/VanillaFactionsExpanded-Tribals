using HarmonyLib;
using RimWorld;
using Verse;
using System.Collections.Generic;



namespace VFETribals
{
    public class ReflectionCache
    {
        public static readonly AccessTools.FieldRef<ResearchManager, Dictionary<ResearchProjectDef, float>> progress =
           AccessTools.FieldRefAccess<ResearchManager, Dictionary<ResearchProjectDef, float>>(AccessTools.Field(typeof(ResearchManager), "progress"));
    }
}

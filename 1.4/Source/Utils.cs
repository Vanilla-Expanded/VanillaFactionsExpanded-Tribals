using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace VFETribals
{
    [StaticConstructorOnStartup]
    public static class Utils
    {
        public static List<TribalResearchProjectDef> researchProjectsWithDesignators = DefDatabase<TribalResearchProjectDef>
            .AllDefsListForReading.Where(x => x.unlocksDesignators != null).ToList();

        public static bool IsUnlocked(this WorkTypeDef workTypeDef)
        {
            return IsUnlocked(workTypeDef, out _);
        }

        public static bool IsUnlocked(this WorkTypeDef workTypeDef, out TribalResearchProjectDef research)
        {
            foreach (var tribalResearch in DefDatabase<TribalResearchProjectDef>.AllDefs)
            {
                if (tribalResearch.unlocksWorkTypes?.Contains(workTypeDef) ?? false)
                {
                    if (tribalResearch.IsFinished is false)
                    {
                        research = tribalResearch;
                        return false;
                    }
                }
            }
            research = null;
            return true;
        }

        public static bool IsUnlocked(this WorkTags workTags)
        {
            return IsUnlocked(workTags, out _);
        }

        public static bool IsUnlocked(this WorkTags workTags, out TribalResearchProjectDef research)
        {
            foreach (var tribalResearch in DefDatabase<TribalResearchProjectDef>.AllDefs)
            {
                if (tribalResearch.unlocksWorkTags.HasFlag(workTags))
                {
                    if (tribalResearch.IsFinished is false)
                    {
                        research = tribalResearch;
                        return false;
                    }
                }
            }
            research = null;
            return true;
        }
    }
}

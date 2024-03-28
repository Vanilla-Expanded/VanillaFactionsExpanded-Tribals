using System;
using System.Collections.Generic;
using Verse;

namespace VFETribals
{
    public class TribalResearchProjectDef : ResearchProjectDef
    {
        public List<WorkTypeDef> unlocksWorkTypes;
        public WorkTags unlocksWorkTags;
        public List<Type> unlocksDesignators;
    }
}

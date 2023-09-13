using RimWorld;
using Verse;

namespace VFETribals
{
    [DefOf]
    public static class VFET_DefOf
    {
        public static TribalResearchProjectDef VFET_Culture;
        [DefAlias("VFET_WildMen")] public static ScenarioDef VFET_WildMenScenario;
        public static FactionDef VFET_WildMen;
        public static LetterDef VFET_ConfigureIdeo, VFET_WildMenJoin;
        public static FactionDef VFET_WildMenGroup;
        public static SitePartDef VFET_WildMenSite;
        public static JobDef VFET_StandAndBeSociallyActive;

        public static SoundDef VFET_RitualSustainer_TribalGathering;
        public static SoundDef VFET_RitualSounds_TribalGathering;
        public static DesignationDef VFET_LightLargeFire, VFET_ExtinguishLargeFire;
        public static JobDef VFET_LightLargeFireJob, VFET_ExtinguishLargeFireJob;
        public static IncidentDef WildManWandersIn;
        public static ThingDef VFET_Stake;
        public static StorytellerDef VFET_TalonTribal;
        public static PreceptDef VFET_TribalGathering;
    }
}

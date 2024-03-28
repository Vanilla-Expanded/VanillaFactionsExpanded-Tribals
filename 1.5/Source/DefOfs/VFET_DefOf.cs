using RimWorld;
using Verse;

namespace VFETribals
{
    [DefOf]
    public static class VFET_DefOf
    {
        public static TribalResearchProjectDef VFET_Culture;
        public static FactionDef VFET_WildMen;
        public static FactionDef VFET_WildMenGroup;

        public static LetterDef VFET_ConfigureIdeo, VFET_WildMenJoin;
        
        public static SitePartDef VFET_WildMenSite;

        public static JobDef VFET_StandAndBeSociallyActive;
        public static JobDef VFET_LightLargeFireJob, VFET_ExtinguishLargeFireJob;

        public static SoundDef VFET_RitualSustainer_TribalGathering;
        public static SoundDef VFET_RitualSounds_TribalGathering;
        public static SoundDef VFET_RitualSustainer_MedievalGathering;
        public static SoundDef VFET_RitualSustainer_IndustrialGathering;
        public static SoundDef VFET_RitualSustainer_SpacerGathering;
        public static SoundDef VFET_RitualSustainer_UltraGathering;

        public static PawnKindDef VFET_Wildperson;

        public static DesignationDef VFET_LightLargeFire, VFET_ExtinguishLargeFire;
        
        public static IncidentDef WildManWandersIn;

        public static ThingDef VFET_Stake;

        public static StorytellerDef VFET_TalonTribal;

        public static EraAdvancementDef VFET_FormTribe;
        public static EraAdvancementDef VFET_FormTown;
        public static EraAdvancementDef VFET_FormCity;
        public static EraAdvancementDef VFET_FormCollective;
        public static EraAdvancementDef VFET_FormNexus;

        public static ThingDef VFET_FloorPainting;

        public static PreceptDef VFET_TribalGathering;
        public static PreceptDef VFET_AdvanceToNeolithic;
        public static PreceptDef VFET_AdvanceToMedieval;
        public static PreceptDef VFET_AdvanceToIndustrial;
        public static PreceptDef VFET_AdvanceToSpacer;
        public static PreceptDef VFET_AdvanceToUltra;
        public static ThingDef VFET_LargeFireSpark;
        public static IncidentDef StrangerInBlackJoin;
        [MayRequireIdeology] public static PreceptDef TreeConnection;
    }
}

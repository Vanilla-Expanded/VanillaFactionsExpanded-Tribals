using RimWorld;
using System.Collections.Generic;
using System.Diagnostics;
using Verse;

namespace VFETribals
{
    public class ChoiceLetter_ConfigureIdeo : ChoiceLetter
    {
        public override IEnumerable<DiaOption> Choices
        {
            get
            {
                yield return new DiaOption(IdeoPresetCategoryDefOf.Classic.LabelCap)
                {
                    action = () =>
                    {
                        Find.IdeoManager.classicMode = true;
                        Close();
                    }, 
                    resolveTree = true
                };
                yield return new DiaOption(IdeoPresetCategoryDefOf.Fluid.LabelCap)
                {
                    action = () =>
                    {
                        DoCustomize(fluid: true);
                    },
                    resolveTree = true
                };
                yield return new DiaOption(IdeoPresetCategoryDefOf.Custom.LabelCap)
                {
                    action = () =>
                    {
                        DoCustomize(fluid: false);
                    },
                    resolveTree = true
                };
            }
        }

        public void Close()
        {
            Find.Archive.Remove(this);
            Find.LetterStack.RemoveLetter(this);
            GameComponent_Tribals.Instance.TryRegisterAdvancementObligation();
        }

        private void DoCustomize(bool fluid = false)
        {
            Faction.OfPlayer.ideos.RemoveAll();
            Page_ConfigureIdeo page_ConfigureIdeo;
            if (fluid)
            {
                page_ConfigureIdeo = new Page_ConfigureFluidIdeo_Colonists
                {
                    letter = this
                };
                page_ConfigureIdeo.SelectOrMakeNewIdeo();
                page_ConfigureIdeo.ideo.Fluid = true;
            }
            else
            {
                page_ConfigureIdeo = new Page_ConfigureIdeo_Colonists
                {
                    letter = this
                };
            }
            Find.WindowStack.Add(page_ConfigureIdeo);
        }
    }
}

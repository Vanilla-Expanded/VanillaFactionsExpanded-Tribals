using RimWorld;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace VFETribals
{
    public class Dialog_EditEthos : Window
    {
        private string newDescription;

        private static readonly Vector2 ButSize = new Vector2(150f, 38f);

        private const float HeaderHeight = 35f;

        public override Vector2 InitialSize => new Vector2(700f, 400f);

        public Dialog_EditEthos()
        {
            newDescription = GameComponent_Tribals.Instance.ethos;
            absorbInputAroundWindow = true;
        }

        public override void OnAcceptKeyPressed()
        {
            Event.current.Use();
        }

        public Vector2 scrollPos;

        public static string TextAreaScrollable(Rect rect, string text, ref Vector2 scrollbarPosition, bool readOnly = false)
        {
            Rect rect2 = new Rect(0f, 0f, rect.width - 16f, Mathf.Max(Text.CalcHeight(text, rect.width) + 10f, rect.height));
            Widgets.BeginScrollView(rect, ref scrollbarPosition, rect2);
            string result = Widgets.TextArea(rect2, text, readOnly);
            Widgets.EndScrollView();
            return result;
        }

        public override void DoWindowContents(Rect rect)
        {
            Text.Font = GameFont.Medium;
            Widgets.Label(new Rect(rect.x, rect.y, rect.width, 35f), "VFET.EditEthos".Translate());
            Text.Font = GameFont.Small;
            float num = rect.y + 35f + 10f;
            string text = TextAreaScrollable(new Rect(rect.x, num, rect.width, rect.height - ButSize.y - 17f - num), newDescription, ref scrollPos);
            if (text != newDescription)
            {
                newDescription = text;
            }
            if (Widgets.ButtonText(new Rect(0f, rect.height - ButSize.y, ButSize.x, ButSize.y), "Cancel".Translate()))
            {
                Close();
            }
            if (Widgets.ButtonText(new Rect(rect.width / 2f - ButSize.x / 2f, rect.height - ButSize.y, ButSize.x, ButSize.y), 
                "Default".Translate()))
            {
                newDescription = GameComponent_Tribals.Instance.GetNewEthos();
                SoundDefOf.Tick_High.PlayOneShotOnCamera();
            }
            if (Widgets.ButtonText(new Rect(rect.width - ButSize.x, rect.height - ButSize.y, ButSize.x, ButSize.y), "DoneButton".Translate()))
            {
                ApplyChanges();
            }
        }

        private void ApplyChanges()
        {
            if (GameComponent_Tribals.Instance.ethos != newDescription)
            {
                GameComponent_Tribals.Instance.ethos = newDescription;
                GameComponent_Tribals.Instance.ethosLocked = true;
            }
            Close();
        }
    }
}

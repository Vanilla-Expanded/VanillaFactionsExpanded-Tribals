using RimWorld;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace VFETribals
{
    [StaticConstructorOnStartup]
    [HotSwappable]
    public class Window_CustomizeCornerStones : Window
    {
        public static readonly Texture2D UnlockedTex = ContentFinder<Texture2D>.Get("UI/Overlays/LockedMonochrome");

        public static readonly Texture2D LockedTex = ContentFinder<Texture2D>.Get("UI/Overlays/Locked");

        public override Vector2 InitialSize => new Vector2(700f, 760f);
        public override void DoWindowContents(Rect inRect)
        {
            var pos = new Vector2(15, 0);
            Text.Font = GameFont.Medium;
            var customizeCornerstonesRect = new Rect(pos.x, pos.y, inRect.width - pos.x, 32);
            Widgets.Label(customizeCornerstonesRect, "VFET.CustomizeCornerstones".Translate());
            pos.y += 50;
            var color = GUI.color;
            var factionName = Faction.OfPlayer.Name;
            var size = Text.CalcSize(factionName).x;
            Rect factionIcon = new Rect((inRect.width / 2f) - ((40 + size + 15) / 2f), pos.y + 1f, 40, 40);
            GUI.color = Faction.OfPlayer.Color;
            GUI.DrawTexture(factionIcon, Faction.OfPlayer.def.FactionIcon);
            GUI.color = color;
            Text.Anchor = TextAnchor.LowerLeft;
            Widgets.Label(new Rect(factionIcon.xMax + 15, factionIcon.y, size, factionIcon.height), factionName);
            Text.Font = GameFont.Small;
            Text.Anchor = TextAnchor.UpperLeft;

            pos.y += 50;
            DoEthos(ref pos.y, inRect.width - 50);
        }
        public static string ClickToEdit => "ClickToEdit".Translate().CapitalizeFirst().Colorize(ColorLibrary.Green);


        public static void DoEthos(ref float curY, float width)
        {
            var ethos = "VFET.Ethos".Translate();
            float num = 15;
            Rect rect = new Rect(num, curY, Text.LineHeight, Text.LineHeight);
            if (Widgets.ButtonImage(rect, GameComponent_Tribals.Instance.ethosLocked ? LockedTex : UnlockedTex))
            {
                GameComponent_Tribals.Instance.ethosLocked = !GameComponent_Tribals.Instance.ethosLocked;
                if (GameComponent_Tribals.Instance.ethosLocked)
                {
                    SoundDefOf.Checkbox_TurnedOn.PlayOneShotOnCamera();
                }
                else
                {
                    SoundDefOf.Checkbox_TurnedOff.PlayOneShotOnCamera();
                }
            }
            GUI.color = Color.white;
            if (Mouse.IsOver(rect))
            {
                string text = (GameComponent_Tribals.Instance.ethosLocked 
                    ? "VFET.EthosLocked" : "VFET.EthosUnlocked").Translate();
                TooltipHandler.TipRegion(rect, text);
            }

            float yMin = curY;
            Widgets.Label(num + Text.LineHeight + 4f, ref curY, width, "VFET.Ethos".Translate());
            float width2 = width - num - 80f;
            int num2 = (int)Mathf.Max(70f, Text.CalcHeight(GameComponent_Tribals.Instance.ethos, width2));
            Rect rect2 = new Rect(num + 40f, curY, width2, num2);
            Widgets.Label(rect2, GameComponent_Tribals.Instance.ethos);

            Rect rect3 = rect2;
            rect3.yMin = yMin;
            rect3.xMin = num + Text.LineHeight + 4f;
            if (Mouse.IsOver(rect3))
            {
                Widgets.DrawHighlight(rect3);
                TooltipHandler.TipRegion(rect2, string.Concat("VFET.EthosTooltip".Translate(), "\n\n", ClickToEdit));
            }
            if (Widgets.ButtonInvisible(rect3))
            {
                Find.WindowStack.Add(new Dialog_EditEthos());
            }

            curY += num2;
            curY += 17f;
        }
    }

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

        public override void DoWindowContents(Rect rect)
        {
            Text.Font = GameFont.Medium;
            Widgets.Label(new Rect(rect.x, rect.y, rect.width, 35f), "VFET.EditEthos".Translate());
            Text.Font = GameFont.Small;
            float num = rect.y + 35f + 10f;
            string text = Widgets.TextArea(new Rect(rect.x, num, rect.width, rect.height - ButSize.y - 17f - num), newDescription);
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

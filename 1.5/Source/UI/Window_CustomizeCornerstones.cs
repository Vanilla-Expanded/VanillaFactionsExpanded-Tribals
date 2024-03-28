using RimWorld;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace VFETribals
{
    [StaticConstructorOnStartup]
    [HotSwappable]
    public class Window_CustomizeCornerstones : Window
    {
        public static readonly Texture2D UnlockedTex = ContentFinder<Texture2D>.Get("UI/Overlays/LockedMonochrome");

        public static readonly Texture2D LockedTex = ContentFinder<Texture2D>.Get("UI/Overlays/Locked");

        public static string ClickToEdit => "ClickToEdit".Translate().CapitalizeFirst().Colorize(ColorLibrary.Green);

        public override Vector2 InitialSize => new Vector2(700f, 760f);

        private Vector2 scrollPos;
        private Vector2 scrollPos2;
        private float scrollHeight = 99999999;

        private List<CornerstoneDef> cornerstoneDefs = new List<CornerstoneDef>();

        public Window_CustomizeCornerstones()
        {
            FillCornerstoneDefs();
        }

        private void FillCornerstoneDefs()
        {
            cornerstoneDefs.Clear();
            foreach (var def in GameComponent_Tribals.Instance.cornerstones)
            {
                cornerstoneDefs.Add(def);
            }
            foreach (var def in DefDatabase<CornerstoneDef>.AllDefsListForReading)
            {
                if (!cornerstoneDefs.Contains(def))
                {
                    cornerstoneDefs.Add(def);
                }
            }
        }

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
            DoEthos(pos.y, inRect.width);
            pos.y += 70 + 15 + 15;
            var cornerstonesRect = new Rect(pos.x, pos.y, 150, 24);
            Widgets.Label(cornerstonesRect, "VFET.Cornerstones".Translate());
            pos.y += 24;
            var viewRect = new Rect(pos.x, pos.y, inRect.width - 16 - 15, scrollHeight);
            var outRect = new Rect(pos.x, pos.y, inRect.width - 15, 445);
            scrollHeight = 0;
            Widgets.BeginScrollView(outRect, ref scrollPos, viewRect);
            var localPos = pos;
            var entryHeight = 50;
            CornerstoneDef cornerstoneToUnlock = null;
            foreach (var def in cornerstoneDefs)
            {
                bool unlocked = GameComponent_Tribals.Instance.cornerstones.Contains(def);
                var entryRect = new Rect(localPos.x, localPos.y, viewRect.width, entryHeight);
                Widgets.DrawMenuSection(entryRect);
                if (unlocked)
                {
                    Widgets.DrawHighlightSelected(entryRect);
                }

                Text.Font = GameFont.Small;
                var labelRect = new Rect(localPos.x + 5, localPos.y + 3, viewRect.width, entryHeight - 24);
                Widgets.Label(labelRect, def.label);
                Text.Font = GameFont.Tiny;

                var descRect = new Rect(labelRect.x, labelRect.yMax, labelRect.width, 24);
                Widgets.Label(descRect, def.description);
                Text.Font = GameFont.Small;
                if (!unlocked && Mouse.IsOver(entryRect) && GameComponent_Tribals.Instance.availableCornerstonePoints > 0)
                {
                    var unlockRect = new Rect((entryRect.width - 200), entryRect.y + 10, 200, entryRect.height - 20);
                    if (Widgets.ButtonText(unlockRect, "VFET.UnlockCornerstone".Translate()))
                    {
                        cornerstoneToUnlock = def;
                    }
                }
                localPos.y += entryHeight + 15;
                scrollHeight += entryHeight + 15;
            }
            if (cornerstoneToUnlock != null)
            {
                GameComponent_Tribals.Instance.AddCornerstone(cornerstoneToUnlock);
                FillCornerstoneDefs();
            }
            Widgets.EndScrollView();
            Text.Font = GameFont.Medium;
            Text.Anchor = TextAnchor.UpperRight;
            var availableCornerstonePointsRect = new Rect(0, outRect.yMax + 5, 500 - 15, 28);
            Widgets.Label(availableCornerstonePointsRect,
                "VFET.AvailableCornerstonePoints".Translate(GameComponent_Tribals.Instance.availableCornerstonePoints));
            Text.Font = GameFont.Small;
            var cornerstonePointsExplanationRect = new Rect(availableCornerstonePointsRect.x, 
                availableCornerstonePointsRect.yMax, availableCornerstonePointsRect.width, 24);
            Widgets.Label(cornerstonePointsExplanationRect, "VFET.CornerstonesExplanation".Translate());
            Text.Anchor = TextAnchor.UpperLeft;
            var closeButtonRect = new Rect(availableCornerstonePointsRect.xMax + 15, 
                availableCornerstonePointsRect.y + 8, inRect.width - 500, 38f);
            if (Widgets.ButtonText(closeButtonRect, "Close".Translate()))
            {
                this.Close();
            }
        }


        public void DoEthos(float curY, float width)
        {
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
            int num2 = 70;
            Rect rect2 = new Rect(num + 40f, curY, width2, num2);
            Widgets.LabelScrollable(rect2, GameComponent_Tribals.Instance.ethos, ref scrollPos2);
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
        }
    }
}

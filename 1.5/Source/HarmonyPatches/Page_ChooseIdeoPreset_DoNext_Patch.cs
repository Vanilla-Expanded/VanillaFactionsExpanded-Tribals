using HarmonyLib;
using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace VFETribals
{
    [HarmonyPatch(typeof(Page_ChooseIdeoPreset), "DoNext")]
    public static class Page_ChooseIdeoPreset_DoNext_Patch
    {
        public static bool Prefix(Page_ChooseIdeoPreset __instance)
        {
            if (Current.Game.Scenario?.playerFaction?.factionDef == VFET_DefOf.VFET_WildMen)
            {
                Find.IdeoManager.classicMode = true;
                Faction.OfPlayer.ideos.SetPrimary(__instance.classicIdeo);
                Find.Scenario.PostIdeoChosen();
                __instance.next.prev = __instance.prev;
                if (__instance.nextAct != null)
                {
                    __instance.nextAct();
                }
                TutorSystem.Notify_Event("PageClosed");
                TutorSystem.Notify_Event("GoToNextPage");

                var page_ConfigureIdeo = new Page_ConfigureIdeo();
                page_ConfigureIdeo.prev = __instance.prev;
                page_ConfigureIdeo.next = __instance.next;
                __instance.ResetSelection();
                Find.WindowStack.Add(page_ConfigureIdeo);
                __instance.Close();
                page_ConfigureIdeo.ideo = __instance.classicIdeo;
                IdeoUIUtility.selected = Find.IdeoManager.IdeosListForReading.FirstOrDefault();
                return false;
            }
            return true;
        }
    }
}

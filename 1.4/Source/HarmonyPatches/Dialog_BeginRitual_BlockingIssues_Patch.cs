﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Verse;
using RimWorld;
using System.Reflection;
using System.Reflection.Emit;
using HarmonyLib;

namespace VFETribals
{




    [HarmonyPatch(typeof(Dialog_BeginRitual))]
    [HarmonyPatch("BlockingIssues")]

    public static class VFETribals_Dialog_BeginRitual_BlockingIssues_Patch
    {

        public static IEnumerable<string> Postfix(IEnumerable<string> values, Precept_Ritual ___ritual)
        {
            if (___ritual?.def == VFET_DefOf.VFET_TribalGathering)
            {
                List<string> resultingList = values.ToList();
               
                foreach (string issue in values)
                {
                    if (issue == "CantJoinRitualInExtremeWeather".Translate())
                    {
                        resultingList.Remove(issue);
                    }
                }
                return resultingList;
            }
            else return values;
           




        }

    }





}

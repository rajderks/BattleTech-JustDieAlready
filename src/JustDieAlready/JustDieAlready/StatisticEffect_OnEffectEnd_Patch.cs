using BattleTech;
using Harmony;
using UnityEngine;
using System;
using System.Diagnostics;
using System.Reflection;

namespace JustDieAlready
{
    [HarmonyPatch(typeof(StatisticEffect))]
    [HarmonyPatch("OnEffectEnd")]
    class StatisticEffect_OnEffectEnd_Patch
    {
        public static bool Prefix(StatisticEffect __instance, bool expired, bool skipLogging) 
        {
            try
            {
                FieldInfo[] fields = __instance.GetType().GetFields(
                         BindingFlags.NonPublic |
                         BindingFlags.Instance);
                foreach(FieldInfo fi in fields)
                {
                    var value = fi.GetValue(__instance);
                    if(JustDieAlready.GlobalSettings.debug) {
                        Logger.Log("--------------------------", false);
                        Logger.Log($"fi: {fi.ToString()}", false);
                        Logger.Log($"val: {value}", false);
                    }
                    // Typechecking BattleTech.StatCollection doesn't work so we do it by name
                    if (value == null && fi.ToString().Contains("BattleTech.StatCollection"))
                    {
                        return false;
                    }
                }
            } catch (Exception e)
            {
                Logger.Error(e);
                return true;
            }
            return true;
        }
    }
}



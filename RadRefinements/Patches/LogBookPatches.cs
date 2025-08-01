﻿using HarmonyLib;
using static RadRefinements.Configs;

namespace RadRefinements
{
    internal class LogBookPatches
    {
        private static int _currentPage = 0;

        [HarmonyPatch(typeof(MissionListUI))]
        private class MissionListUIPatches
        {
            [HarmonyPostfix]
            [HarmonyPatch("ToggleMenu")]
            public static void OpenLastOpenBookmark(MissionListUI __instance, bool ___UIActive)
            {
                if (!enableLogbookLastSection.Value)
                    return;

                if (___UIActive)                
                    __instance.SwitchMode((MissionListMode)_currentPage);                
            }
        }

        [HarmonyPatch(typeof(GPButtonLogMode))]
        private class GPButtonLogModePatches
        {
            [HarmonyPostfix]
            [HarmonyPatch("OnActivate")]
            public static void GetLastActivatedBookmark(MissionListMode ___mode)
            {
                if (!enableLogbookLastSection.Value)
                    return;

                _currentPage = (int)___mode;
            }
        }
    }
}

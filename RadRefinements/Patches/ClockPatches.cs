using HarmonyLib;
using System;
using System.Linq;
using UnityEngine;

namespace RadRefinements
{
    internal class ClockPatches
    {
        [HarmonyPatch(typeof(ShipItem))]
        private class ShipItemPatches
        {
            [HarmonyPrefix]
            [HarmonyPatch("Awake")]
            public static void AddTextMesh(ShipItem __instance)
            {
                if (__instance.name != "chronometer") return;

                var textObject = GameObject.Instantiate(DayLogs.instance.transform.parent.GetChild(0).GetChild(1));
                textObject.name = "clock_reading_text";
                textObject.SetParent(__instance.transform);
                textObject.gameObject.layer = 0;
                textObject.localEulerAngles = new Vector3(0, 0, 0);
                textObject.localPosition = new Vector3(0f, 0.3f, -0.2f);
                textObject.GetComponent<TextMesh>().color = new Color32(0xDB, 0xD6, 0xC9, 0x88);
                textObject.GetComponent<TextMesh>().fontSize = 55;
                textObject.GetComponent<TextMesh>().fontStyle = FontStyle.Bold;
                textObject.GetComponent<TextMesh>().anchor = TextAnchor.UpperCenter;

                textObject.gameObject.SetActive(false);
            }
        }

        [HarmonyPatch(typeof(ShipItemClock))]
        private class ShipItemClockPatches
        {
            [HarmonyPostfix]
            [HarmonyPatch("ExtraLateUpdate")]
            public static void AddReading(ShipItemClock __instance)
            {
                if ((!Plugin.enableClockGlobalText.Value && !Plugin.enableClockLocalText.Value) ||
                    !GameState.playing ||
                    GameState.currentlyLoading ||
                    GameState.loadingBoatLocalItems) 
                    return;

                var text = __instance.transform.GetComponentsInChildren<Transform>(true).FirstOrDefault(t => t.name == "clock_reading_text");
                if (text == null)
                    return;

                if (__instance.held != null ||
                    !__instance.sold ||
                    __instance.gameObject.layer == 5 ||
                    __instance.currentActualBoat == null ||
                    Vector3.Distance(Refs.observerMirror.transform.position, __instance.transform.position) > Plugin.clockViewableDistance.Value ||
                    Vector3.Angle(-__instance.transform.forward, Refs.observerMirror.transform.position - __instance.transform.position) > 85f ||
                    SpyglassPatches.heldAndUp)
                {
                    text.gameObject.SetActive(false);
                    return;
                }

                text.GetComponent<TextMesh>().text = GetReading();
                text.gameObject.SetActive(true);
            }
        }

        private static string GetReading()
        {
            var globalTime = Sun.sun.globalTime;
            var localTime = Sun.sun.localTime;

            if (!Plugin.enableClockLocalText.Value)
                return GetTime(globalTime);
            if (!Plugin.enableClockGlobalText.Value)
                return GetTime(localTime);

            return $"{GetTime(globalTime)}\n\n\n\n{GetTime(localTime)}";
        }

        private static string GetTime(float time)
        {
            var hours = (int)time;
            var minutes = Math.Round((time % 1) * 60) % 60;
            return $"{hours:00}:{minutes:00}";
        }
    }
}

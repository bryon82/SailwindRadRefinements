using HarmonyLib;
using System;
using System.Collections.Generic;
using UnityEngine;
using static RadRefinements.Configs;

namespace RadRefinements
{
    internal class ClockPatches
    {
        internal static Dictionary<ShipItemClock, TextMesh> ClockTextMeshes { get; } = new Dictionary<ShipItemClock, TextMesh>();

        [HarmonyPatch(typeof(ShipItem))]
        private class ShipItemPatches
        {
            [HarmonyPrefix]
            [HarmonyPatch("Awake")]
            public static void AddTextMesh(ShipItem __instance)
            {
                if (__instance.name != "chronometer")
                    return;

                var textObject = GameObject.Instantiate(DayLogs.instance.transform.parent.GetChild(0).GetChild(1));
                textObject.name = "clock_reading_text";
                textObject.SetParent(__instance.transform);
                textObject.gameObject.layer = 0;
                textObject.localEulerAngles = new Vector3(0, 0, 0);
                textObject.localPosition = new Vector3(0f, 0.3f, -0.2f);
                var textMesh = textObject.GetComponent<TextMesh>();
                textMesh.color = new Color32(0xDB, 0xD6, 0xC9, 0x88);
                textMesh.fontSize = 55;
                textMesh.fontStyle = FontStyle.Bold;
                textMesh.anchor = TextAnchor.UpperCenter;

                textObject.gameObject.SetActive(false);
                ClockTextMeshes.Add((ShipItemClock)__instance, textMesh);
            }
        }

        [HarmonyPatch(typeof(ShipItemClock))]
        private class ShipItemClockPatches
        {
            [HarmonyPostfix]
            [HarmonyPatch("ExtraLateUpdate")]
            public static void AddReading(ShipItemClock __instance)
            {
                if ((!enableClockGlobalText.Value && !enableClockLocalText.Value) ||
                    !GameState.playing ||
                    GameState.currentlyLoading ||
                    GameState.loadingBoatLocalItems) 
                    return;

                if (!ClockTextMeshes.TryGetValue(__instance, out var textMesh))
                    return;

                var observerPosition = Refs.observerMirror.transform.position;
                var clockPosition = __instance.transform.position;
                //var textRenderer = textMesh.GetComponent<Renderer>();
                //var textPosition = textRenderer != null ? textRenderer.bounds.center : text.position;
                //var directionToText = textPosition - observerPosition;
                //var distanceToText = directionToText.magnitude;

                if (__instance.held != null ||
                    !__instance.sold ||
                    __instance.gameObject.layer == 5 ||
                    __instance.currentActualBoat == null ||
                    Vector3.Distance(observerPosition, clockPosition) > clockViewableDistance.Value ||
                    Vector3.Angle(-__instance.transform.forward, observerPosition - clockPosition) > 85f ||
                    SpyglassPatches.HeldAndUp)// ||
                    //(distanceToText > 0.1f &&
                    //Physics.Raycast(observerPosition, directionToText.normalized, out var hit, distanceToText, ~0, QueryTriggerInteraction.Collide) &&
                    //!hit.transform.IsChildOf(__instance.transform)))
                {
                    textMesh.gameObject.SetActive(false);
                    return;
                }

                textMesh.text = GetReading();
                textMesh.gameObject.SetActive(true);
            }
        }

        private static string GetReading()
        {
            var globalTime = Sun.sun.globalTime;
            var localTime = Sun.sun.localTime;

            if (!enableClockLocalText.Value)
                return GetTime(globalTime);
            if (!enableClockGlobalText.Value)
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

using HarmonyLib;
using System;
using System.Collections.Generic;
using UnityEngine;
using static RadRefinements.Configs;

namespace RadRefinements
{
    internal class CompassPatches
    {
        internal static Dictionary<ShipItemCompass, Transform> CompassTexts { get; } = new Dictionary<ShipItemCompass, Transform>();

        [HarmonyPatch(typeof(ShipItemCompass))]
        private class ShipItemCompassPatches
        {
            [HarmonyPrefix]
            [HarmonyPatch("OnLoad")]
            public static void AddTextMesh(ShipItemCompass __instance)
            {
                if (__instance.name != "compass") return;

                var textObject = GameObject.Instantiate(DayLogs.instance.transform.parent.GetChild(0).GetChild(1));
                textObject.name = "compass_reading_text";
                textObject.SetParent(__instance.transform);
                textObject.gameObject.layer = 0;
                textObject.localEulerAngles = new Vector3(0f, 0f, 0f);
                textObject.localPosition = new Vector3(0f, 0.02f, 0f);
                textObject.GetComponent<TextMesh>().color = new Color32(0xDB, 0xD6, 0xC9, 0x88);
                textObject.GetComponent<TextMesh>().fontSize = 55;
                textObject.GetComponent<TextMesh>().fontStyle = FontStyle.Normal;
                textObject.GetComponent<TextMesh>().anchor = TextAnchor.LowerCenter;
                textObject.GetComponent<TextMesh>().lineSpacing = 0.7f;

                textObject.gameObject.SetActive(false);
                CompassTexts.Add(__instance, textObject);
            }

            [HarmonyPostfix]
            [HarmonyPatch("ExtraLateUpdate")]
            public static void AddReading(ShipItemCompass __instance)
            {
                if ((!enableCompassDegreesText.Value && !enableCompassCardinalText.Value) ||
                    !GameState.playing ||
                    GameState.currentlyLoading ||
                    GameState.loadingBoatLocalItems ||
                    __instance.name != "compass")
                    return;
                
                if (!CompassTexts.TryGetValue(__instance, out var text))
                    return;

                var observerPosition = Refs.observerMirror.transform.position;
                var compassPosition = __instance.transform.position;

                if ((__instance.held != null && !enableCompassReadingHeld.Value) ||
                    !__instance.sold ||
                    __instance.gameObject.layer == 5 ||
                    __instance.currentActualBoat == null ||
                    Vector3.Distance(observerPosition, compassPosition) > compassViewableDistance.Value ||
                    SpyglassPatches.HeldAndUp)
                {
                    text.gameObject.SetActive(false);
                    return;
                }
                
                //var textRenderer = text.GetComponent<Renderer>();
                //var textPosition = textRenderer != null ? textRenderer.bounds.center : text.position;
                //var directionToText = textPosition - observerPosition;
                //var distanceToText = directionToText.magnitude;
                //if (distanceToText > 0.1f &&
                //    Physics.Raycast(observerPosition, directionToText.normalized, out var hit, distanceToText, ~0, QueryTriggerInteraction.Collide) &&
                //    !hit.transform.IsChildOf(__instance.transform))
                //{
                //    text.gameObject.SetActive(false);
                //    return;
                //}

                var angleToPlayer = Vector3.SignedAngle(-__instance.transform.forward, observerPosition - compassPosition, Vector3.up);
                angleToPlayer = __instance.held != null ? 0f : angleToPlayer;
                text.localEulerAngles = new Vector3(0, angleToPlayer, 0);
                text.GetComponent<TextMesh>().fontSize = __instance.held != null ? 25 : 55;
                text.GetComponent<TextMesh>().text = GetReading(__instance.transform.eulerAngles.y);
                text.gameObject.SetActive(true);
            }

            private static string GetReading(float reading)
            {
                if (!enableCompassCardinalText.Value)
                {
                    return $"{Math.Round(reading)}°";
                }

                if (!enableCompassDegreesText.Value)
                {
                    return CompassRose.GetAbbreviatedDirection(reading, compassCardinalPrecisionLevel.Value);
                }

                return $"{CompassRose.GetAbbreviatedDirection(reading, compassCardinalPrecisionLevel.Value)}\n{Math.Round(reading)}°";
            }
        }
    }
}


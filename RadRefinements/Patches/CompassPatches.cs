using HarmonyLib;
using System;
using System.Collections.Generic;
using UnityEngine;
using static RadRefinements.Configs;

namespace RadRefinements
{
    internal class CompassPatches
    {
        internal static Dictionary<ShipItemCompass, TextMesh> CompassTextMeshes { get; } = new Dictionary<ShipItemCompass, TextMesh>();

        [HarmonyPatch(typeof(ShipItemCompass))]
        private class ShipItemCompassPatches
        {
            [HarmonyPrefix]
            [HarmonyPatch("OnLoad")]
            public static void AddTextMesh(ShipItemCompass __instance)
            {
                if (__instance.name != "compass")
                    return;

                var textObject = GameObject.Instantiate(DayLogs.instance.transform.parent.GetChild(0).GetChild(1));
                textObject.name = "compass_reading_text";
                textObject.SetParent(__instance.transform);
                textObject.gameObject.layer = 0;
                textObject.localEulerAngles = new Vector3(0f, 0f, 0f);
                textObject.localPosition = new Vector3(0f, 0.02f, 0f);
                var textMesh = textObject.GetComponent<TextMesh>();
                textMesh.color = new Color32(0xDB, 0xD6, 0xC9, 0x88);
                textMesh.fontSize = 55;
                textMesh.fontStyle = FontStyle.Normal;
                textMesh.anchor = TextAnchor.LowerCenter;
                textObject.GetComponent<TextMesh>().lineSpacing = 0.7f;

                textObject.gameObject.SetActive(false);
                CompassTextMeshes.Add(__instance, textMesh);
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
                
                if (!CompassTextMeshes.TryGetValue(__instance, out var textMesh))
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
                    textMesh.gameObject.SetActive(false);
                    return;
                }
                
                //var textRenderer = textMesh.GetComponent<Renderer>();
                //var textPosition = textRenderer != null ? textRenderer.bounds.center : textMesh.position;
                //var directionToText = textPosition - observerPosition;
                //var distanceToText = directionToText.magnitude;
                //if (distanceToText > 0.1f &&
                //    Physics.Raycast(observerPosition, directionToText.normalized, out var hit, distanceToText, ~0, QueryTriggerInteraction.Collide) &&
                //    !hit.transform.IsChildOf(__instance.transform))
                //{
                //    textMesh.gameObject.SetActive(false);
                //    return;
                //}

                var angleToPlayer = Vector3.SignedAngle(-__instance.transform.forward, observerPosition - compassPosition, Vector3.up);
                angleToPlayer = __instance.held != null ? 0f : angleToPlayer;
                textMesh.transform.localEulerAngles = new Vector3(0, angleToPlayer, 0);
                textMesh.fontSize = __instance.held != null ? 25 : 55;
                textMesh.text = GetReading(__instance.transform.eulerAngles.y);
                textMesh.gameObject.SetActive(true);
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


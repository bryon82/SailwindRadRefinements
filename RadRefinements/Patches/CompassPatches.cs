﻿using HarmonyLib;
using System;
using System.Linq;
using UnityEngine;

namespace RadRefinements
{
    internal class CompassPatches
    {
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
            }

            [HarmonyPostfix]
            [HarmonyPatch("ExtraLateUpdate")]
            public static void AddReading(ShipItemCompass __instance)
            {
                if ((!Plugin.enableCompassDegreesText.Value && !Plugin.enableCompassCardinalText.Value) ||
                    !GameState.playing ||
                    GameState.currentlyLoading ||
                    GameState.loadingBoatLocalItems ||
                    __instance.name != "compass")
                    return;

                var text = __instance.transform.GetComponentsInChildren<Transform>(true).FirstOrDefault(t => t.name == "compass_reading_text");
                if (text == null)
                    return;

                if ((__instance.held != null && !Plugin.enableCompassReadingHeld.Value) ||
                    !__instance.sold ||
                    __instance.gameObject.layer == 5 ||
                    __instance.currentActualBoat == null ||
                    Vector3.Distance(Refs.observerMirror.transform.position, __instance.transform.position) > Plugin.compassViewableDistance.Value ||
                    SpyglassPatches.heldAndUp)
                {
                    text.gameObject.SetActive(false);
                    return;
                }

                var angleToPlayer = Vector3.SignedAngle(-__instance.transform.forward, Refs.observerMirror.transform.position - __instance.transform.position, Vector3.up);
                angleToPlayer = __instance.held != null ? 0f : angleToPlayer;
                text.localEulerAngles = new Vector3(0, angleToPlayer, 0);
                text.GetComponent<TextMesh>().fontSize = __instance.held != null ? 25 : 55;
                text.GetComponent<TextMesh>().text = GetReading(__instance.transform.eulerAngles.y);
                text.gameObject.SetActive(true);
            }

            private static string GetReading(float reading)
            {
                if (!Plugin.enableCompassCardinalText.Value)
                {
                    return $"{Math.Round(reading)}°";
                }

                if (!Plugin.enableCompassDegreesText.Value)
                {
                    return CompassRose.GetAbbreviatedDirection(reading, Plugin.compassCardinalPrecisionLevel.Value);
                }

                return $"{CompassRose.GetAbbreviatedDirection(reading, Plugin.compassCardinalPrecisionLevel.Value)}\n{Math.Round(reading)}°";
            }
        }
    }
}


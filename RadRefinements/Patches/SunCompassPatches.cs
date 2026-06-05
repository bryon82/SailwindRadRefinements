using HarmonyLib;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static RadRefinements.Configs;
using static RadRefinements.RR_Plugin;

namespace RadRefinements
{
    internal class SunCompassPatches
    {
        internal static Dictionary<ShipItemCompass, TextMesh> SunCompassTextMeshes { get; } = new Dictionary<ShipItemCompass, TextMesh>();
        internal static Dictionary<ShipItemCompass, bool> SunCompassesInSun { get; } = new Dictionary<ShipItemCompass, bool>();        

        [HarmonyPatch(typeof(ShipItemCompass))]
        private class ShipItemCompassPatches
        {
            [HarmonyPrefix]
            [HarmonyPatch("OnLoad")]
            public static void AddTextMesh(ShipItemCompass __instance)
            {
                if (__instance.name != "sun compass")
                    return;

                var textObject = GameObject.Instantiate(DayLogs.instance.transform.parent.GetChild(0).GetChild(1));
                textObject.name = "sun_compass_reading_text";
                textObject.SetParent(__instance.transform);
                textObject.gameObject.layer = 0;
                textObject.localEulerAngles = new Vector3(45f, 0f, 0f);
                textObject.localPosition = new Vector3(0f, 0f, 0.18f);
                var textMesh = textObject.GetComponent<TextMesh>();
                textMesh.color = new Color32(0xDB, 0xD6, 0xC9, 0x88);
                textMesh.fontSize = 20;
                textMesh.fontStyle = FontStyle.Normal;
                textMesh.anchor = TextAnchor.LowerCenter;
                textMesh.lineSpacing = 0.7f;

                textObject.gameObject.SetActive(false);
                SunCompassTextMeshes.Add(__instance, textMesh);
                SunCompassesInSun.Add(__instance, false);
                __instance.StartCoroutine(CheckItemInSunlight(__instance));
            }

            [HarmonyPostfix]
            [HarmonyPatch("ExtraLateUpdate")]
            public static void AddReading(ShipItemCompass __instance)
            {
                

                if ((!enableSunCompassText.Value) ||
                    !GameState.playing ||
                    GameState.currentlyLoading ||
                    GameState.loadingBoatLocalItems ||
                    __instance.name != "sun compass" ||
                    !__instance.sold)
                    return;

                if (Input.GetKeyDown(KeyCode.P))
                {
                    for (int i = 0; i < Refs.islands.Count(); i++)
                    {
                        var island = Refs.islands[i];
                        LogDebug($"{island.name} index {i}");
                    }
                }

                if (!SunCompassTextMeshes.TryGetValue(__instance, out var textMesh))
                    return;

                if (__instance.held == null || __instance.gameObject.layer == 5 || !SunCompassesInSun[__instance])
                {
                    textMesh.gameObject.SetActive(false);
                    return;
                }

                var lat = FloatingOriginManager.instance.GetGlobeCoords(__instance.transform).z;                
                textMesh.text = $"{Math.Round(lat, 1)}°";
                textMesh.gameObject.SetActive(true);
            }
        }

        public static bool IsInSunlight(Transform t)
        {
            Vector3 directionToSun = -Sun.sun.transform.forward;
            int layerMask = Physics.DefaultRaycastLayers | (1 << 24);
            Vector3 rayOrigin = t.position + directionToSun * 0.01f;

            var hits = Physics.RaycastAll(rayOrigin, directionToSun, Mathf.Infinity, layerMask, QueryTriggerInteraction.Collide);
            foreach (var hit in hits)
                LogDebug($"Hit: {hit.collider.name}, layer: {hit.collider.gameObject.layer}, dist: {hit.distance}");

            bool blocked = Physics.Raycast(
                rayOrigin,
                directionToSun,
                Mathf.Infinity,
                layerMask,
                QueryTriggerInteraction.Collide
            );

            //LogDebug($"Checking {Sun.sun.name} sunlight for {t.name}: Ray origin {rayOrigin}, direction {directionToSun}, blocked: {blocked}");
            return !blocked;
        }

        internal static IEnumerator CheckItemInSunlight(ShipItem sunCompass)
        {
            var compass = sunCompass as ShipItemCompass;
            while (sunCompass != null)
            {
                var time = Sun.sun.localTime;
                if (compass.held != null && time >= 11 && time <= 13)
                {
                    SunCompassesInSun[compass] = IsInSunlight(compass.transform);
                    yield return new WaitForSeconds(0.5f);
                }
                else
                {
                    SunCompassesInSun[compass] = false;
                    yield return new WaitForSeconds(2f);
                }
            }
        }
    }
}

using HarmonyLib;
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
                if ((!Plugin.enableCompassDegreesText.Value && !Plugin.enableCompassCardinalText.Value) || !GameState.playing || GameState.currentlyLoading || __instance.name != "compass")
                    return;
                                
                var textMesh = __instance.transform.GetComponentsInChildren<Transform>(true).FirstOrDefault(t => t.name == "compass_reading_text");                
                if (__instance.held != null ||
                    !__instance.sold ||
                    __instance.gameObject.layer == 5 ||
                    __instance.currentActualBoat == null ||
                    Vector3.Distance(Refs.observerMirror.transform.position, __instance.transform.position) > Plugin.compassViewableDistance.Value)
                {                    
                    textMesh.gameObject.SetActive(false);
                    return;
                }

                var angleToPlayer = Vector3.SignedAngle(-__instance.transform.forward, Refs.observerMirror.transform.position - __instance.transform.position, Vector3.up);
                textMesh.localEulerAngles = new Vector3(0, angleToPlayer, 0);

                textMesh.GetComponent<TextMesh>().text = GetReading(__instance.transform.eulerAngles.y);
                textMesh.gameObject.SetActive(true);
            }

            private static string GetReading(float reading)
            {
                if (!Plugin.enableCompassCardinalText.Value)
                {
                    return $"{Math.Round(reading)}°";
                }

                if (!Plugin.enableCompassDegreesText.Value)
                {
                    return GetAbbreviatedDirection(reading, Plugin.compassCardinalPrecisionLevel.Value);
                }

                return $"{GetAbbreviatedDirection(reading, Plugin.compassCardinalPrecisionLevel.Value)}\n{Math.Round(reading)}°";
            }

            public static string GetCardinalDirection(float degrees, int precision)
            {
                if (precision == 4)
                {
                    string[] directions = { "North", "East", "South", "West", "North" };
                    int index = (int)Math.Round(degrees / 90.0) % 4;
                    return directions[index];
                }
                else if (precision == 8)
                {
                    string[] directions = {
                        "North", "Northeast", "East", "Southeast",
                        "South", "Southwest", "West", "Northwest", "North"
                    };
                    int index = (int)Math.Round(degrees / 45.0) % 8;
                    return directions[index];
                }
                else if (precision == 16)
                {
                    string[] directions = {
                        "North", "North-northeast", "Northeast", "East-northeast",
                        "East", "East-southeast", "Southeast", "South-southeast",
                        "South", "South-southwest", "Southwest", "West-southwest",
                        "West", "West-northwest", "Northwest", "North-northwest", "North"
                    };
                    int index = (int)Math.Round(degrees / 22.5) % 16;
                    return directions[index];
                }
                else // precision == 32
                {                    
                    string[] directions = {
                        "North", "North by east", "North-northeast", "Northeast by north",
                        "Northeast", "Northeast by east", "East-northeast", "East by north",
                        "East", "East by south", "East-southeast", "Southeast by east",
                        "Southeast", "Southeast by south", "South-southeast", "South by east",
                        "South", "South by west", "South-southwest", "Southwest by south",
                        "Southwest", "Southwest by west", "West-southwest", "West by south",
                        "West", "West by north", "West-northwest", "Northwest by west",
                        "Northwest", "Northwest by north", "North-northwest", "North by west", "North"
                    };
                    int index = (int)Math.Round(degrees / 11.25) % 32;
                    return directions[index];
                }
            }
                        
            public static string GetAbbreviatedDirection(float degrees, int precision)
            {
                string direction = GetCardinalDirection(degrees, precision);

                return direction
                    .ToLower()
                    .Replace("north", "N")
                    .Replace("east", "E")
                    .Replace("south", "S")
                    .Replace("west", "W")
                    .Replace("-", "")
                    .Replace(" by ", "b");
            }
        }
    }
}


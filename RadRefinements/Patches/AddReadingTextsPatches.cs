using HarmonyLib;
using System.Linq;
using UnityEngine;
using static RadRefinements.Configs;
using static RadRefinements.RR_Plugin;

namespace RadRefinements
{
    internal class AddReadingTextsPatches
    {
        [HarmonyPatch(typeof(ShipItem))]
        private class ShipItemPatches
        {
            private static Transform textMeshTemplate;

            [HarmonyPrefix]
            [HarmonyPatch("Awake")]
            public static void AddTextMesh(ShipItem __instance)
            {
                if (textMeshTemplate == null)
                    textMeshTemplate = DayLogs.instance.transform.parent.GetChild(0).GetChild(1);

                if (__instance.name == "chronometer")
                {
                    var zPos = __instance.transform.name == "172 clock M(Clone)" ? -0.075f : -0.2f;
                    var readingText = AddReadingText(__instance, new Vector3(0f, -0.3f, zPos));
                    readingText.isClock = true;
                }

                else if (__instance.name == "compass" || __instance.name == "bearing compass")
                {
                    var readingText = AddReadingText(__instance, new Vector3(0f, 0.03f, 0f));
                    readingText.textMesh.lineSpacing = 0.6f;
                    readingText.isCompass = true;
                    if (__instance.name == "bearing compass")
                    {
                        readingText.isCompassFlipped = true;
                        readingText.indicator1 = __instance.GetComponentsInChildren<Transform>(true)
                            .FirstOrDefault(t => t.name == "CompassCard");
                    }
                    else
                        readingText.indicator1 = __instance.GetComponentsInChildren<Transform>(true)
                            .FirstOrDefault(t => t.name == "compass_base");
                }

                else if (__instance.name == "sun compass")
                {
                    var readingText = AddReadingText(__instance, new Vector3(0f, 0f, 0.18f), new Vector3(45f, 0f, 0f));
                    readingText.textMesh.fontSize = 20;
                    readingText.isSunCompass = true;
                }

                else if (__instance.name == "quadrant")
                {
                    var parent = __instance.transform.GetComponentsInChildren<Transform>(true)
                            .FirstOrDefault(t => t.name == "dial");
                    var readingText = AddReadingText(
                        __instance, new Vector3(-0.025f, -0.19f, -0.03f), new Vector3(0, 90f, 270f), parent);                        
                    readingText.textMesh.fontSize = 40;
                    readingText.isQuadrant = true;
                }

                else if (__instance.name == "chip log")
                {
                    var readingText = AddReadingText(__instance, new Vector3(0f, 0.21f, -0.02f));
                    var pointerName = 
                        __instance.transform.name == "93 chip log E(Clone)" ? "pointer_001" : "pointer_002";
                    readingText.isChipLog = true;
                    readingText.indicator1 = __instance.GetComponentsInChildren<Transform>(true)
                        .FirstOrDefault(t => t.name == pointerName);
                }

                else if (__instance.name == "barometer")
                {
                    var readingText = AddReadingText(__instance, new Vector3(0f, 0.12f, -0.075f));
                    readingText.isBarometer = true;
                }

                else if (__instance.name == "thermometer")
                {
                    var readingText = AddReadingText(__instance, new Vector3(0f, 0.12f, -0.075f));
                    readingText.isThermometer = true;
                }

                else if (__instance.name == "hygrometer")
                {
                    var readingText = AddReadingText(__instance, new Vector3(0f, 0.12f, -0.075f));
                    readingText.isHygrometer = true;
                }

                else if (__instance.name == "inclinometer")
                {
                    var readingText = AddReadingText(__instance, new Vector3(0f, 0.12f, -0.05f));
                    readingText.isInclinometer = true;
                    readingText.indicator1 = __instance.GetComponentsInChildren<Transform>(true)
                        .FirstOrDefault(t => t.name == "Arm");
                }

                else if (__instance.name == "binnacle")
                {
                    var readingTextComp = AddReadingText(__instance, new Vector3(0f, 1.26f, 0f));
                    readingTextComp.textMesh.lineSpacing = 0.6f;
                    readingTextComp.isCompass = true;
                    readingTextComp.isCompassFlipped = true;
                    readingTextComp.indicator1 = __instance.GetComponentsInChildren<Transform>(true)
                        .FirstOrDefault(t => t.name == "CompassFace");

                    var readingTextInc = AddReadingText(__instance, new Vector3(0f, 0.9f, -0.201f));
                    readingTextInc.isInclinometer = true;
                    readingTextInc.indicator1 = __instance.GetComponentsInChildren<Transform>(true)
                        .FirstOrDefault(t => t.name == "InclinometerArm");
                }

                else if (__instance.name == "wind compass")
                {
                    var readingText = AddReadingText(__instance, new Vector3(0f, 0.12f, -0.075f));
                    readingText.textMesh.lineSpacing = 0.6f;
                    readingText.isWindCompass = true;
                    readingText.indicator1 = __instance.GetComponentsInChildren<Transform>(true)
                        .FirstOrDefault(t => t.name == "indicator");
                }

                else if (__instance.name == "anemometer" && __instance.transform.name == "514 anemometer B(Clone)")
                {
                    var readingText = AddReadingText(__instance, new Vector3(0f, 0.13f, -0.075f));
                    readingText.isAnemometer = true;
                    readingText.indicator1 = __instance.GetComponentsInChildren<Transform>(true)
                        .FirstOrDefault(t => t.name == "pointer_001 (1)");
                    readingText.indicator2 = __instance.GetComponentsInChildren<Transform>(true)
                        .FirstOrDefault(t => t.name == "plus_tumbler_3");
                }

                else if (__instance.name == "weathervane")
                {
                    Transform parent;
                    Vector3 pos;
                    if (__instance.transform.name.Contains("large"))
                    {
                        parent = __instance.transform.GetComponentsInChildren<Transform>(true)
                            .FirstOrDefault(t => t.name == "extension");
                        pos = new Vector3(0f, 0.4f, 0f);
                    }
                    else
                    {
                        parent = __instance.transform.GetComponentsInChildren<Transform>(true)
                            .FirstOrDefault(t => t.name == "pivot parent");
                        pos = new Vector3(0f, 0.5f, 0f);
                    }
                        
                    var readingText = AddReadingText(__instance, pos, parent: parent);
                    readingText.textMesh.lineSpacing = 0.6f;
                    readingText.isWeathervane = true;
                    readingText.indicator1 = __instance.GetComponentsInChildren<Transform>(true)
                        .FirstOrDefault(t => t.name == "wind_vane_arrow");
                }
            }

            private static ReadingText AddReadingText(ShipItem shipItem, Vector3 position, Vector3 rotation = default, Transform parent = null)
            {
                if (parent == null)
                    parent = shipItem.transform;
                var textObject = GameObject.Instantiate(textMeshTemplate);
                textObject.name = "reading_text";
                textObject.SetParent(parent);
                textObject.gameObject.layer = 0;
                textObject.localPosition = position;
                textObject.localEulerAngles = rotation;
                textObject.localScale = new Vector3(0.0114f, 0.0127f, 0.0127f);
                var textMesh = textObject.GetComponent<TextMesh>();
                textMesh.color = readingTextColor.Value;
                textMesh.fontSize = 55;
                textMesh.fontStyle = FontStyle.Normal;
                textMesh.anchor = TextAnchor.LowerCenter;
                textMesh.richText = true;
                textObject.gameObject.SetActive(false);

                var readingText = shipItem.gameObject.AddComponent<ReadingText>();
                readingText.textMesh = textMesh;
                readingText.shipItem = shipItem;
                return readingText;
            }
        }
    }
}

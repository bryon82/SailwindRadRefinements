using HarmonyLib;
using System.Linq;
using UnityEngine;
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
                    var textMesh = AddTextMesh(__instance.transform, new Vector3(0f, -0.3f, -0.2f), Vector3.zero);

                    var readingText = __instance.gameObject.AddComponent<ReadingText>();
                    readingText.readingTextMesh = textMesh;
                    readingText.shipItem = __instance;
                    readingText.isClock = true;
                }

                else if (__instance.name == "compass" || __instance.name == "bearing compass")
                {
                    var textMesh = AddTextMesh(__instance.transform, new Vector3(0f, 0.03f, 0f), Vector3.zero);
                    textMesh.lineSpacing = 0.6f;

                    var readingText = __instance.gameObject.AddComponent<ReadingText>();
                    readingText.readingTextMesh = textMesh;
                    readingText.shipItem = __instance;
                    readingText.isCompass = true;
                }

                else if (__instance.name == "sun compass")
                {
                    var textMesh = AddTextMesh(__instance.transform, new Vector3(0f, 0f, 0.18f), new Vector3(45f, 0f, 0f));
                    textMesh.fontSize = 20;

                    var readingText = __instance.gameObject.AddComponent<ReadingText>();
                    readingText.readingTextMesh = textMesh;
                    readingText.shipItem = __instance;
                    readingText.isSunCompass = true;
                }

                else if (__instance.name == "quadrant")
                {
                    var textMesh = AddTextMesh(
                        __instance.transform.GetComponentsInChildren<Transform>(true)
                            .FirstOrDefault(t => t.name == "dial"),
                        new Vector3(-0.025f, -0.19f, -0.03f),
                        new Vector3(0, 90f, 270f));
                    textMesh.fontSize = 40;

                    var readingText = __instance.gameObject.AddComponent<ReadingText>();
                    readingText.readingTextMesh = textMesh;
                    readingText.shipItem = __instance;
                    readingText.isQuadrant = true;
                }

                else if (__instance.name == "chip log")
                {
                    var textMesh = AddTextMesh(__instance.transform, new Vector3(0f, 0.21f, -0.02f), Vector3.zero);

                    var pointerName = "pointer_002";
                    if (__instance.transform.name == "93 chip log E(Clone)")
                        pointerName = "pointer_001";

                    var readingText = __instance.gameObject.AddComponent<ReadingText>();
                    readingText.readingTextMesh = textMesh;
                    readingText.shipItem = __instance;
                    readingText.isChipLog = true;
                    readingText.pointer = __instance.GetComponentsInChildren<Transform>(true)
                        .FirstOrDefault(t => t.name == pointerName);
                }

                else if (__instance.name == "barometer")
                {
                    var textMesh = AddTextMesh(__instance.transform, new Vector3(0f, 0.12f, -0.075f), Vector3.zero);

                    var readingText = __instance.gameObject.AddComponent<ReadingText>();
                    readingText.readingTextMesh = textMesh;
                    readingText.shipItem = __instance;
                    readingText.isBarometer = true;
                }

                else if (__instance.name == "thermometer")
                {
                    var textMesh = AddTextMesh(__instance.transform, new Vector3(0f, 0.12f, -0.075f), Vector3.zero);

                    var readingText = __instance.gameObject.AddComponent<ReadingText>();
                    readingText.readingTextMesh = textMesh;
                    readingText.shipItem = __instance;
                    readingText.isThermometer = true;
                }

                else if (__instance.name == "hygrometer")
                {
                    var textMesh = AddTextMesh(__instance.transform, new Vector3(0f, 0.12f, -0.075f), Vector3.zero);

                    var readingText = __instance.gameObject.AddComponent<ReadingText>();
                    readingText.readingTextMesh = textMesh;
                    readingText.shipItem = __instance;
                    readingText.isHygrometer = true;
                }

                else if (__instance.name == "inclinometer")
                {
                    var textMesh = AddTextMesh(__instance.transform, new Vector3(0f, 0.12f, -0.05f), Vector3.zero);

                    var readingText = __instance.gameObject.AddComponent<ReadingText>();
                    readingText.readingTextMesh = textMesh;
                    readingText.shipItem = __instance;
                    readingText.isInclinometer = true;
                    readingText.pointer = __instance.GetComponentsInChildren<Transform>(true)
                        .FirstOrDefault(t => t.name == "Arm");
                }
            }

            private static TextMesh AddTextMesh(Transform parent, Vector3 position, Vector3 rotation)
            {
                var textObject = GameObject.Instantiate(textMeshTemplate);
                textObject.name = "reading_text";
                textObject.SetParent(parent);
                textObject.gameObject.layer = 0;
                textObject.localPosition = position;
                textObject.localEulerAngles = rotation;
                textObject.localScale = new Vector3(0.0114f, 0.0127f, 0.0127f);
                var textMesh = textObject.GetComponent<TextMesh>();
                textMesh.color = new Color32(0xDB, 0xD6, 0xC9, 0x88);
                textMesh.fontSize = 55;
                textMesh.fontStyle = FontStyle.Normal;
                textMesh.anchor = TextAnchor.LowerCenter;
                textMesh.richText = true;
                textObject.gameObject.SetActive(false);

                return textMesh;
            }
        }
    }
}

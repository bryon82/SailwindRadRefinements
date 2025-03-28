using HarmonyLib;
using System;
using System.Linq;
using UnityEngine;

namespace RadRefinements
{
    internal class QuadrantPatches
    {
        [HarmonyPatch(typeof(ShipItemQuadrant))]
        private class ShipItemQuadrantPatches
        {
            [HarmonyPrefix]
            [HarmonyPatch("OnLoad")]
            public static void AddTextMesh(ShipItemQuadrant __instance)
            {
                var textObject = GameObject.Instantiate(DayLogs.instance.transform.parent.GetChild(0).GetChild(1));
                textObject.name = "quadrant_reading_text";                
                textObject.SetParent(__instance.transform.GetComponentsInChildren<Transform>(true).FirstOrDefault(t => t.name == "dial"));
                textObject.gameObject.layer = 2;
                textObject.localEulerAngles = new Vector3(0, 90f, 270f);
                textObject.localPosition = new Vector3(-0.025f, -0.19f, -0.03f);
                textObject.GetComponent<TextMesh>().color = new Color32(0xDB, 0xD6, 0xC9, 0x88);
                textObject.GetComponent<TextMesh>().fontSize = 15;
                textObject.GetComponent<TextMesh>().fontStyle = FontStyle.Normal;
                textObject.GetComponent<TextMesh>().richText = true;
                textObject.gameObject.SetActive(false);
            }

            [HarmonyPostfix]
            [HarmonyPatch("ExtraLateUpdate")]
            public static void AddReading(ShipItemQuadrant __instance)
            {
                if (!Plugin.enableQuadrantText.Value || !GameState.playing || GameState.currentlyLoading) 
                    return;

                var textMesh = __instance.transform.GetComponentsInChildren<Transform>(true).FirstOrDefault(t => t.name == "quadrant_reading_text");
                if (!__instance.GetPrivateField<bool>("inspecting") || __instance.GetPrivateField<bool>("rotating"))
                {
                    textMesh.gameObject.SetActive(false);
                    return;
                }
                var dial = __instance.GetPrivateField<Transform>("dial");
                var reading = Math.Round(dial.localEulerAngles.x, 2);                    
                textMesh.GetComponent<TextMesh>().text = $"{reading}°";
                textMesh.gameObject.SetActive(true);                   
            }
        }
    }
}


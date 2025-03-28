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
                textObject.localEulerAngles = new Vector3(0, 0, 0);
                textObject.localPosition = new Vector3(0f, 0.05f, 0f);
                textObject.GetComponent<TextMesh>().color = new Color32(0xDB, 0xD6, 0xC9, 0x88);
                textObject.GetComponent<TextMesh>().fontSize = 55;
                textObject.GetComponent<TextMesh>().fontStyle = FontStyle.Normal;

                textObject.gameObject.SetActive(false);
            }

            [HarmonyPostfix]
            [HarmonyPatch("ExtraLateUpdate")]
            public static void AddReading(ShipItemCompass __instance)
            {                
                if (!Plugin.enableCompassText.Value || !GameState.playing || GameState.currentlyLoading || __instance.name != "compass")
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

                var reading = Math.Round(__instance.transform.eulerAngles.y);
                textMesh.GetComponent<TextMesh>().text = $"{reading}°";
                textMesh.gameObject.SetActive(true);
            }
        }
    }
}


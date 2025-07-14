using HarmonyLib;
using System;
using System.Collections;
using System.Linq;
using UnityEngine;
using static RadRefinements.Configs;

namespace RadRefinements
{
    internal class QuadrantPatches
    {
        [HarmonyPatch(typeof(ShipItemQuadrant))]
        private class ShipItemQuadrantPatches
        {
            private static bool _rotating = false;

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
                if (!enableQuadrantText.Value || !GameState.playing || GameState.currentlyLoading || GameState.loadingBoatLocalItems) 
                    return;

                var text = __instance.transform.GetComponentsInChildren<Transform>(true).FirstOrDefault(t => t.name == "quadrant_reading_text");
                if (text == null)
                    return;

                if (!__instance.GetPrivateField<bool>("inspecting") || __instance.GetPrivateField<bool>("rotating"))
                {
                    text.gameObject.SetActive(false);
                    return;
                }
                var dial = __instance.GetPrivateField<Transform>("dial");
                var reading = Math.Round(dial.localEulerAngles.x, 2);
                text.GetComponent<TextMesh>().text = $"{reading}°";
                text.gameObject.SetActive(true);
            }

            [HarmonyPrefix]
            [HarmonyPatch("OnAltActivate")]
            public static bool ReverseLook(ShipItemQuadrant __instance, bool ___inspecting, Transform ___rotatingParent, ref Quaternion ___initialRot)
            {
                if (Input.GetKey(KeyCode.E) && !___inspecting && !_rotating) 
                {
                    __instance.StartCoroutine(Rotate180(___rotatingParent));
                    ___initialRot *= Quaternion.Euler(Vector3.up * 180f);                    
                    return false;
                }
                return true;
            }

            private static IEnumerator Rotate180(Transform rotatingParent)
            {
                _rotating = true;

                Quaternion startRotation = rotatingParent.rotation;                
                Quaternion rotation180 = Quaternion.AngleAxis(180f, Vector3.up);
                Quaternion targetRotation = startRotation * rotation180;

                float elapsedTime = 0;

                while (elapsedTime < 0.3f)
                {
                    float t = elapsedTime / 0.3f;                    
                    t = Mathf.SmoothStep(0, 1, t);
                    rotatingParent.rotation = Quaternion.Slerp(startRotation, targetRotation, t);

                    elapsedTime += Time.deltaTime;
                    yield return null;
                }

                rotatingParent.rotation = targetRotation;
                _rotating = false;
            }
        }        
    }
}


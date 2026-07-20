using HarmonyLib;
using System.Collections;
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


using HarmonyLib;
using UnityEngine;
using static RadRefinements.Configs;

namespace RadRefinements.Patches
{
    internal class OarPatches
    {

        [HarmonyPatch(typeof(ShipItemOar))]
        private class ShipItemOarPatches
        {
            internal static float addAmount = 0f;

            [HarmonyPrefix]
            [HarmonyPatch("ExtraLateUpdate")]
            public static void ReduceNeedsPrefix(bool ___isRowing)
            {
                if (___isRowing)
                {
                    addAmount = Time.deltaTime * rowingNeedsMult.Value;
                }
            }

            [HarmonyPostfix]
            [HarmonyPatch("ExtraLateUpdate")]
            public static void ReduceNeedsPostfix()
            {
                if (addAmount > 0f)
                {
                    PlayerNeeds.water += addAmount * 0.4f;
                    PlayerNeeds.food += addAmount * 0.6f;
                    addAmount = 0f;
                }
            }

            [HarmonyPostfix]
            [HarmonyPatch("OnAltHeld")]
            public static void ContinualRow(ShipItemOar __instance, ref bool ___isHoldingButton, bool ___isOverWater, ref float ___rowProgress, ref bool ___isRowing)
            {
                if (!continualRow.Value)
                    return;

                ___isHoldingButton = true;
                if ((bool)__instance.held && ___isOverWater && (bool)GameState.currentBoat)
                {
                    if (___rowProgress <= 1f)
                    {
                        ___isRowing = true;
                        ___rowProgress += Time.deltaTime;
                        GameState.currentBoat.parent.GetComponent<Rigidbody>().AddForceAtPosition(__instance.waterPos.forward * (0f - __instance.rowForce) * Time.deltaTime, __instance.waterPos.position);
                    }
                    else
                    {
                        ___isRowing = false;
                        ___rowProgress = 0f;
                    }
                }
            }
        }
    }
}

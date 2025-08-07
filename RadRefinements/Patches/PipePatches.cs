using HarmonyLib;
using System.Collections;
using UnityEngine;
using static RadRefinements.Configs;

namespace RadRefinements
{
    internal class PipePatches
    {
        private static int _clickedCount = 0;

        [HarmonyPatch(typeof(ShipItem), "OnAltActivate")]
        private class ShipItemPatches
        {
            public static void Postfix(ShipItem __instance)
            {
                if (enableSingleClickSmoking.Value && __instance is ShipItemPipe pipe && pipe.sold) 
                {
                    _clickedCount++;
                    __instance.StartCoroutine(MovePipe(pipe));
                }
            }

            private static IEnumerator MovePipe(ShipItemPipe pipe)
            {
                while (pipe.held && pipe.holdDistance > 0.25 && _clickedCount % 2 != 0 && pipe.amount > 0)
                {
                    pipe.holdDistance = Mathf.Lerp(pipe.holdDistance, 0.25f, Time.deltaTime * 4.44f);
                    pipe.holdHeight = Mathf.Lerp(pipe.holdHeight, pipe.height, Time.deltaTime * 4.44f);
                    pipe.heldRotationOffset = Mathf.Lerp(pipe.heldRotationOffset, 0f - pipe.maxRot, Time.deltaTime * pipe.rotRate);
                    pipe.SetPrivateField("inhaling", !PipeExhaleEffect.instance.exhaling);
                    pipe.SetPrivateField("drinking", true);
                    yield return null;
                }

                if (!pipe.held || pipe.amount <= 0)
                    _clickedCount = 0;
            }
        }        

        [HarmonyPatch(typeof(ShipItemPipe), "OnAltHeld")]
        public class ShipItemPipePatches
        {
            public static bool Prefix()
            {
                if (enableSingleClickSmoking.Value)
                    return false;

                return true;
            }
        }
    }
}

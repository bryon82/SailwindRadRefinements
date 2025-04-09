using Crest;
using HarmonyLib;
using UnityEngine;

namespace RadRefinements.Patches
{
    internal class FishingRodPatches
    {
        [HarmonyPatch(typeof(ShipItem))]
        private class ShipItemFishingHookPatches
        {
            [HarmonyPostfix]
            [HarmonyPatch("OnLoad")]
            public static void OnLoadRemoveDescription(ShipItem __instance)
            {
                if (!Plugin.removeItemHints.Value || !__instance.sold || !(__instance is ShipItemFishingHook))
                    return;
                
                __instance.description = "";                
            }

            [HarmonyPostfix]
            [HarmonyPatch("EnterBoat")]
            public static void EnterBoatRemoveDescription(ShipItem __instance)
            {
                if (!Plugin.removeItemHints.Value || !__instance.sold || !(__instance is ShipItemFishingHook))
                    return;

                __instance.description = "";
            }
        }

        /*
        [HarmonyPatch(typeof(ShipItemFishingRod))]
        private class ShipItemFishingRodPatches
        {
            [HarmonyPrefix]
            [HarmonyPatch("OnLoad")]
            public static void AddHangable(ShipItemFishingRod __instance)
            {
                __instance.gameObject.AddComponent<ShipItemHangable>();
                __instance.gameObject.AddComponent<HangableItem>();
                var hangableItem = __instance.gameObject.GetComponent<HangableItem>();
                hangableItem.SetPrivateField("rotX", -40f);
                hangableItem.SetPrivateField("rotY", 0f);
            }
        }

        [HarmonyPatch(typeof(FishingRodFish))]
        [HarmonyPatch("Update")]
        private class FishingRodFishPatches
        {
            [HarmonyPostfix]
            public static void Postfix(
                FishingRodFish __instance,
                ShipItemFishingRod ___rod,
                SimpleFloatingObject ___floater,
                ConfigurableJoint ___bobberJoint,
                ref float ___fishTimer)
            {
                if (__instance.currentFish != null ||
                    ___rod.health <= 0f ||
                    !___rod.gameObject.GetComponent<HangableItem>().IsHanging() ||
                    !___floater.InWater ||
                    ___bobberJoint.linearLimit.limit <= 1f ||
                    __instance.gameObject.layer == 16)
                {
                    return;
                }

                ___fishTimer -= Time.deltaTime;
                float value = Vector3.Distance(__instance.transform.position, ___rod.transform.position);
                float num = Mathf.InverseLerp(3f, 20f, value) * 2.5f + 0.5f;
                if (___fishTimer <= 0f)
                {
                    ___fishTimer = 1f;
                    if (Random.Range(0f, 100f) < num / 10f)
                    {
                        __instance.CatchFish();
                    }
                }
            }
        }
        */
    }
}

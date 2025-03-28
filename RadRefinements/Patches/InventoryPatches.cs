using cakeslice;
using HarmonyLib;
using UnityEngine;

namespace RadRefinements
{
    internal class InventoryPatches
    {
        [HarmonyPatch(typeof(LookUI))]
        private class LookUIPatches
        {
            [HarmonyPrefix]
            [HarmonyPatch("RegisterPointer")]
            public static void GetPointer(GoPointer goPointer)
            {
                SwapSlot.goPntr = goPointer;
            }

            [HarmonyPostfix]
            [HarmonyPatch("LateUpdate")]
            public static void LateUpdate()
            {
                if (!Plugin.enableQuickMap.Value)
                    return;

                if (Input.GetKeyDown(Plugin.quickMapButton.Value))
                {
                    ViewMap.ToggleMap();
                }
            }
        }

        [HarmonyPatch(typeof(GPButtonInventorySlot))]
        private class GPButtonInventorySlotPatches
        {
            private static bool swapSlotMade = false;

            [HarmonyPrefix]
            [HarmonyPatch("Awake")]
            public static void Awake(GPButtonInventorySlot __instance)
            {
                if (!swapSlotMade)
                {
                    swapSlotMade = true;
                    Plugin.logger.LogDebug("Creating swap slot.");
                    var inventory_parent = __instance.transform.parent;
                    var swapSlot = Object.Instantiate(inventory_parent.GetChild(0), inventory_parent);
                    swapSlot.name = "inventory_slot_swap";
                    Object.Destroy(swapSlot.GetComponent<MeshFilter>());
                    Object.Destroy(swapSlot.GetComponent<MeshRenderer>());
                    Object.Destroy(swapSlot.GetComponent<SphereCollider>());
                    Object.Destroy(swapSlot.GetComponent<GPButtonInventorySlot>());
                    Object.Destroy(swapSlot.GetComponent<Outline>());
                    swapSlot.gameObject.AddComponent<SwapSlot>();
                }
            }

            [HarmonyPrefix]
            [HarmonyPatch("OnItemClick")]
            public static void OnItemClickSwapItems(PickupableItem heldItem, ShipItem ___currentItem, GPButtonInventorySlot __instance)
            {
                if (!Plugin.enableInventorySwap.Value || !(bool)___currentItem)
                    return;

                SwapSlot.instance.SwapItems(heldItem, __instance);
            }
        }

    }
}

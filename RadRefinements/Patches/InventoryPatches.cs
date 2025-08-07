using cakeslice;
using HarmonyLib;
using UnityEngine;
using static RadRefinements.Configs;

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
                SwapSlot.GoPntr = goPointer;
            }

            [HarmonyPostfix]
            [HarmonyPatch("LateUpdate")]
            public static void ToggleQuickMap()
            {
                if (!enableQuickMap.Value || GameState.wasInSettingsMenu)
                    return;

                if (Input.GetKeyDown(quickMapKey.Value))
                {
                    ViewMap.ToggleMap();
                }
            }

            [HarmonyPostfix]
            [HarmonyPatch("LateUpdate")]
            public static void ToggleQuickSlot()
            {
                if (!enableQuickSlots.Value || GameState.wasInSettingsMenu)
                    return;

                if (Input.GetKeyDown(quickSlot1Key.Value))
                {
                    QuickSlots.ToggleInventoryItem(0);
                }
                if (Input.GetKeyDown(quickSlot2Key.Value))
                {
                    QuickSlots.ToggleInventoryItem(1);
                }
                if (Input.GetKeyDown(quickSlot3Key.Value))
                {
                    QuickSlots.ToggleInventoryItem(2);
                }
                if (Input.GetKeyDown(quickSlot4Key.Value))
                {
                    QuickSlots.ToggleInventoryItem(3);
                }
                if (Input.GetKeyDown(quickSlot5Key.Value))
                {
                    QuickSlots.ToggleInventoryItem(4);
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
                if (!enableInventorySwap.Value || !(bool)___currentItem)
                    return;

                SwapSlot.Instance.SwapItems(heldItem, __instance);
            }
        }

        [HarmonyPatch(typeof(CrateInventoryUI))]
        public class CrateInventoryUIPatches
        {
            [HarmonyPostfix]
            [HarmonyPatch("Update")]
            public static void AddItemDescription(CrateInventoryButton[] ___buttons)
            {
                if (!enableCrateItemDescription.Value)
                    return;

                foreach (var button in ___buttons)
                {
                    button.description = button.GetPrivateField<ShipItem>("currentItem")?.description ?? "";
                }
            }
        }
    }
}

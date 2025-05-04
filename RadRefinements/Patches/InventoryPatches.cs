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
                RR_SwapSlot.goPntr = goPointer;
            }

            [HarmonyPostfix]
            [HarmonyPatch("LateUpdate")]
            public static void ToggleQuickMap()
            {
                if (!RR_Plugin.enableQuickMap.Value || GameState.wasInSettingsMenu)
                    return;

                if (Input.GetKeyDown(RR_Plugin.quickMapKey.Value))
                {
                    ViewMap.ToggleMap();
                }
            }

            [HarmonyPostfix]
            [HarmonyPatch("LateUpdate")]
            public static void ToggleQuickSlot()
            {
                if (!RR_Plugin.enableQuickSlots.Value || GameState.wasInSettingsMenu)
                    return;

                if (Input.GetKeyDown(RR_Plugin.quickSlot1Key.Value))
                {
                    QuickSlots.ToggleInventoryItem(0);
                }
                if (Input.GetKeyDown(RR_Plugin.quickSlot2Key.Value))
                {
                    QuickSlots.ToggleInventoryItem(1);
                }
                if (Input.GetKeyDown(RR_Plugin.quickSlot3Key.Value))
                {
                    QuickSlots.ToggleInventoryItem(2);
                }
                if (Input.GetKeyDown(RR_Plugin.quickSlot4Key.Value))
                {
                    QuickSlots.ToggleInventoryItem(3);
                }
                if (Input.GetKeyDown(RR_Plugin.quickSlot5Key.Value))
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
                    swapSlot.gameObject.AddComponent<RR_SwapSlot>();
                }
            }

            [HarmonyPrefix]
            [HarmonyPatch("OnItemClick")]
            public static void OnItemClickSwapItems(PickupableItem heldItem, ShipItem ___currentItem, GPButtonInventorySlot __instance)
            {
                if (!RR_Plugin.enableInventorySwap.Value || !(bool)___currentItem)
                    return;

                RR_SwapSlot.Instance.SwapItems(heldItem, __instance);
            }
        }

        [HarmonyPatch(typeof(CrateInventoryUI))]
        public class CrateInventoryUIPatches
        {
            [HarmonyPostfix]
            [HarmonyPatch("Update")]
            public static void AddItemDescription(CrateInventoryButton[] ___buttons)
            {
                if (!RR_Plugin.enableCrateItemDescription.Value)
                    return;

                foreach (var button in ___buttons)
                {
                    button.description = button.GetPrivateField<ShipItem>("currentItem")?.description ?? "";
                }
            }
        }
    }
}

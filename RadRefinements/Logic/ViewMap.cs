using System.Linq;
using UnityEngine;

namespace RadRefinements
{
    internal class ViewMap
    {
        public static string[] MapNames = 
        {
            "ocean map",
            "Al'Ankh map",
            "Emerald Archipelago map",
            "Aestrin map",
            "Fire Fish Lagoon map"
        };

        public static int mapSlotIndex = 4;

        public static bool GetMapSlotIndex()
        {
            var slot = 
                GPButtonInventorySlot.inventorySlots
                .FirstOrDefault(s => s.currentItem && MapNames.Contains(s.currentItem.name));
            
            if (!slot)
                return false;

            mapSlotIndex = slot.slotIndex;
            return true;
        }

        public static void ToggleMap()
        {
            var goPointer = SwapSlot.goPntr;
            var heldItem = goPointer.GetHeldItem();

            var mapName = heldItem?.GetComponent<ShipItem>()?.name;
            if (heldItem && MapNames.Contains(mapName))
            {
                Debug.Log($"Stowing map: {mapName}");                
                QuickSlots.StowItem(mapSlotIndex, heldItem, goPointer);
            }
            else
            {
                var mapSlotIndexFound = GetMapSlotIndex();
                if (!mapSlotIndexFound)
                    return;
                Plugin.logger.LogDebug($"Displaying map: {GPButtonInventorySlot.inventorySlots[mapSlotIndex].currentItem.name}");
                QuickSlots.GetInventoryItem(mapSlotIndex, heldItem, goPointer);
                var map = goPointer.GetHeldItem().GetComponent<ShipItemFoldable>();
                if (map.amount > 0f)
                    map.InvokePrivateMethod("Unfold");
            }
        }
    }
}

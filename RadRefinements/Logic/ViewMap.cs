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

        public static GPButtonInventorySlot GetMapSlot()
        {
            var slot = 
                GPButtonInventorySlot.inventorySlots
                .FirstOrDefault(s => s.currentItem && MapNames.Contains(s.currentItem.name));
            
            if (!slot)
                return null;

            mapSlotIndex = slot.slotIndex;
            return slot;
        }

        public static void DisplayMap(PickupableItem heldItem, GoPointer goPointer)
        {
            if (!Plugin.enableInventorySwap.Value && heldItem)
                return;
            
            if (heldItem && heldItem.big)
                return;            

            var mapSlot = GetMapSlot();
            if (!mapSlot)
                return;
            var mapName = mapSlot.currentItem.name;            
            Plugin.logger.LogDebug($"Displaying map: {mapName}");            

            if (!heldItem)
            {
                mapSlot.OnActivate(goPointer);
            }
            else
            {
                mapSlot.OnItemClick(heldItem);
            }
            var map = goPointer.GetHeldItem().GetComponent<ShipItemFoldable>();
            if (map.amount > 0f)
                map.InvokePrivateMethod("Unfold");
        }

        public static void ToggleMap()
        {
            var goPointer = SwapSlot.goPntr;
            var heldItem = goPointer.GetHeldItem();

            var mapName = heldItem?.GetComponent<ShipItem>()?.name;
            if (heldItem && MapNames.Contains(mapName))
            {
                Debug.Log($"Stowing map: {mapName}");
                GPButtonInventorySlot.inventorySlots[mapSlotIndex].OnActivate();
                GPButtonInventorySlot.inventorySlots[mapSlotIndex].OnItemClick(goPointer.GetHeldItem());                
                goPointer.GetHeldItem().OnDrop();
                goPointer.DropItem();
            }
            else
            {
                DisplayMap(heldItem, goPointer);
            }
        }
    }
}

using System.Linq;
using static RadRefinements.RR_Plugin;

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

        private static int _mapSlotIndex = 4;

        public static int MapSlotIndex
        {
            get => _mapSlotIndex;
            set => _mapSlotIndex = value;
        }

        public static bool GetMapSlotIndex()
        {
            var slot = 
                GPButtonInventorySlot.inventorySlots
                .FirstOrDefault(s => s.currentItem && MapNames.Contains(s.currentItem.name));
            
            if (!slot)
                return false;

            _mapSlotIndex = slot.slotIndex;
            return true;
        }

        public static void ToggleMap()
        {
            var goPointer = RR_SwapSlot.GoPntr;
            var heldItem = goPointer.GetHeldItem();

            var mapName = heldItem?.GetComponent<ShipItem>()?.name;
            if (heldItem && MapNames.Contains(mapName))
            {
                LogDebug($"Stowing map: {mapName}");                
                QuickSlots.StowItem(_mapSlotIndex, heldItem, goPointer);
            }
            else
            {
                var mapSlotIndexFound = GetMapSlotIndex();
                if (!mapSlotIndexFound)
                    return;
                LogDebug($"Displaying map: {GPButtonInventorySlot.inventorySlots[_mapSlotIndex].currentItem.name}");
                QuickSlots.GetInventoryItem(_mapSlotIndex, heldItem, goPointer);
                var map = goPointer.GetHeldItem().GetComponent<ShipItemFoldable>();
                if (map.amount > 0f)
                    map.InvokePrivateMethod("Unfold");
            }
        }
    }
}

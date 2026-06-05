using System.Linq;
using static RadRefinements.RR_Plugin;
using static RadRefinements.Configs;

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
            var goPointer = SwapSlot.GoPntr;
            var heldItem = goPointer.GetHeldItem();

            var mapName = heldItem?.GetComponent<ShipItem>()?.name;
            if (heldItem && MapNames.Contains(mapName))
            {
                LogDebug($"Stowing map: {mapName}");

                var quickSlot = GPButtonInventorySlot.inventorySlots[_mapSlotIndex];
                quickSlot.OnActivate();
                quickSlot.OnItemClick(heldItem);
                goPointer.GetHeldItem().OnDrop();
                goPointer.DropItem();

                //QuickSlots.StowItem(_mapSlotIndex, heldItem, goPointer);
            }
            else
            {
                var mapSlotIndexFound = GetMapSlotIndex();
                if (!mapSlotIndexFound)
                    return;
                LogDebug($"Displaying map: {GPButtonInventorySlot.inventorySlots[_mapSlotIndex].currentItem.name}");
                //QuickSlots.GetInventoryItem(_mapSlotIndex, heldItem, goPointer);

                if (heldItem && (heldItem.big || !enableInventorySwap.Value))
                    return;

                var quickSlot = GPButtonInventorySlot.inventorySlots[_mapSlotIndex];
                if (!heldItem)
                {
                    quickSlot.OnActivate(goPointer);
                }
                else
                {
                    quickSlot.OnItemClick(heldItem);
                }
                var map = goPointer.GetHeldItem().GetComponent<ShipItemFoldable>();
                if (map.amount > 0f)
                    map.InvokePrivateMethod("Unfold");
            }
        }
    }
}

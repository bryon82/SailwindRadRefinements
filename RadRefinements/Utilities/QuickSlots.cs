using static RadRefinements.Configs;

namespace RadRefinements
{
    internal class QuickSlots
    {
        public static void GetInventoryItem(int slotIndex, PickupableItem heldItem, GoPointer goPointer)
        {
            if (heldItem && (heldItem.big || !enableInventorySwap.Value))
                return;

            var quickSlot = GPButtonInventorySlot.inventorySlots[slotIndex];
            if (!heldItem)
            {
                quickSlot.OnActivate(goPointer);
            }
            else
            {
                quickSlot.OnItemClick(heldItem);
            }
        }

        public static void StowItem(int slotIndex, PickupableItem heldItem, GoPointer goPointer)
        {
            var quickSlot = GPButtonInventorySlot.inventorySlots[slotIndex];
            quickSlot.OnActivate();
            quickSlot.OnItemClick(heldItem);
            goPointer.GetHeldItem().OnDrop();
            goPointer.DropItem();
        }

        public static void ToggleInventoryItem(int slotIndex)
        {
            var goPointer = SwapSlot.GoPntr;
            var heldItem = goPointer.GetHeldItem();

            if (heldItem)
            {
                StowItem(slotIndex, heldItem, goPointer);
            }
            else
            {
                GetInventoryItem(slotIndex, heldItem, goPointer);
            }
        }
    }
}

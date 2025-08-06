using System.Collections;
using System.Linq;
using UnityEngine;
using static RadRefinements.RR_Plugin;

namespace RadRefinements
{
    internal class SwapSlot : MonoBehaviour
    {
        public static SwapSlot Instance { get; private set; }
        internal static GPButtonInventorySlot Slot { get; private set; }
        internal static GoPointer GoPntr { get; set; }        

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }
            Instance = this;
            Slot = new GPButtonInventorySlot();
        }

        public void SwapItems(PickupableItem item, GPButtonInventorySlot invSlot)
        {
            var component = item.GetComponent<ShipItem>();
            if (!component.sold)
            {
                return;
            }

            if ((bool)component && !component.big)
            {
                var component2 = item.GetComponent<ShipItemBottle>();
                if ((bool)component2 && component2.GetCapacity() > 10f)
                {
                    return;
                }

                // insert into swap slot
                LogDebug($"Inserting {component.name} into swap slot.");
                Slot.currentItem = component;
                item.held = null;
                component.GetComponent<Collider>().enabled = false;
                Slot.currentItem.gameObject.layer = 5;
                Transform[] componentsInChildren = Slot.currentItem.GetComponentsInChildren<Transform>(true);
                for (int i = 0; i < componentsInChildren.Length; i++)
                {
                    componentsInChildren[i].gameObject.layer = 5;
                }

                // withdraw from inventory slot                
                var storedItem = invSlot.currentItem;
                LogDebug($"Withdrawing {storedItem.name} from inventory slot.");
                GoPntr.PickUpItem(storedItem);
                StartCoroutine(GrabItem(storedItem));

                // move from swap slot to inventory slot
                LogDebug($"Moving {Slot.currentItem.name} from swap slot to inventory slot.");
                Debug.Log("Inserting item.");
                Slot.currentItem.GetItemRigidbody().EnterInventorySlot(invSlot.transform);
                invSlot.currentItem = Slot.currentItem;
                UISoundPlayer.instance.PlayUISound(UISounds.itemInventoryIn, 0.2f, 1.36f);
                //invSlot.InsertItem(slot.currentItem); // used internals above to lower sound effect volume
                Slot.currentItem = null;

                LogDebug("Items Swapped");
            }
        }

        private IEnumerator GrabItem(ShipItem item)
        {
            yield return new WaitForEndOfFrame();
            
            GoPntr.SetPrivateField("heldItem",item);
            item.gameObject.layer = 2;
            GoPntr.GetPrivateField<PickupableItem>("heldItem").held = GoPntr;
            GoPntr.SetPrivateField("heldItemRot", Vector3.zero);
            GoPntr.SetPrivateField("timerAfterPickup", 0f);
            item.OnPickup();
            //goPntr.PickUpItem(item); // used internals above to remove sound effect
            GoPntr.transform.parent.GetComponent<LookUI>().SetPrivateField("currentButton", item.gameObject); 
        }

        internal static bool IsItemHeld()
        {
            return GoPntr.GetHeldItem() != null;
        }

        internal void WithdrawFromSwapSlot()
        {
            var storedItem = Slot.currentItem;
            LogDebug($"Withdrawing {storedItem.name} from inventory slot.");
            GoPntr.PickUpItem(storedItem);
            StartCoroutine(GrabItem(storedItem));
        }

        internal void SwapSlotToOpenInvSlot()
        {
            var openInvSlot = GPButtonInventorySlot.inventorySlots.FirstOrDefault(s => s.currentItem == null);
            Slot.currentItem.GetItemRigidbody().EnterInventorySlot(openInvSlot.transform);
            openInvSlot.currentItem = Slot.currentItem;
            Slot.currentItem = null;
        }
    }
}

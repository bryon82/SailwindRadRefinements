
using BepInEx.Logging;
using System.Collections;
using System.Linq;
using UnityEngine;

namespace RadRefinements
{
    internal class RR_SwapSlot : MonoBehaviour
    {
        public static RR_SwapSlot Instance { get; private set; }
        private static readonly ManualLogSource logger = RR_Plugin.logger;

        internal static GPButtonInventorySlot slot;
        internal static GoPointer goPntr;        

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }
            Instance = this;
            slot = new GPButtonInventorySlot();
        }

        public void SwapItems(PickupableItem item, GPButtonInventorySlot invSlot)
        {
            ShipItem component = item.GetComponent<ShipItem>();
            if (!component.sold)
            {
                return;
            }

            if ((bool)component && !component.big)
            {
                ShipItemBottle component2 = item.GetComponent<ShipItemBottle>();
                if ((bool)component2 && component2.GetCapacity() > 10f)
                {
                    return;
                }

                // insert into swap slot
                logger.LogDebug($"Inserting {component.name} into swap slot.");
                slot.currentItem = component;
                item.held = null;
                component.GetComponent<Collider>().enabled = false;
                slot.currentItem.gameObject.layer = 5;
                Transform[] componentsInChildren = slot.currentItem.GetComponentsInChildren<Transform>(true);
                for (int i = 0; i < componentsInChildren.Length; i++)
                {
                    componentsInChildren[i].gameObject.layer = 5;
                }

                // withdraw from inventory slot                
                var storedItem = invSlot.currentItem;
                logger.LogDebug($"Withdrawing {storedItem.name} from inventory slot.");
                goPntr.PickUpItem(storedItem);
                StartCoroutine(GrabItem(storedItem));

                // move from swap slot to inventory slot
                logger.LogDebug($"Moving {slot.currentItem.name} from swap slot to inventory slot.");
                Debug.Log("Inserting item.");
                slot.currentItem.GetItemRigidbody().EnterInventorySlot(invSlot.transform);
                invSlot.currentItem = slot.currentItem;
                UISoundPlayer.instance.PlayUISound(UISounds.itemInventoryIn, 0.2f, 1.36f);
                //invSlot.InsertItem(slot.currentItem); // used internals above to lower sound effect volume
                slot.currentItem = null;

                logger.LogDebug("Items Swapped");
            }
        }

        private IEnumerator GrabItem(ShipItem item)
        {
            yield return new WaitForEndOfFrame();
            
            goPntr.SetPrivateField("heldItem",item);
            item.gameObject.layer = 2;
            goPntr.GetPrivateField<PickupableItem>("heldItem").held = goPntr;
            goPntr.SetPrivateField("heldItemRot", Vector3.zero);
            goPntr.SetPrivateField("timerAfterPickup", 0f);
            item.OnPickup();
            //goPntr.PickUpItem(item); // used internals above to remove sound effect
            goPntr.transform.parent.GetComponent<LookUI>().SetPrivateField("currentButton", item.gameObject); 
        }

        internal static bool IsItemHeld()
        {
            return goPntr.GetHeldItem() != null;
        }

        internal void WithdrawFromSwapSlot()
        {
            var storedItem = slot.currentItem;
            logger.LogDebug($"Withdrawing {storedItem.name} from inventory slot.");
            goPntr.PickUpItem(storedItem);
            StartCoroutine(GrabItem(storedItem));
        }

        internal void SwapSlotToOpenInvSlot()
        {
            var openInvSlot = GPButtonInventorySlot.inventorySlots.FirstOrDefault(s => s.currentItem == null);
            slot.currentItem.GetItemRigidbody().EnterInventorySlot(openInvSlot.transform);
            openInvSlot.currentItem = slot.currentItem;
            slot.currentItem = null;
        }
    }
}

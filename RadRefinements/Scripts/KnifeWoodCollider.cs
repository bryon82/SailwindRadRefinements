using System.Linq;
using UnityEngine;
using static RadRefinements.RR_Plugin;

namespace RadRefinements
{
    internal class KnifeWoodCollider : MonoBehaviour
    {
        public ShipItem CurrentWood { get; private set; }

        private void Awake()
        {
            base.transform.parent.GetComponent<KnifeWood>().RegisterKnifeWoodCol(this);
        }

        public void OnTriggerEnter(Collider other)
        {
            var component = other.GetComponent<ShipItem>();
            if (component == null)
                return;

            LogDebug("KnifeCollider: entering wood col: " + component.name);
            if ((component is ShipItemBottle && component.health <= 0) ||
            (component is ShipItemCrate &&
            component.gameObject
                .GetComponent<ShipItemCrate>()
                ?.GetPrivateField<CrateInventory>("crateInventory")
                ?.containedItems.Count() <= 0))
            {
                CurrentWood = component;
            }
        }

        public void OnTriggerExit(Collider other)
        {
            if (other.GetComponent<ShipItem>() == CurrentWood)            
                CurrentWood = null;            
        }
    }
}

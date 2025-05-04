using BepInEx.Logging;
using System.Linq;
using UnityEngine;

namespace RadRefinements
{
    internal class RR_KnifeWoodCollider : MonoBehaviour
    {
        private static readonly ManualLogSource logger = RR_Plugin.logger;
        public ShipItem currentWood;

        private void Awake()
        {
            base.transform.parent.GetComponent<RR_KnifeWood>().RegisterKnifeWoodCol(this);
        }

        public void OnTriggerEnter(Collider other)
        {
            ShipItem component = other.GetComponent<ShipItem>();
            if (component == null)
                return;

            logger.LogDebug("KnifeCollider: entering wood col: " + component.name);
            if ((component is ShipItemBottle && component.health <= 0) ||
            (component is ShipItemCrate &&
            component.gameObject
                .GetComponent<ShipItemCrate>()
                ?.GetPrivateField<CrateInventory>("crateInventory")
                ?.containedItems.Count() <= 0))
            {
                currentWood = component;
            }
        }

        public void OnTriggerExit(Collider other)
        {
            if (other.GetComponent<ShipItem>() == currentWood)
            {
                currentWood = null;
            }
        }
    }
}

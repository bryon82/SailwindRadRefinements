using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace RadRefinements
{
    internal class KnifeWoodCollider : MonoBehaviour
    {
        public ShipItem currentWood;

        private void Awake()
        {
            base.transform.parent.GetComponent<KnifeWood>().RegisterKnifeWoodCol(this);
        }

        public void OnTriggerEnter(Collider other)
        {
            ShipItem component = other.GetComponent<ShipItem>();
            if (component is null)
                return;

            Plugin.logger.LogDebug("KnifeCollider: entering wood col: " + component.name);
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

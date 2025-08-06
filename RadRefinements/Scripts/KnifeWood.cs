using System.Collections;
using System.Linq;
using UnityEngine;

namespace RadRefinements
{
    internal class KnifeWood : MonoBehaviour
    {
        private KnifeWoodCollider _knifeWoodCol;
        
        public void RegisterKnifeWoodCol(KnifeWoodCollider col)
        {
            _knifeWoodCol = col;
        }

        internal void CutContainer(ShipItem container)
        {
            if ((container is ShipItemBottle && container.health > 0) ||
                (container is ShipItemCrate &&
                container.gameObject
                    .GetComponent<ShipItemCrate>()
                    ?.GetPrivateField<CrateInventory>("crateInventory")
                    ?.containedItems.Count() > 0) ||
                container.amount > 0)
            {
                return;
            }

            var key = container.gameObject.GetComponent<Good>()?.sizeDescription ?? container.name;
            var numPieces = Knife.WoodPiecesPerContainer[key];
            var num = -0.01f * numPieces;
            for (int i = 0; i < numPieces; i++)
            {
                var obj = Instantiate(PrefabsDirectory.instance.directory[71]);
                obj.transform.position = container.transform.position + container.transform.right * num;
                obj.transform.rotation = container.transform.rotation * Quaternion.Euler(0f, 90f, 0f);
                num += 0.02f;
                var component = obj.GetComponent<ShipItemStoveFuel>();
                component.sold = true;
                obj.GetComponent<SaveablePrefab>().RegisterToSave();
            }

            container.GetComponent<ShipItem>().DestroyItem();
            StartCoroutine(MakeLocalPosTracker());
        }

        internal IEnumerator MakeLocalPosTracker()
        {
            yield return new WaitForEndOfFrame();
            if (CrateInventoryUI.instance.GetPrivateField<Transform>("localPosTracker") == null)
                CrateInventoryUI.instance.SetPrivateField("localPosTracker", Instantiate(new GameObject()).transform);
        }
    }
}

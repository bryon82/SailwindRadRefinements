using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace RadRefinements
{
    internal class RR_KnifeWood : MonoBehaviour
    {
        private RR_KnifeWoodCollider _knifeWoodCol;

        internal static Dictionary<string, int> woodPiecesPerContainer =
            new Dictionary<string, int>
            {
                { "small crate", 4 },
                { "standard crate", 6 },
                { "large crate", 8 },
                { "very large crate", 10 },
                { "standard barrel", 6 },
                { "firewood", 4 },
                { "fishing hooks", 4 },
                { "lantern candles", 2 },
                { "green tobacco", 2 },
                { "blue tobacco", 2 },
                { "black tobacco", 2 },
                { "brown tobacco", 2 },
                { "white tobacco", 2 },
            };

        public void RegisterKnifeWoodCol(RR_KnifeWoodCollider col)
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
                    ?.containedItems.Count() > 0))
            {
                return;
            }

            var key = container.gameObject.GetComponent<Good>()?.sizeDescription ?? container.name;
            var numPieces = woodPiecesPerContainer[key];
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

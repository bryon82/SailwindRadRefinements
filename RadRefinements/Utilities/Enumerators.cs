using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace RadRefinements
{
    internal class Enumerators : MonoBehaviour
    {
        internal static Enumerators instance;

        private void Awake()
        {
            instance = this;
        }

        internal void MakeLocalPosTracker()
        {
            StartCoroutine(LocalPosTracker());
        }

        internal IEnumerator LocalPosTracker()
        {
            yield return new WaitForEndOfFrame();
            if (CrateInventoryUI.instance.GetPrivateField<Transform>("localPosTracker") == null)
                CrateInventoryUI.instance.SetPrivateField("localPosTracker", Instantiate(new GameObject()).transform);
        }
    }
}

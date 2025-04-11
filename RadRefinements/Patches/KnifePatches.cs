using HarmonyLib;
using UnityEngine;

namespace RadRefinements.Patches
{
    internal class KnifePatches
    {
        [HarmonyPatch(typeof(ShipItemKnife))]
        private class ShipItemKnifePatches
        {
            [HarmonyPostfix]
            [HarmonyPatch("OnAltActivate")]
            public static void KnifeCutContainer(
                ShipItemKnife __instance,
                ref bool ___animating,
                ref float ___heldRotationOffset,
                ref float ___cutTimer)
            {
                if (!Plugin.enableWoodFromContainers.Value || !__instance.sold)
                    return;
                
                var pointedAtItem = __instance.held.GetPointedAtItem();
                var good = pointedAtItem.gameObject.GetComponent<Good>();
                if ((bool)pointedAtItem &&
                    pointedAtItem.sold &&
                    (KnifeWood.woodPiecesPerContainer.ContainsKey(pointedAtItem.name) ||
                    (good != null && KnifeWood.woodPiecesPerContainer.ContainsKey(good.sizeDescription))))
                {
                    ___animating = true;
                    ___heldRotationOffset = 0f;
                    ___cutTimer = 0.25f;
                    var knifeWood = __instance.GetComponent<KnifeWood>();
                    knifeWood.CutContainer(pointedAtItem.GetComponent<ShipItem>());
                }                
            }

            [HarmonyPostfix]
            [HarmonyPatch("OnLoad")]
            public static void OnLoadPatch(ShipItemKnife __instance)
            {
                if (Plugin.removeItemHints.Value && __instance.sold)                    
                    __instance.description = "";

                __instance.gameObject.AddComponent<KnifeWood>();

            }
        }

        [HarmonyPatch(typeof(LookUI))]
        private class LookUIPatches
        {
            [HarmonyPrefix]
            [HarmonyPatch("ShowLookText")]
            public static bool AddKnifeControlsText(
                LookUI __instance,
                GoPointerButton button,
                GoPointer ___pointer,
                TextMesh ___textLicon,
                TextMesh ___textRIcon,
                ref bool ___showingIcon,
                TextMesh ___controlsText,
                TextMesh ___extraText)
            {
                if (!Plugin.enableWoodFromContainers.Value)
                    return true;

                ___extraText.text = button.lookText;
                __instance.transform.position = button.gameObject.transform.position;
                __instance.transform.LookAt(Camera.main.transform);
                ___textLicon.gameObject.SetActive(false);
                ___textRIcon.gameObject.SetActive(false);
                ___showingIcon = false;

                var good = button.gameObject.GetComponent<Good>();
                var shipItem = button.gameObject.GetComponent<ShipItem>();
                if ((bool)___pointer.GetHeldItem() &&
                    (bool)___pointer.GetHeldItem().GetComponent<ShipItemKnife>() &&
                    ((shipItem != null && KnifeWood.woodPiecesPerContainer.ContainsKey(shipItem.name)) ||
                    (good != null && KnifeWood.woodPiecesPerContainer.ContainsKey(good.sizeDescription))))
                {   
                    ___textRIcon.gameObject.SetActive(true);
                    ___showingIcon = true;
                    ___controlsText.text = "\ncut";
                    return false;                    
                }

                return true;
            }
        }
    }
}

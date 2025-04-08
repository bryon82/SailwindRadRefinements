using HarmonyLib;

namespace RadRefinements.Patches
{
    internal class KnifePatches
    {
        [HarmonyPatch(typeof(ShipItemKnife))]
        private class ShipItemKnifePatches
        {
            [HarmonyPostfix]
            [HarmonyPatch("OnAltActivate")]
            public static void KnifeCrate(
                ShipItemKnife __instance,
                ref bool ___animating,
                ref float ___heldRotationOffset,
                ref float ___cutTimer)
            {
                if (__instance.sold)
                {
                    ShipItem pointedAtItem = __instance.held.GetPointedAtItem();

                    if ((bool)pointedAtItem &&
                        pointedAtItem.sold &&
                        (Knife.woodPiecesPerContainer.ContainsKey(pointedAtItem.name) ||
                        (pointedAtItem.gameObject.GetComponent<Good>() != null &&
                        Knife.woodPiecesPerContainer.ContainsKey(pointedAtItem.gameObject.GetComponent<Good>().sizeDescription))))
                    {
                        ___animating = true;
                        ___heldRotationOffset = 0f;
                        ___cutTimer = 0.25f;
                        Knife.CutContainer(pointedAtItem.GetComponent<ShipItem>());
                    }
                }
            }
        }
    }
}

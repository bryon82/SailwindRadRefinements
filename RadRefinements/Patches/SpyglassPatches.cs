using HarmonyLib;

namespace RadRefinements
{
    internal class SpyglassPatches
    {
        internal static bool heldAndUp = false;

        [HarmonyPatch(typeof(ShipItemSpyglass))]
        private static class ShipItemSpyglassPatches
        {
            [HarmonyPostfix]
            [HarmonyPatch("OnAltActivate")]
            public static void DisableItemReadingsActivate(ShipItemSpyglass __instance, float ___heldRotationOffset)
            {
                if (__instance.sold && ___heldRotationOffset == -22f)
                {
                    heldAndUp = true;
                }
                else
                {
                    heldAndUp = false;
                }
            }

            [HarmonyPostfix]
            [HarmonyPatch("OnPickup")]
            public static void DisableItemReadingsOnPickup(ShipItemSpyglass __instance, float ___heldRotationOffset)
            {
                if (__instance.sold && ___heldRotationOffset == 0f)
                {
                    heldAndUp = true;
                }
                else
                {
                    heldAndUp = false;
                }
            }

            [HarmonyPostfix]
            [HarmonyPatch("OnEnterInventory")]
            public static void EnableItemReadingsInventory()
            {
                heldAndUp = false;
            }

            [HarmonyPostfix]
            [HarmonyPatch("OnDrop")]
            public static void EnableItemReadingsDrop()
            {
                heldAndUp = false;
            }
        }
    }
}

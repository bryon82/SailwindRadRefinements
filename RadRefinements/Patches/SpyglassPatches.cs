using HarmonyLib;

namespace RadRefinements
{
    internal class SpyglassPatches
    {
        internal static bool HeldAndUp { get; private set; } = false;

        [HarmonyPatch(typeof(ShipItemSpyglass))]
        private static class ShipItemSpyglassPatches
        {
            [HarmonyPostfix]
            [HarmonyPatch("OnAltActivate")]
            public static void DisableItemReadingsActivate(ShipItemSpyglass __instance, float ___heldRotationOffset)
            {
                if (__instance.sold && ___heldRotationOffset == -22f)
                {
                    HeldAndUp = true;
                }
                else
                {
                    HeldAndUp = false;
                }
            }

            [HarmonyPostfix]
            [HarmonyPatch("OnPickup")]
            public static void DisableItemReadingsOnPickup(ShipItemSpyglass __instance, float ___heldRotationOffset)
            {
                if (__instance.sold && ___heldRotationOffset == 0f)
                {
                    HeldAndUp = true;
                }
                else
                {
                    HeldAndUp = false;
                }
            }

            [HarmonyPostfix]
            [HarmonyPatch("OnEnterInventory")]
            public static void EnableItemReadingsInventory()
            {
                HeldAndUp = false;
            }

            [HarmonyPostfix]
            [HarmonyPatch("OnDrop")]
            public static void EnableItemReadingsDrop()
            {
                HeldAndUp = false;
            }
        }
    }
}

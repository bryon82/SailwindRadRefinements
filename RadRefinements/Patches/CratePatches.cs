using HarmonyLib;
using System.Linq;

namespace RadRefinements.Patches
{
    internal class CratePatches
    {
        [HarmonyPatch(typeof(ShipItemCrate))]
        private class ShipItemCratePatches
        {
            [HarmonyPostfix]
            [HarmonyPatch("UpdateLookText")]
            public static void AddNumItemsText(ShipItemCrate __instance, Good ___goodC, CrateInventory ___crateInventory)
            {
                if (!__instance.sold || __instance.amount > 0 || !(bool)___crateInventory || (bool)___goodC && ___goodC.GetMissionIndex() > -1)
                    return;

                __instance.lookText = $"{__instance.lookText}\n{___crateInventory.containedItems.Count()} items";
            }
        }
    }
}

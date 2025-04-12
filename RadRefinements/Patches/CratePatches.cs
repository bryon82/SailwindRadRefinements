using HarmonyLib;
using System.Linq;
using UnityEngine;

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
                if (!Plugin.enableCrateInvCountText.Value ||
                    !__instance.sold ||
                    __instance.amount > 0 ||
                    !(bool)___crateInventory ||
                    ((bool)___goodC &&
                    ___goodC.GetMissionIndex() > -1))
                {
                    return;
                }

                string inventoryText = string.Empty;
                if (Input.GetKey(Plugin.crateInvCountTextKey.Value))
                    inventoryText = Crate.GetCrateInventory(__instance);

                __instance.lookText = 
                    string.IsNullOrEmpty(inventoryText) ?
                    $"{__instance.lookText}\n{___crateInventory.containedItems.Count()} items" : 
                    inventoryText;
            }
        }
    }
}

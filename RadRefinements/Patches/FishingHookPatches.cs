using HarmonyLib;
using static RadRefinements.Configs;

namespace RadRefinements
{
    internal class FishingHookPatches
    {

        [HarmonyPatch(typeof(ShipItem))]
        private class ShipItemFishingHookPatches
        {
            [HarmonyPostfix]
            [HarmonyPatch("OnLoad")]
            public static void OnLoadRemoveDescription(ShipItem __instance)
            {
                if (removeItemHints.Value && __instance.sold && (__instance is ShipItemFishingHook))
                    __instance.description = "";
            }

            [HarmonyPostfix]
            [HarmonyPatch("EnterBoat")]
            public static void EnterBoatRemoveDescription(ShipItem __instance)
            {
                if (!removeItemHints.Value || !__instance.sold || !(__instance is ShipItemFishingHook))
                    return;

                __instance.description = "";
            }
        }
    }
}

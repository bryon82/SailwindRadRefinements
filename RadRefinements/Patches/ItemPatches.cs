using HarmonyLib;
using static RadRefinements.Configs;

namespace RadRefinements
{
    internal class ItemPatches
    {
        [HarmonyPatch(typeof(ShipItem), "OnLoad")]
        private class ShipItemPatches
        {
            public static void Postfix(ShipItem __instance)
            {
                if (__instance is ShipItemHammer && removeItemHints.Value && __instance.sold)
                    __instance.description = "";
            }
        }
    }
}

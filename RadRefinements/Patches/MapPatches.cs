using System;
using HarmonyLib;

namespace RadRefinements.Patches
{
    internal class MapPatches
    {
        [HarmonyPatch(typeof(ShipItemFoldable))]
        private class ShipItemFoldablePatches
        {
            [HarmonyPostfix]
            [HarmonyPatch("Fold")]
            public static void Postfix(ShipItemFoldable __instance)
            {
                __instance.description = __instance.name;
            }

            [HarmonyPostfix]
            [HarmonyPatch("Unfold")]
            public static void RemoveName(ShipItemFoldable __instance)
            {
                __instance.description = String.Empty;
            }
        }
    }
}

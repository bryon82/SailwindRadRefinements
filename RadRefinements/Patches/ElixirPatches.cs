using HarmonyLib;
using UnityEngine;
using static RadRefinements.Configs;

namespace RadRefinements.Patches
{
    internal class ElixirPatches
    {
        [HarmonyPatch(typeof(ShipItem), "Awake")]
        private static class ShipItemPatch
        {
            private static readonly Color32 snakeOilColor = new Color32(0xFF, 0xFF, 0x00, 0xFF);
            private static readonly Color32 energyElixirColor = new Color32(0x00, 0xFF, 0xFF, 0xFF);

            [HarmonyPostfix]
            public static void Postfix(ShipItem __instance)
            {
                if (!enableElixirColors.Value)
                    return;
                if (__instance is ShipItemElixir)
                {
                    var renderer = __instance.GetComponent<MeshRenderer>();
                    if (__instance.name == "snake oil")
                        renderer.materials[0].color = snakeOilColor;
                    else if (__instance.name == "energy elixir")
                        renderer.materials[0].color = energyElixirColor;
                }
            }
        }
    }
}

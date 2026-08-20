using HarmonyLib;
using UnityEngine;
using static RadRefinements.Configs;

namespace RadRefinements 
{
    internal class BlueTobaccoPatches
    {
        [HarmonyPatch(typeof(PrefabsDirectory), "PopulateShipItems")]
        private class SetBlueTobaccoType
        {            
            public static void Postfix(PrefabsDirectory __instance)
            {
                __instance.directory[318].GetComponent<ShipItemTobacco>().tobaccoType = 5;
            }
        }

        [HarmonyPatch(typeof(PlayerTobacco), "Smoke")]
        private class SetBlueTobaccoPotency
        {
            public static void Postfix(PlayerTobacco __instance, int tobaccoType)
            {
                if (tobaccoType == 5)
                {
                    if (!enableBlueTobacco.Value)
                    {
                        __instance.green += Time.deltaTime * 2f;
                        PlayerNeeds.sleep -= Time.deltaTime * 0.11f;
                        return;
                    }

                    __instance.green += Time.deltaTime * 2f * bluePotencyMult.Value * 0.33f;
                    PlayerNeeds.sleep -= Time.deltaTime * 0.11f * bluePotencyMult.Value;
                }
            }
        }
    }
}

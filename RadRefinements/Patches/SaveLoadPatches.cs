﻿using HarmonyLib;
using ModSaveBackups;
using System;
using UnityEngine;

namespace RadRefinements
{   
    internal class SaveLoadPatches
    {

        [HarmonyPatch(typeof(SaveLoadManager))]
        private class SaveLoadManagerPatches
        {
            [HarmonyPostfix]
            [HarmonyPatch("SaveModData")]
            public static void DoSaveGamePatch()
            {
                var saveContainer = new RadRefinementsSaveContainer();

                saveContainer.swapSlotHasItem = SwapSlot.slot.currentItem != null;
                saveContainer.mapSlotIndex = ViewMap.mapSlotIndex;

                ModSave.Save(Plugin.instance.Info, saveContainer);
            }
        }

        [HarmonyPostfix]
        [HarmonyPatch("LoadModData")]
        public static void LoadModDataPatch()
        {
            if (!ModSave.Load(Plugin.instance.Info, out RadRefinementsSaveContainer saveContainer))
            {
                Plugin.logger.LogWarning("Save file loading failed. If this is the first time loading this save with this mod, this is normal.");
                return;
            }

            ViewMap.mapSlotIndex = saveContainer.mapSlotIndex;

            if (saveContainer.swapSlotHasItem)
            {
                Plugin.logger.LogWarning("Loaded game with item in swap slot, moving it to held or open inventory slot");
                if (SwapSlot.IsItemHeld())
                {
                    SwapSlot.instance.SwapSlotToOpenInvSlot();
                }
                else
                {
                    SwapSlot.instance.WithdrawFromSwapSlot();
                }
            }
        }
    }

    [Serializable]
    public class RadRefinementsSaveContainer
    {
        public bool swapSlotHasItem;
        public int mapSlotIndex;
    }
    
}

﻿using HarmonyLib;
using ModSaveBackups;
using System;
using static RadRefinements.RR_Plugin;

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
                var saveContainer = new RadRefinementsSaveContainer
                {
                    swapSlotHasItem = RR_SwapSlot.Slot.currentItem != null,
                    mapSlotIndex = ViewMap.MapSlotIndex
                };

                ModSave.Save(Instance.Info, saveContainer);
            }

            [HarmonyPostfix]
            [HarmonyPatch("LoadModData")]
            public static void LoadModDataPatch()
            {
                if (!ModSave.Load(Instance.Info, out RadRefinementsSaveContainer saveContainer))
                {
                    LogWarning("Save file loading failed. If this is the first time loading this save with this mod, this is normal.");
                    return;
                }

                ViewMap.MapSlotIndex = saveContainer.mapSlotIndex;                    

                if (saveContainer.swapSlotHasItem)
                {
                    LogWarning("Loaded game with item in swap slot, moving it to held or open inventory slot");
                    if (RR_SwapSlot.IsItemHeld())
                    {
                        RR_SwapSlot.Instance.SwapSlotToOpenInvSlot();
                    }
                    else
                    {
                        RR_SwapSlot.Instance.WithdrawFromSwapSlot();
                    }
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

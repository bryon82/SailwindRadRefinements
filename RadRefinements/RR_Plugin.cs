﻿using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;
using HarmonyLib;
using System.Reflection;
using UnityEngine;

namespace RadRefinements
{
    [BepInPlugin(PLUGIN_GUID, PLUGIN_NAME, PLUGIN_VERSION)]
    [BepInDependency(MODSAVEBACKUPS_GUID, MODSAVEBACKUPS_VERSION)]
    public class RR_Plugin : BaseUnityPlugin
    {
        public const string PLUGIN_GUID = "com.raddude82.radrefinements";
        public const string PLUGIN_NAME = "RadRefinements";
        public const string PLUGIN_VERSION = "1.0.14";

        public const string MODSAVEBACKUPS_GUID = "com.raddude82.modsavebackups";
        public const string MODSAVEBACKUPS_VERSION = "1.1.1";

        internal static RR_Plugin Instance { get; set; }
        internal static ManualLogSource logger;

        internal static ConfigEntry<bool> enableInventorySwap;
        internal static ConfigEntry<bool> enableQuickMap;
        internal static ConfigEntry<KeyCode> quickMapKey;
        internal static ConfigEntry<bool> enableQuickSlots;
        internal static ConfigEntry<KeyCode> quickSlot1Key;
        internal static ConfigEntry<KeyCode> quickSlot2Key;
        internal static ConfigEntry<KeyCode> quickSlot3Key;
        internal static ConfigEntry<KeyCode> quickSlot4Key;
        internal static ConfigEntry<KeyCode> quickSlot5Key;
        internal static ConfigEntry<bool> enableCrateItemDescription;
        internal static ConfigEntry<bool> enableQuadrantText;
        internal static ConfigEntry<bool> enableCompassReadingHeld;
        internal static ConfigEntry<bool> enableCompassDegreesText;
        internal static ConfigEntry<bool> enableCompassCardinalText;  
        internal static ConfigEntry<int> compassCardinalPrecisionLevel;
        internal static ConfigEntry<float> compassViewableDistance;
        internal static ConfigEntry<bool> enableClockGlobalText;
        internal static ConfigEntry<bool> enableClockLocalText;
        internal static ConfigEntry<float> clockViewableDistance;
        internal static ConfigEntry<bool> enableWoodFromContainers;
        internal static ConfigEntry<bool> enableCrateInvCountText;
        internal static ConfigEntry<KeyCode> crateInvCountTextKey;
        internal static ConfigEntry<bool> removeItemHints;
        internal static ConfigEntry<bool> enableFishMovement;

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }
            Instance = this;
            logger = Logger;

            enableInventorySwap = Config.Bind("Inventory Settings", "Enable inventory swap", true, "Allows you to swap the item you are holding with the item in your selected inventory slot.");
            enableQuickMap = Config.Bind("Inventory Settings", "Enable quick map", true, "On quick map key press, causes your character to hold the leftmost map that is in your inventory slots or to put it back in the inventory slot it came from.");
            quickMapKey = Config.Bind("Inventory Settings", "Quick map key", KeyCode.M, "Key that retrieves or stows your map when pressed");
            enableQuickSlots = Config.Bind("Inventory Settings", "Enable quick slots", true, "Enables the ability to retrieve items from your inventory when a key is pressed.");
            quickSlot1Key = Config.Bind("Inventory Settings", "Quick slot 1 key", KeyCode.Alpha1, "Key that retrieves from or stows to inventory slot 1 when pressed");
            quickSlot2Key = Config.Bind("Inventory Settings", "Quick slot 2 key", KeyCode.Alpha2, "Key that retrieves from or stows to inventory slot 2 when pressed");
            quickSlot3Key = Config.Bind("Inventory Settings", "Quick slot 3 key", KeyCode.Alpha3, "Key that retrieves from or stows to inventory slot 3 when pressed");
            quickSlot4Key = Config.Bind("Inventory Settings", "Quick slot 4 key", KeyCode.Alpha4, "Key that retrieves from or stows to inventory slot 4 when pressed");
            quickSlot5Key = Config.Bind("Inventory Settings", "Quick slot 5 key", KeyCode.Alpha5, "Key that retrieves from or stows to inventory slot 5 when pressed");
            enableCrateItemDescription = Config.Bind("Inventory Settings", "Enable crate item description", true, "Allows you to see the text you would see when looking at the item out of the crate.");
            enableQuadrantText = Config.Bind("Item Text Settings", "Enable quadrant reading text", true, "Enables the text that shows the quadrant reading value.");
            enableCompassReadingHeld = Config.Bind("Item Text Settings", "Enable compass reading while held", false, "Enables the text that shows the compass reading while holding the compass.");
            enableCompassDegreesText = Config.Bind("Item Text Settings", "Enable compass reading degrees text", true, "Enables the text that shows the compass reading value in degrees.");
            enableCompassCardinalText = Config.Bind("Item Text Settings", "Enable compass reading cardinal text", true, "Enables the text that shows the compass reading value in cardinal directions.");
            compassCardinalPrecisionLevel = Config.Bind("Item Text Settings", "Number of compass ordinal directions", 16, new ConfigDescription("Number of ordinal directions given in the compass reading.", new AcceptableValueList<int>(4, 8, 16, 32)));
            compassViewableDistance = Config.Bind("Item Text Settings", "Compass viewable distance", 3f, "Sets the how close player needs to be to see compass reading text");
            enableClockGlobalText = Config.Bind("Item Text Settings", "Enable clock global time text", true, "Enables the text that shows the global time on the clock.");
            enableClockLocalText = Config.Bind("Item Text Settings", "Enable clock local time text", true, "Enables the text that shows the clock time on the clock.");
            clockViewableDistance = Config.Bind("Item Text Settings", "Clock viewable distance", 7f, "Sets the how close player needs to be to see clock time text");
            enableWoodFromContainers = Config.Bind("Other Settings", "Enable wood from breaking containers", true, "Allows you to get firewood by breaking containers with a knife.");
            enableCrateInvCountText = Config.Bind("Other Settings", "Enable crate total count text", true, "Enables the look text that shows the number of items in a crate.");
            crateInvCountTextKey = Config.Bind("Other Settings", "Crate inventory count text key", KeyCode.E, "Crate inventory count text will show when holding this key.");
            removeItemHints = Config.Bind("Other Settings", "Remove item hints", false, "Removes the hint text that appears when you look at a common items (e.g., knife, fishing hook).");
            enableFishMovement = Config.Bind("Other Settings", "Enable fish movement", true, "Enables fish movement when fish caught. Fish will move around instead of just sitting still.");

            Harmony.CreateAndPatchAll(Assembly.GetExecutingAssembly(), PLUGIN_GUID);
        }
    }    
}

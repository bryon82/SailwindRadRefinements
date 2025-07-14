using BepInEx.Configuration;
using UnityEngine;

namespace RadRefinements
{
    internal class Configs
    {
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
        internal static ConfigEntry<bool> enableFishTension;
        internal static ConfigEntry<bool> enableElixirColors;

        internal static void InitializeConfigs()
        {
            var config = RR_Plugin.Instance.Config;

            enableInventorySwap = config.Bind("Inventory Settings", "Enable inventory swap", true, "Allows you to swap the item you are holding with the item in your selected inventory slot.");
            enableQuickMap = config.Bind("Inventory Settings", "Enable quick map", true, "On quick map key press, causes your character to hold the leftmost map that is in your inventory slots or to put it back in the inventory slot it came from.");
            quickMapKey = config.Bind("Inventory Settings", "Quick map key", KeyCode.M, "Key that retrieves or stows your map when pressed");
            enableQuickSlots = config.Bind("Inventory Settings", "Enable quick slots", true, "Enables the ability to retrieve items from your inventory when a key is pressed.");
            quickSlot1Key = config.Bind("Inventory Settings", "Quick slot 1 key", KeyCode.Alpha1, "Key that retrieves from or stows to inventory slot 1 when pressed");
            quickSlot2Key = config.Bind("Inventory Settings", "Quick slot 2 key", KeyCode.Alpha2, "Key that retrieves from or stows to inventory slot 2 when pressed");
            quickSlot3Key = config.Bind("Inventory Settings", "Quick slot 3 key", KeyCode.Alpha3, "Key that retrieves from or stows to inventory slot 3 when pressed");
            quickSlot4Key = config.Bind("Inventory Settings", "Quick slot 4 key", KeyCode.Alpha4, "Key that retrieves from or stows to inventory slot 4 when pressed");
            quickSlot5Key = config.Bind("Inventory Settings", "Quick slot 5 key", KeyCode.Alpha5, "Key that retrieves from or stows to inventory slot 5 when pressed");
            enableCrateItemDescription = config.Bind("Inventory Settings", "Enable crate item description", true, "Allows you to see the text you would see when looking at the item out of the crate.");
            enableQuadrantText = config.Bind("Item Text Settings", "Enable quadrant reading text", true, "Enables the text that shows the quadrant reading value.");
            enableCompassReadingHeld = config.Bind("Item Text Settings", "Enable compass reading while held", false, "Enables the text that shows the compass reading while holding the compass.");
            enableCompassDegreesText = config.Bind("Item Text Settings", "Enable compass reading degrees text", true, "Enables the text that shows the compass reading value in degrees.");
            enableCompassCardinalText = config.Bind("Item Text Settings", "Enable compass reading cardinal text", true, "Enables the text that shows the compass reading value in cardinal directions.");
            compassCardinalPrecisionLevel = config.Bind("Item Text Settings", "Number of compass ordinal directions", 16, new ConfigDescription("Number of ordinal directions given in the compass reading.", new AcceptableValueList<int>(4, 8, 16, 32)));
            compassViewableDistance = config.Bind("Item Text Settings", "Compass viewable distance", 3f, "Sets the how close player needs to be to see compass reading text");
            enableClockGlobalText = config.Bind("Item Text Settings", "Enable clock global time text", true, "Enables the text that shows the global time on the clock.");
            enableClockLocalText = config.Bind("Item Text Settings", "Enable clock local time text", true, "Enables the text that shows the clock time on the clock.");
            clockViewableDistance = config.Bind("Item Text Settings", "Clock viewable distance", 7f, "Sets the how close player needs to be to see clock time text");
            enableWoodFromContainers = config.Bind("Other Settings", "Enable wood from breaking containers", true, "Allows you to get firewood by breaking containers with a knife.");
            enableCrateInvCountText = config.Bind("Other Settings", "Enable crate total count text", true, "Enables the look text that shows the number of items in a crate.");
            crateInvCountTextKey = config.Bind("Other Settings", "Crate inventory count text key", KeyCode.E, "Crate inventory count text will show when holding this key.");
            removeItemHints = config.Bind("Other Settings", "Remove item hints", false, "Removes the hint text that appears when you look at a common items (e.g., knife, fishing hook).");
            enableFishMovement = config.Bind("Other Settings", "Enable fish movement", true, "Enables fish movement when fish caught. Fish will move around instead of just sitting still.");
            enableFishTension = config.Bind("Other Settings", "Enable fish tension", true, "Enables fish tension based on fish speed. Faster fish will cause more tension on the rod when reeling in.");
            enableElixirColors = config.Bind("Other Settings", "Enable elixir colors", true, "Makes the bottles for Energy Elixir blue and Snake Oil brown.");
        }
    }
}

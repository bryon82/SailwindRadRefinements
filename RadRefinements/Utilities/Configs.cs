using BepInEx.Configuration;
using UnityEngine;

namespace RadRefinements
{
    internal class Configs
    {
        internal static ConfigEntry<bool> enableInventorySwap;
        internal static ConfigEntry<bool> enableQuickMap;
        internal static ConfigEntry<KeyCode> quickMapKey;
        internal static ConfigEntry<bool> enableCrateItemDescription;
        internal static ConfigEntry<bool> enableQuadrantText;
        internal static ConfigEntry<bool> enableCompassReadingHeld;
        internal static ConfigEntry<bool> enableCompassDegreesText;
        internal static ConfigEntry<bool> enableCompassCardinalText;
        internal static ConfigEntry<int> compassCardinalPrecisionLevel;
        internal static ConfigEntry<float> compassViewableDistance;
        internal static ConfigEntry<bool> enableSunCompassText;
        internal static ConfigEntry<bool> enableClockGlobalText;
        internal static ConfigEntry<bool> enableClockLocalText;
        internal static ConfigEntry<float> clockViewableDistance;
        internal static ConfigEntry<bool> enableWoodFromContainers;
        internal static ConfigEntry<bool> enableCrateInvCountText;
        internal static ConfigEntry<KeyCode> crateInvCountTextKey;
        internal static ConfigEntry<bool> removeItemHints;
        internal static ConfigEntry<bool> enableElixirColors;
        internal static ConfigEntry<bool> enableLogbookLastSection;
        internal static ConfigEntry<bool> enableSingleClickSmoking;

        internal static void InitializeConfigs()
        {
            var config = RR_Plugin.Instance.Config;

            enableInventorySwap = config.Bind(
                "Inventory Settings",
                "Enable inventory swap",
                true,
                "Allows you to swap the item you are holding with the item in your selected inventory slot.");
            enableQuickMap = config.Bind(
                "Inventory Settings",
                "Enable quick map",
                true,
                "On quick map key press, causes your character to hold the leftmost map that is in your inventory slots or to put it back in the inventory slot it came from.");
            quickMapKey = config.Bind(
                "Inventory Settings", 
                "Quick map key", 
                KeyCode.M, 
                "Key that retrieves or stows your map when pressed");
            enableCrateItemDescription = config.Bind(
                "Inventory Settings",
                "Enable item description in crates",
                true);
            enableQuadrantText = config.Bind(
                "Item Text Settings",
                "Enable quadrant reading text",
                true);
            enableCompassReadingHeld = config.Bind(
                "Item Text Settings",
                "Enable compass reading while held",
                false);
            enableCompassDegreesText = config.Bind(
                "Item Text Settings",
                "Enable compass reading degrees text",
                true);
            enableCompassCardinalText = config.Bind(
                "Item Text Settings",
                "Enable compass reading cardinal text",
                true);
            compassCardinalPrecisionLevel = config.Bind(
                "Item Text Settings",
                "Number of compass ordinal directions",
                16,
                new ConfigDescription(
                    "Number of ordinal directions given in the compass reading.",
                    new AcceptableValueList<int>(4, 8, 16, 32)));
            compassViewableDistance = config.Bind(
                "Item Text Settings",
                "Compass viewable distance",
                3f,
                "Sets the how close player needs to be to see compass reading text");
            enableSunCompassText = config.Bind(
                "Item Text Settings",
                "Enable sun compass reading text",
                true);
            enableClockGlobalText = config.Bind(
                "Item Text Settings",
                "Enable clock global time text",
                true);
            enableClockLocalText = config.Bind(
                "Item Text Settings",
                "Enable clock local time text",
                true);
            clockViewableDistance = config.Bind(
                "Item Text Settings",
                "Clock viewable distance",
                7f,
                "Sets the how close player needs to be to see clock time text");
            enableWoodFromContainers = config.Bind(
                "Other Settings",
                "Enable firewood from breaking containers",
                true,
                "Allows you to get firewood by breaking containers with a knife.");
            enableCrateInvCountText = config.Bind(
                "Other Settings",
                "Enable crate total count text",
                true,
                "Enables the look text that shows the number of items in a crate.");
            crateInvCountTextKey = config.Bind(
                "Other Settings",
                "Crate inventory count text key",
                KeyCode.E,
                "Crate inventory count text will show when holding this key.");
            removeItemHints = config.Bind(
                "Other Settings",
                "Remove item hints",
                false,
                "Removes the hint text that appears when you look at a common items (e.g., knife, fishing hook).");
            enableElixirColors = config.Bind(
                "Other Settings",
                "Enable elixir colors",
                true,
                "Makes the bottles for Energy Elixir blue and Snake Oil brown.");
            enableLogbookLastSection = config.Bind(
                "Other Settings",
                "Logbook remembers last section",
                true,
                "Makes the logbook remember the last section you had open so when you reopen it, it opens to that section.");
            enableSingleClickSmoking = config.Bind(
                "Other Settings",
                "Enable single click smoking",
                false,
                "Allows you to smoke a pipe with a single click instead of having to hold the button down.");
        }
    }
}

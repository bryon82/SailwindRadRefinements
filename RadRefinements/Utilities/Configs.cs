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

        internal static ConfigEntry<Color> readingTextColor;
        internal static ConfigEntry<bool> enableQuadrantText;
        internal static ConfigEntry<int> quadrantDecimalPlaces;
        internal static ConfigEntry<bool> enableCompassReadingHeld;
        internal static ConfigEntry<bool> enableCompassDegreesText;
        internal static ConfigEntry<bool> enableCompassCardinalText;
        internal static ConfigEntry<int> compassCardinalPrecision;
        internal static ConfigEntry<float> compassViewDist;
        internal static ConfigEntry<int> compassDecimalPlaces;
        internal static ConfigEntry<bool> enableSunCompassText;
        internal static ConfigEntry<int> sunCompassDecimalPlaces;
        internal static ConfigEntry<bool> enableClockGlobalText;
        internal static ConfigEntry<bool> enableClockLocalText;
        internal static ConfigEntry<float> clockViewDist;
        internal static ConfigEntry<bool> enableChipLogText;
        internal static ConfigEntry<float> chipLogViewDist;
        internal static ConfigEntry<int> chipLogDecimalPlaces;
        internal static ConfigEntry<bool> enableBarometerText;
        internal static ConfigEntry<float> barometerViewDist;
        internal static ConfigEntry<int> barometerDecimalPlaces;
        internal static ConfigEntry<bool> enableThermometerText;
        internal static ConfigEntry<float> thermometerViewDist;
        internal static ConfigEntry<int> thermometerDecimalPlaces;
        internal static ConfigEntry<bool> enableHygrometerText;
        internal static ConfigEntry<float> hygrometerViewDist;
        internal static ConfigEntry<int> hygrometerDecimalPlaces;
        internal static ConfigEntry<bool> enableInclinometerText;
        internal static ConfigEntry<float> inclinometerViewDist;
        internal static ConfigEntry<bool> negativeInclinometerAngles;
        internal static ConfigEntry<int> inclinometerDecimalPlaces;
        internal static ConfigEntry<bool> enableWindCompassDegreesText;
        internal static ConfigEntry<bool> enableWindCompassCardinalText;
        internal static ConfigEntry<int> windCompassCardinalPrecision;
        internal static ConfigEntry<int> windCompassDecimalPlaces;
        internal static ConfigEntry<bool> enableAnemometerText;
        internal static ConfigEntry<float> anemometerViewDist;
        internal static ConfigEntry<int> anemometerDecimalPlaces;
        internal static ConfigEntry<bool> enableWeathervaneDegreesText;
        internal static ConfigEntry<bool> enableWeathervanePointOfSailText;
        internal static ConfigEntry<float> weathervaneViewDist;
        internal static ConfigEntry<int> weathervaneDecimalPlaces;
        internal static ConfigEntry<bool> weatherVaneColorCoded;

        internal static ConfigEntry<bool> enableWoodFromContainers;
        internal static ConfigEntry<bool> enableCrateInvCountText;
        internal static ConfigEntry<KeyCode> crateInvCountTextKey;
        internal static ConfigEntry<bool> removeItemHints;
        internal static ConfigEntry<bool> enableElixirColors;
        internal static ConfigEntry<bool> enableLogbookLastSection;
        internal static ConfigEntry<bool> enableSingleClickSmoking;
        internal static ConfigEntry<string> barometerUnits;
        internal static ConfigEntry<string> thermometerUnits;

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

            readingTextColor = config.Bind(
                "Item Text Settings",
                "Reading text color",
                new Color(0.859f, 0.839f, 0.788f, 0.533f));
                

            enableQuadrantText = config.Bind(
                "Enable Item Texts",
                "Quadrant reading text",
                true);
            quadrantDecimalPlaces = config.Bind(
                "Item Text Settings",
                "Number of decimal places for quadrant",
                2,
                new ConfigDescription(
                    "Number of digits after the decimal.",
                    new AcceptableValueRange<int>(0, 3)));
            enableCompassReadingHeld = config.Bind(
                "Item Text Settings",
                "Compass reading text while held",
                false);
            enableCompassDegreesText = config.Bind(
                "Enable Item Texts",
                "Compass reading degrees text",
                true);
            enableCompassCardinalText = config.Bind(
                "Enable Item Texts",
                "Compass reading cardinal text",
                true);
            compassCardinalPrecision = config.Bind(
                "Item Text Settings",
                "Number of compass ordinal directions",
                16,
                new ConfigDescription(
                    "Number of ordinal directions given in the compass reading.",
                    new AcceptableValueList<int>(4, 8, 16, 32)));
            compassViewDist = config.Bind(
                "Item Text Settings",
                "Compass viewable distance",
                3f);
            compassDecimalPlaces = config.Bind(
                "Item Text Settings",
                "Number of decimal places for compass",
                0,
                new ConfigDescription(
                    "Number of digits after the decimal.",
                    new AcceptableValueRange<int>(0, 3)));
            enableSunCompassText = config.Bind(
                "Enable Item Texts",
                "Sun compass reading text",
                true);
            sunCompassDecimalPlaces = config.Bind(
                "Item Text Settings",
                "Number of decimal places for sun compass",
                1,
                new ConfigDescription(
                    "Number of digits after the decimal.",
                    new AcceptableValueRange<int>(0, 3)));
            enableClockGlobalText = config.Bind(
                "Enable Item Texts",
                "Clock global time text",
                true);
            enableClockLocalText = config.Bind(
                "Enable Item Texts",
                "Clock local time text",
                true);
            clockViewDist = config.Bind(
                "Item Text Settings",
                "Clock viewable distance",
                5f);
            enableChipLogText = config.Bind(
                "Enable Item Texts",
                "Chip log reading text",
                true);
            chipLogViewDist = config.Bind(
                "Item Text Settings",
                "Chip log viewable distance",
                5f);
            chipLogDecimalPlaces = config.Bind(
                "Item Text Settings",
                "Number of decimal places for chip log",
                1,
                new ConfigDescription(
                    "Number of digits after the decimal.",
                    new AcceptableValueRange<int>(0, 3)));
            enableBarometerText = config.Bind(
                "Enable Item Texts",
                "Barometer reading text",
                true);
            barometerViewDist = config.Bind(
                "Item Text Settings",
                "Barometer viewable distance",
                5f);
            barometerDecimalPlaces = config.Bind(
                "Item Text Settings",
                "Number of decimal places for barometer",
                1,
                new ConfigDescription(
                    "Number of digits after the decimal.",
                    new AcceptableValueRange<int>(0, 3)));
            enableThermometerText = config.Bind(
                "Enable Item Texts",
                "Thermometer reading text",
                true);
            thermometerViewDist = config.Bind(
                "Item Text Settings",
                "Thermometer viewable distance",
                5f);
            thermometerDecimalPlaces = config.Bind(
                "Item Text Settings",
                "Number of decimal places for thermometer",
                0,
                new ConfigDescription(
                    "Number of digits after the decimal.",
                    new AcceptableValueRange<int>(0, 3)));
            enableHygrometerText = config.Bind(
                "Enable Item Texts",
                "Hygrometer reading text",
                true);
            hygrometerViewDist = config.Bind(
                "Item Text Settings",
                "Hygrometer viewable distance",
                5f);
            hygrometerDecimalPlaces = config.Bind(
                "Item Text Settings",
                "Number of decimal places for hygrometer",
                0,
                new ConfigDescription(
                    "Number of digits after the decimal.",
                    new AcceptableValueRange<int>(0, 3)));
            enableInclinometerText = config.Bind(
                "Enable Item Texts",
                "Inclinometer reading text",
                true);
            inclinometerViewDist = config.Bind(
                "Item Text Settings",
                "Inclinometer viewable distance",
                5f);
            negativeInclinometerAngles = config.Bind(
                "Item Text Settings",
                "Inclinometer shows negative angles",
                false);
            inclinometerDecimalPlaces = config.Bind(
                "Item Text Settings",
                "Number of decimal places for inclinometer",
                0,
                new ConfigDescription(
                    "Number of digits after the decimal.",
                    new AcceptableValueRange<int>(0, 3)));
            enableWindCompassDegreesText = config.Bind(
                "Enable Item Texts",
                "Wind compass reading degrees text",
                true);
            enableWindCompassCardinalText = config.Bind(
                "Enable Item Texts",
                "Wind compass reading cardinal text",
                true);
            windCompassCardinalPrecision = config.Bind(
                "Item Text Settings",
                "Number of wind compass ordinal directions",
                16,
                new ConfigDescription(
                    "Number of ordinal directions given in the wind compass reading.",
                    new AcceptableValueList<int>(4, 8, 16, 32)));
            windCompassDecimalPlaces = config.Bind(
                "Item Text Settings",
                "Number of decimal places for wind compass",
                0,
                new ConfigDescription(
                    "Number of digits after the decimal.",
                    new AcceptableValueRange<int>(0, 3)));
            enableAnemometerText = config.Bind(
                "Enable Item Texts",
                "Anemometer reading text",
                true);
            anemometerViewDist = config.Bind(
                "Item Text Settings",
                "Anemometer viewable distance",
                5f);
            anemometerDecimalPlaces = config.Bind(
                "Item Text Settings",
                "Number of decimal places for anemometer",
                0,
                new ConfigDescription(
                    "Number of digits after the decimal.",
                    new AcceptableValueRange<int>(0, 3)));
            enableWeathervaneDegreesText = config.Bind(
                "Enable Item Texts",
                "Weathervane degrees reading text",
                true);
            enableWeathervanePointOfSailText = config.Bind(
                "Enable Item Texts",
                "Weathervane point of sail reading text",
                true);
            weathervaneViewDist = config.Bind(
                "Item Text Settings",
                "Weathervane viewable distance",
                5f);
            weathervaneDecimalPlaces = config.Bind(
                "Item Text Settings",
                "Number of decimal places for weathervane",
                0,
                new ConfigDescription(
                    "Number of digits after the decimal.",
                    new AcceptableValueRange<int>(0, 3)));
            weatherVaneColorCoded = config.Bind(
                "Item Text Settings",
                "Weathervane color coded",
                true,
                "Makes the weathervane text color coded based on the wind coming from port or starboard.");

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
            barometerUnits = config.Bind(
                "Item Text Settings",
                "Barometer units",
                "inHg",
                new ConfigDescription(
                    "The units used in the barometer readings.",
                    new AcceptableValueList<string>("inHg", "hPa", "mbar", "atm")));
            thermometerUnits = config.Bind(
                "Item Text Settings",
                "Thermometer units",
                "°F",
                new ConfigDescription(
                    "The units used in the thermometer readings.",
                    new AcceptableValueList<string>("°F", "°C", "K")));
        }
    }
}

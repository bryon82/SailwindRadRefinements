using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;
using HarmonyLib;
using System.Reflection;
using UnityEngine;

namespace RadRefinements
{
    [BepInPlugin(PLUGIN_GUID, PLUGIN_NAME, PLUGIN_VERSION)]
    public class Plugin : BaseUnityPlugin
    {
        public const string PLUGIN_GUID = "com.raddude82.radrefinements";
        public const string PLUGIN_NAME = "RadRefinements";
        public const string PLUGIN_VERSION = "1.0.0";

        internal static Plugin instance;
        internal static ManualLogSource logger;

        internal static ConfigEntry<bool> enableInventorySwap;
        internal static ConfigEntry<bool> enableQuickMap;
        internal static ConfigEntry<KeyCode> quickMapButton;
        internal static ConfigEntry<bool> enableQuadrantText;
        internal static ConfigEntry<bool> enableCompassText;
        internal static ConfigEntry<float> compassViewableDistance;
        internal static ConfigEntry<bool> enableClockGlobalText;
        internal static ConfigEntry<bool> enableClockLocalText;
        internal static ConfigEntry<float> clockViewableDistance;

        private void Awake()
        {
            instance = this;
            logger = Logger;

            enableInventorySwap = Config.Bind("Inventory Settings", "Enable inventory swap", true, "Allows you to swap the item you are holding with the item in your selected inventory slot.");
            enableQuickMap = Config.Bind("Inventory Settings", "Enable quick map", true, "On quick map button press, causes your character to hold the leftmost map that is in your inventory slots or to put it back in the inventory slot it came from.");
            quickMapButton = Config.Bind("Inventory Settings", "Quick map button", KeyCode.M, "Key that retrieves or stows your map");
            enableQuadrantText = Config.Bind("Item Text Settings", "Enable quadrant reading text", true, "Enables the text that shows the quadrant reading value.");
            enableCompassText = Config.Bind("Item Text Settings", "Enable compass reading text", true, "Enables the text that shows the compass reading value.");
            compassViewableDistance = Config.Bind("Item Text Settings", "Compass viewable distance", 3f, "Sets the how close player needs to be to see compass reading text");
            enableClockGlobalText = Config.Bind("Item Text Settings", "Enable clock global time text", true, "Enables the text that shows the global time on the clock.");
            enableClockLocalText = Config.Bind("Item Text Settings", "Enable clock local time text", true, "Enables the text that shows the clock time on the clock.");
            clockViewableDistance = Config.Bind("Item Text Settings", "Clock viewable distance", 7f, "Sets the how close player needs to be to see clock time text");

            Harmony.CreateAndPatchAll(Assembly.GetExecutingAssembly(), PLUGIN_GUID);
        }
    }    
}

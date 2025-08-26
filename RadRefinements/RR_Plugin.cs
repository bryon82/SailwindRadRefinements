﻿using BepInEx;
using BepInEx.Logging;
using HarmonyLib;
using System.Reflection;

namespace RadRefinements
{
    [BepInPlugin(PLUGIN_GUID, PLUGIN_NAME, PLUGIN_VERSION)]
    [BepInDependency(MODSAVEBACKUPS_GUID, MODSAVEBACKUPS_VERSION)]    
    public class RR_Plugin : BaseUnityPlugin
    {
        public const string PLUGIN_GUID = "com.raddude82.radrefinements";
        public const string PLUGIN_NAME = "RadRefinements";
        public const string PLUGIN_VERSION = "1.1.3";

        public const string MODSAVEBACKUPS_GUID = "com.raddude82.modsavebackups";
        public const string MODSAVEBACKUPS_VERSION = "1.1.1";

        internal static RR_Plugin Instance { get; private set; }
        private static ManualLogSource _logger;

        internal static void LogDebug(string message) => _logger.LogDebug(message);
        internal static void LogInfo(string message) => _logger.LogInfo(message);
        internal static void LogWarning(string message) => _logger.LogWarning(message);
        internal static void LogError(string message) => _logger.LogError(message);

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }
            Instance = this;
            _logger = Logger;

            Configs.InitializeConfigs();

            Harmony.CreateAndPatchAll(Assembly.GetExecutingAssembly(), PLUGIN_GUID);
        }
    }
}

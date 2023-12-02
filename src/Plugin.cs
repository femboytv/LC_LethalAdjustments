using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;
using HarmonyLib;

namespace LethalAdjustments
{
    [BepInPlugin(PluginInfo.PLUGIN_GUID, PluginInfo.PLUGIN_NAME, PluginInfo.PLUGIN_VERSION)]
    public class Plugin : BaseUnityPlugin
    {
        private void Awake()
        {
            Patches.Init(Logger);

            var harmony = new Harmony(PluginInfo.PLUGIN_GUID);
            harmony.PatchAll(typeof(Patches));

            Logger.LogInfo($"Plugin {PluginInfo.PLUGIN_GUID} is loaded!");
        }
    }

    internal class Config
    {
        private static ConfigFile MapSizeFile { get; set; }
        internal static ConfigEntry<float> MapSizeMultiplier { get; set; }

        private static ConfigFile ScrapAmountFile { get; set; }
        internal static ConfigEntry<float> ScrapAmountMultiplier { get; set; }

        private static ConfigFile ScrapValueFile { get; set; }
        internal static ConfigEntry<float> ScrapValueMultiplier { get; set; }

        static Config()
        {
            MapSizeFile = new ConfigFile(Paths.ConfigPath + "\\LethalAdjustments\\MapSize.cfg", true);
            MapSizeMultiplier = MapSizeFile.Bind("MapSize", "Multiplier", 1f, "Multiplier of the map's size");

            ScrapAmountFile = new ConfigFile(Paths.ConfigPath + "\\LethalAdjustments\\ScrapAmount.cfg", true);
            ScrapAmountMultiplier = ScrapAmountFile.Bind("ScrapAmount", "Multiplier", 1f, "Multiplier of the amount of scrap spawned");

            ScrapValueFile = new ConfigFile(Paths.ConfigPath + "\\LethalAdjustments\\ScrapValue.cfg", true);
            ScrapValueMultiplier = ScrapValueFile.Bind("ScrapValue", "Multiplier", 1f, "Multiplier of the value of scrap spawned");
        }
    }


    internal class Patches
    {
        private static ManualLogSource Logger { get; set; }

        public static void Init(ManualLogSource logger)
        {
            Logger = logger;
        }

        [HarmonyPatch(typeof(RoundManager), "Awake")]
        [HarmonyPostfix]
        private static void AwakePatch()
        {
            Logger.LogInfo($"RoundManager: mapSizeMultiplier = {Config.MapSizeMultiplier.Value}");
            RoundManager.Instance.mapSizeMultiplier = Config.MapSizeMultiplier.Value;
            Logger.LogInfo($"RoundManager: scrapAmountMultiplier = {Config.ScrapAmountMultiplier.Value}");
            RoundManager.Instance.scrapAmountMultiplier = Config.ScrapAmountMultiplier.Value;
            Logger.LogInfo($"RoundManager: scrapValueMultiplier = {Config.ScrapValueMultiplier.Value}");
            RoundManager.Instance.scrapValueMultiplier = Config.ScrapValueMultiplier.Value;
        }
    }
}
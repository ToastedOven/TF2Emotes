using BepInEx.Configuration;
using RiskOfOptions.OptionConfigs;
using RiskOfOptions.Options;
using RiskOfOptions;
using System;
using System.Collections.Generic;
using System.Text;

namespace TitanFall2Emotes
{
    public static class Settings
    {
        public static ConfigEntry<bool> EnemiesEmoteWithYou;
        public static ConfigEntry<bool> PartyCrashers;
        public static ConfigEntry<bool> RPSisAccurate;
        internal static void SetupConfig()
        {
            EnemiesEmoteWithYou = TF2Plugin.Instance.Config.Bind<bool>("Gameplay Altering Stuff", "Enemies Emote With You", true, "Enemies will be friendlies if you don't shoot back :)  (Mainly supports with TF2Emotes but kinda works with other stuff)");
            PartyCrashers = TF2Plugin.Instance.Config.Bind<bool>("Gameplay Altering Stuff", "Party Crashers", false, "You know exactly what it means");
            RPSisAccurate = TF2Plugin.Instance.Config.Bind<bool>("Gameplay Altering Stuff", "Lore Accurate Rock Paper Scissors", true, "bottom text");

        }
        internal static void SetupROO()
        {
            ModSettingsManager.AddOption(new CheckBoxOption(Settings.EnemiesEmoteWithYou, new CheckBoxConfig() { restartRequired = false }));
            ModSettingsManager.AddOption(new CheckBoxOption(Settings.PartyCrashers, new CheckBoxConfig() { restartRequired = false }));
            ModSettingsManager.AddOption(new CheckBoxOption(Settings.RPSisAccurate, new CheckBoxConfig() { restartRequired = false }));
        }
    }
}

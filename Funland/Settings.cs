using BepInEx.Configuration;
using RiskOfOptions.OptionConfigs;
using RiskOfOptions.Options;
using RiskOfOptions;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace TitanFall2Emotes
{
    public static class Settings
    {
        public static ConfigEntry<bool> RPSisAccurate;
        internal static void SetupConfig()
        {
            ModSettingsManager.SetModIcon(Assets.Load<Sprite>("@ExampleEmotePlugin_example_emotes:assets/icon.png"));
            RPSisAccurate = TF2Plugin.Instance.Config.Bind<bool>("Gameplay Altering Stuff", "Lore Accurate Rock Paper Scissors", true, "bottom text");

        }
        internal static void SetupROO()
        {
            ModSettingsManager.AddOption(new CheckBoxOption(Settings.RPSisAccurate, new CheckBoxConfig() { restartRequired = false }));
        }
    }
}

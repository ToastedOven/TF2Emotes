using BepInEx;
using BepInEx.Configuration;
using EmotesAPI;
using R2API;
using R2API.Networking;
using R2API.Utils;
using RiskOfOptions;
using RiskOfOptions.Options;
using RoR2;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Networking;

namespace ExamplePlugin
{
    [BepInDependency("com.weliveinasociety.CustomEmotesAPI")]
    [BepInPlugin(PluginGUID, PluginName, PluginVersion)]
    [NetworkCompatibility(CompatibilityLevel.EveryoneMustHaveMod, VersionStrictness.EveryoneNeedSameModVersion)]
    [R2APISubmoduleDependency("SoundAPI", "PrefabAPI", "CommandHelper", "ResourcesAPI")]
    public class TF2Plugin : BaseUnityPlugin
    {
        public const string PluginGUID = "com.weliveinasociety.teamfortress2emotes";
        public const string PluginAuthor = "Nunchuk";
        public const string PluginName = "TF2Emotes";
        public const string PluginVersion = "1.0.0";
        internal static Shader defaultShader = Addressables.LoadAssetAsync<GameObject>("RoR2/Base/Commando/CommandoBody.prefab").WaitForCompletion().GetComponentInChildren<SkinnedMeshRenderer>().material.shader;
        public void Awake()
        {
            Assets.PopulateAssets();
            //Assets.AddSoundBank("caramelldeeznuts.bnk");
            Assets.LoadSoundBanks();
        }

        void Update()
        {
            
        }
    }
}

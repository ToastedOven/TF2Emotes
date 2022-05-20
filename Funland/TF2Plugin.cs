using BepInEx;
using BepInEx.Configuration;
using EmotesAPI;
using R2API;
using R2API.Networking;
using R2API.Networking.Interfaces;
using R2API.Utils;
using RiskOfOptions;
using RiskOfOptions.Options;
using RoR2;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Networking;

namespace TitanFall2Emotes
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
        internal static List<string> Conga_Emotes = new List<string>();
        internal static List<string> KazotskyKick_Emotes = new List<string>();
        internal static List<string> RPS_Start_Emotes = new List<string>();
        internal static List<string> RPS_Loss_Emotes = new List<string>();
        internal static List<string> RPS_Win_Emotes = new List<string>();
        internal static List<string> Flip_Wait_Emotes = new List<string>();
        internal static List<string> Flip_Flip_Emotes = new List<string>();
        internal static List<string> Flip_Throw_Emotes = new List<string>();
        internal static Shader defaultShader = Addressables.LoadAssetAsync<GameObject>("RoR2/Base/Commando/CommandoBody.prefab").WaitForCompletion().GetComponentInChildren<SkinnedMeshRenderer>().material.shader;
        public void Awake()
        {
            Assets.PopulateAssets();
            Assets.AddSoundBank("Init.bnk");
            Assets.AddSoundBank("tf2.bnk");
            Assets.LoadSoundBanks();
            Rancho();
            RPS();
            Conga();
            Flip();
            KazotskyKick();
            Register();
            DEBUG();
            CustomEmotesAPI.animChanged += CustomEmotesAPI_animChanged;
            CustomEmotesAPI.emoteSpotJoined_Body += CustomEmotesAPI_emoteSpotJoined_Body;
        }
        void Register()
        {
            NetworkingAPI.RegisterMessageType<SyncRandomEmoteToClient>();
            NetworkingAPI.RegisterMessageType<SyncRandomEmoteToHost>();
        }
        public void Rancho()
        {
            AddAnimation("Engi/Rancho/RanchoRelaxo", "Rancho", "Engi/Rancho/engiRanchoPassive", false, false);
            AddAnimation("Engi/Rancho/engiRanchoBurp", "", "Engi/Rancho/engiRanchoPassive", false, false, false);
            AddAnimation("Engi/Rancho/engiRanchoBigDrink", "", "Engi/Rancho/engiRanchoPassive", false, false, false);
            AddAnimation("Engi/Rancho/engiRanchoQuickDrink", "", "Engi/Rancho/engiRanchoPassive", false, false, false);
        }
        public void Flip()
        {
            CustomEmotesAPI.AddNonAnimatingEmote("Flippin' Awesome");

            string emote = AddHiddenAnimation(new string[] { "Demo/Flip/Demo_Flip_Start" }, new string[] { "Demo/Flip/Demo_Flip_Wait" }, new string[] { "Demo_Flip_Waiting" }, "Flip", new JoinSpot[] { new JoinSpot("FlipJoinSpot", new Vector3(0, 0, 1.5f)) });
            Flip_Wait_Emotes.Add(emote);

            emote = AddHiddenAnimation(new string[] { "Demo/Flip/Demo_Flip_Throw" }, new string[] { "Demo_Flip_Throw" }, "");
            Flip_Throw_Emotes.Add(emote);
            emote = AddHiddenAnimation(new string[] { "Demo/Flip/Demo_Flip_Flip" }, new string[] { "Demo_Flip_Flip" }, "");
            Flip_Flip_Emotes.Add(emote);


            emote = AddHiddenAnimation(new string[] { "Engi/Flip/Engi_Flip_Start" }, new string[] { "Engi/Flip/Engi_Flip_Wait" }, new string[] { "Engi_Flip_Waiting" }, "Flip", new JoinSpot[] { new JoinSpot("FlipJoinSpot", new Vector3(0, 0, 1.5f)) });
            Flip_Wait_Emotes.Add(emote);

            emote = AddHiddenAnimation(new string[] { "Engi/Flip/Engi_Flip_Throw" }, new string[] { "Engi_Flip_Throw" }, "");
            Flip_Throw_Emotes.Add(emote);
            emote = AddHiddenAnimation(new string[] { "Engi/Flip/Engi_Flip_Flip" }, new string[] { "Engi_Flip_Flip" }, "");
            Flip_Flip_Emotes.Add(emote);


            emote = AddHiddenAnimation(new string[] { "Medic/Flip/Medic_Flip_Start" }, new string[] { "Medic/Flip/Medic_Flip_Wait" }, new string[] { "Medic_Flip_Waiting" }, "Flip", new JoinSpot[] { new JoinSpot("FlipJoinSpot", new Vector3(0, 0, 1.5f)) });
            Flip_Wait_Emotes.Add(emote);

            emote = AddHiddenAnimation(new string[] { "Medic/Flip/Medic_Flip_Throw" }, new string[] { "Medic_Flip_Throw" }, "");
            Flip_Throw_Emotes.Add(emote);
            emote = AddHiddenAnimation(new string[] { "Medic/Flip/Medic_Flip_Flip" }, new string[] { "Medic_Flip_Flip" }, "");
            Flip_Flip_Emotes.Add(emote);


            emote = AddHiddenAnimation(new string[] { "Heavy/Flip/Heavy_Flip_Start" }, new string[] { "Heavy/Flip/Heavy_Flip_Wait" }, new string[] { "Heavy_Flip_Waiting" }, "Flip", new JoinSpot[] { new JoinSpot("FlipJoinSpot", new Vector3(0, 0, 1.5f)) });
            Flip_Wait_Emotes.Add(emote);

            emote = AddHiddenAnimation(new string[] { "Heavy/Flip/Heavy_Flip_Throw" }, new string[] { "Heavy_Flip_Throw" }, "");
            Flip_Throw_Emotes.Add(emote);
            emote = AddHiddenAnimation(new string[] { "Heavy/Flip/Heavy_Flip_Flip" }, new string[] { "Heavy_Flip_Flip" }, "");
            Flip_Flip_Emotes.Add(emote);


            emote = AddHiddenAnimation(new string[] { "Pyro/Flip/Pyro_Flip_Start" }, new string[] { "Pyro/Flip/Pyro_Flip_Wait" }, new string[] { "Pyro_Flip_Waiting" }, "Flip", new JoinSpot[] { new JoinSpot("FlipJoinSpot", new Vector3(0, 0, 1.5f)) });
            Flip_Wait_Emotes.Add(emote);

            emote = AddHiddenAnimation(new string[] { "Pyro/Flip/Pyro_Flip_Throw" }, new string[] { "Pyro_Flip_Throw" }, "");
            Flip_Throw_Emotes.Add(emote);
            emote = AddHiddenAnimation(new string[] { "Pyro/Flip/Pyro_Flip_Flip" }, new string[] { "Pyro_Flip_Flip" }, "");
            Flip_Flip_Emotes.Add(emote);


            emote = AddHiddenAnimation(new string[] { "Scout/Flip/Scout_Flip_Start" }, new string[] { "Scout/Flip/Scout_Flip_Wait" }, new string[] { "Scout_Flip_Waiting" }, "Flip", new JoinSpot[] { new JoinSpot("FlipJoinSpot", new Vector3(0, 0, 1.5f)) });
            Flip_Wait_Emotes.Add(emote);

            emote = AddHiddenAnimation(new string[] { "Scout/Flip/Scout_Flip_Throw" }, new string[] { "Scout_Flip_Throw" }, "");
            Flip_Throw_Emotes.Add(emote);
            emote = AddHiddenAnimation(new string[] { "Scout/Flip/Scout_Flip_Flip" }, new string[] { "Scout_Flip_Flip" }, "");
            Flip_Flip_Emotes.Add(emote);


            emote = AddHiddenAnimation(new string[] { "Sniper/Flip/Sniper_Flip_Start" }, new string[] { "Sniper/Flip/Sniper_Flip_Wait" }, new string[] { "Sniper_Flip_Waiting" }, "Flip", new JoinSpot[] { new JoinSpot("FlipJoinSpot", new Vector3(0, 0, 1.5f)) });
            Flip_Wait_Emotes.Add(emote);

            emote = AddHiddenAnimation(new string[] { "Sniper/Flip/Sniper_Flip_Throw" }, new string[] { "Sniper_Flip_Throw" }, "");
            Flip_Throw_Emotes.Add(emote);
            emote = AddHiddenAnimation(new string[] { "Sniper/Flip/Sniper_Flip_Flip" }, new string[] { "Sniper_Flip_Flip" }, "");
            Flip_Flip_Emotes.Add(emote);


            emote = AddHiddenAnimation(new string[] { "Soldier/Flip/Soldier_Flip_Start" }, new string[] { "Soldier/Flip/Soldier_Flip_Wait" }, new string[] { "Soldier_Flip_Waiting" }, "Flip", new JoinSpot[] { new JoinSpot("FlipJoinSpot", new Vector3(0, 0, 1.5f)) });
            Flip_Wait_Emotes.Add(emote);

            emote = AddHiddenAnimation(new string[] { "Soldier/Flip/Soldier_Flip_Throw" }, new string[] { "Soldier_Flip_Throw" }, "");
            Flip_Throw_Emotes.Add(emote);
            emote = AddHiddenAnimation(new string[] { "Soldier/Flip/Soldier_Flip_Flip" }, new string[] { "Soldier_Flip_Flip" }, "");
            Flip_Flip_Emotes.Add(emote);


            emote = AddHiddenAnimation(new string[] { "Spy/Flip/Spy_Flip_Start" }, new string[] { "Spy/Flip/Spy_Flip_Wait" }, new string[] { "Spy_Flip_Waiting" }, "Flip", new JoinSpot[] { new JoinSpot("FlipJoinSpot", new Vector3(0, 0, 1.5f)) });
            Flip_Wait_Emotes.Add(emote);

            emote = AddHiddenAnimation(new string[] { "Spy/Flip/Spy_Flip_Throw" }, new string[] { "Spy_Flip_Throw" }, "");
            Flip_Throw_Emotes.Add(emote);
            emote = AddHiddenAnimation(new string[] { "Spy/Flip/Spy_Flip_Flip" }, new string[] { "Spy_Flip_Flip" }, "");
            Flip_Flip_Emotes.Add(emote);
        }
        public void RPS()
        {
            CustomEmotesAPI.AddNonAnimatingEmote("Rock Paper Scissors");
            CustomEmotesAPI.AddNonAnimatingEmote("Rock", false);
            CustomEmotesAPI.AddNonAnimatingEmote("Paper", false);
            CustomEmotesAPI.AddNonAnimatingEmote("Scissors", false);
            string emote = AddHiddenAnimation(new string[] { "Engi/RPS/EngiRPSStart" }, new string[] { "Engi/RPS/EngiRPSLoop" }, new string[] { "RPS_Engi_Initiate" }, "RPS", new JoinSpot[] { new JoinSpot("RPSJoinSpot", new Vector3(0, 0, 1.5f)) });
            RPS_Start_Emotes.Add(emote);

            emote = AddHiddenAnimation(new string[] { "Engi/RPS/EngiRPS_RWin" }, new string[] { "RPS_Engi_WinRock" }, "");
            RPS_Win_Emotes.Add(emote);
            emote = AddHiddenAnimation(new string[] { "Engi/RPS/EngiRPS_RLose" }, new string[] { "RPS_Engi_Loss" }, "");
            RPS_Loss_Emotes.Add(emote);

            emote = AddHiddenAnimation(new string[] { "Engi/RPS/EngiRPS_PWin" }, new string[] { "RPS_Engi_WinPaper" }, "");
            RPS_Win_Emotes.Add(emote);
            emote = AddHiddenAnimation(new string[] { "Engi/RPS/EngiRPS_PLose" }, new string[] { "RPS_Engi_Loss" }, "");
            RPS_Loss_Emotes.Add(emote);

            emote = AddHiddenAnimation(new string[] { "Engi/RPS/EngiRPS_SWin" }, new string[] { "RPS_Engi_WinScissors" }, "");
            RPS_Win_Emotes.Add(emote);
            emote = AddHiddenAnimation(new string[] { "Engi/RPS/EngiRPS_SLose" }, new string[] { "RPS_Engi_Loss" }, "");
            RPS_Loss_Emotes.Add(emote);


            emote = AddHiddenAnimation(new string[] { "Demo/RPS/DemoRPS_Start" }, new string[] { "Demo/RPS/DemoRPS_Loop" }, new string[] { "RPS_Demo_Initiate" }, "RPS", new JoinSpot[] { new JoinSpot("RPSJoinSpot", new Vector3(0, 0, 1.5f)) });
            RPS_Start_Emotes.Add(emote);

            emote = AddHiddenAnimation(new string[] { "Demo/RPS/DemoRPS_RWin" }, new string[] { "RPS_Demo_WinRock" }, "");
            RPS_Win_Emotes.Add(emote);
            emote = AddHiddenAnimation(new string[] { "Demo/RPS/DemoRPS_RLose" }, new string[] { "RPS_Demo_Loss" }, "");
            RPS_Loss_Emotes.Add(emote);

            emote = AddHiddenAnimation(new string[] { "Demo/RPS/DemoRPS_PWin" }, new string[] { "RPS_Demo_WinPaper" }, "");
            RPS_Win_Emotes.Add(emote);
            emote = AddHiddenAnimation(new string[] { "Demo/RPS/DemoRPS_PLose" }, new string[] { "RPS_Demo_Loss" }, "");
            RPS_Loss_Emotes.Add(emote);

            emote = AddHiddenAnimation(new string[] { "Demo/RPS/DemoRPS_SWin" }, new string[] { "RPS_Demo_WinScissors" }, "");
            RPS_Win_Emotes.Add(emote);
            emote = AddHiddenAnimation(new string[] { "Demo/RPS/DemoRPS_SLose" }, new string[] { "RPS_Demo_Loss" }, "");
            RPS_Loss_Emotes.Add(emote);


            emote = AddHiddenAnimation(new string[] { "Soldier/RPS/SoldierRPS_Start" }, new string[] { "Soldier/RPS/SoldierRPS_Loop" }, new string[] { "RPS_Soldier_Initiate" }, "RPS", new JoinSpot[] { new JoinSpot("RPSJoinSpot", new Vector3(0, 0, 1.5f)) });
            RPS_Start_Emotes.Add(emote);

            emote = AddHiddenAnimation(new string[] { "Soldier/RPS/SoldierRPS_RWin" }, new string[] { "RPS_Soldier_WinRock" }, "");
            RPS_Win_Emotes.Add(emote);
            emote = AddHiddenAnimation(new string[] { "Soldier/RPS/SoldierRPS_RLose" }, new string[] { "RPS_Soldier_Loss" }, "");
            RPS_Loss_Emotes.Add(emote);

            emote = AddHiddenAnimation(new string[] { "Soldier/RPS/SoldierRPS_PWin" }, new string[] { "RPS_Soldier_WinPaper" }, "");
            RPS_Win_Emotes.Add(emote);
            emote = AddHiddenAnimation(new string[] { "Soldier/RPS/SoldierRPS_PLose" }, new string[] { "RPS_Soldier_Loss" }, "");
            RPS_Loss_Emotes.Add(emote);

            emote = AddHiddenAnimation(new string[] { "Soldier/RPS/SoldierRPS_SWin" }, new string[] { "RPS_Soldier_WinScissors" }, "");
            RPS_Win_Emotes.Add(emote);
            emote = AddHiddenAnimation(new string[] { "Soldier/RPS/SoldierRPS_SLose" }, new string[] { "RPS_Soldier_Loss" }, "");
            RPS_Loss_Emotes.Add(emote);


            emote = AddHiddenAnimation(new string[] { "Heavy/RPS/HeavyRPS_Start" }, new string[] { "Heavy/RPS/HeavyRPS_Loop" }, new string[] { "RPS_Heavy_Initiate" }, "RPS", new JoinSpot[] { new JoinSpot("RPSJoinSpot", new Vector3(0, 0, 1.5f)) });
            RPS_Start_Emotes.Add(emote);

            emote = AddHiddenAnimation(new string[] { "Heavy/RPS/HeavyRPS_RWin" }, new string[] { "RPS_Heavy_WinRock" }, "");
            RPS_Win_Emotes.Add(emote);
            emote = AddHiddenAnimation(new string[] { "Heavy/RPS/HeavyRPS_RLose" }, new string[] { "RPS_Heavy_Loss" }, "");
            RPS_Loss_Emotes.Add(emote);

            emote = AddHiddenAnimation(new string[] { "Heavy/RPS/HeavyRPS_PWin" }, new string[] { "RPS_Heavy_WinPaper" }, "");
            RPS_Win_Emotes.Add(emote);
            emote = AddHiddenAnimation(new string[] { "Heavy/RPS/HeavyRPS_PLose" }, new string[] { "RPS_Heavy_Loss" }, "");
            RPS_Loss_Emotes.Add(emote);

            emote = AddHiddenAnimation(new string[] { "Heavy/RPS/HeavyRPS_SWin" }, new string[] { "RPS_Heavy_WinScissors" }, "");
            RPS_Win_Emotes.Add(emote);
            emote = AddHiddenAnimation(new string[] { "Heavy/RPS/HeavyRPS_SLose" }, new string[] { "RPS_Heavy_Loss" }, "");
            RPS_Loss_Emotes.Add(emote);


            emote = AddHiddenAnimation(new string[] { "Medic/RPS/MedicRPS_Start" }, new string[] { "Medic/RPS/MedicRPS_Loop" }, new string[] { "RPS_Medic_Initiate" }, "RPS", new JoinSpot[] { new JoinSpot("RPSJoinSpot", new Vector3(0, 0, 1.5f)) });
            RPS_Start_Emotes.Add(emote);

            emote = AddHiddenAnimation(new string[] { "Medic/RPS/MedicRPS_RWin" }, new string[] { "RPS_Medic_WinRock" }, "");
            RPS_Win_Emotes.Add(emote);
            emote = AddHiddenAnimation(new string[] { "Medic/RPS/MedicRPS_RLose" }, new string[] { "RPS_Medic_Loss" }, "");
            RPS_Loss_Emotes.Add(emote);

            emote = AddHiddenAnimation(new string[] { "Medic/RPS/MedicRPS_PWin" }, new string[] { "RPS_Medic_WinPaper" }, "");
            RPS_Win_Emotes.Add(emote);
            emote = AddHiddenAnimation(new string[] { "Medic/RPS/MedicRPS_PLose" }, new string[] { "RPS_Medic_Loss" }, "");
            RPS_Loss_Emotes.Add(emote);

            emote = AddHiddenAnimation(new string[] { "Medic/RPS/MedicRPS_SWin" }, new string[] { "RPS_Medic_WinScissors" }, "");
            RPS_Win_Emotes.Add(emote);
            emote = AddHiddenAnimation(new string[] { "Medic/RPS/MedicRPS_SLose" }, new string[] { "RPS_Medic_Loss" }, "");
            RPS_Loss_Emotes.Add(emote);


            emote = AddHiddenAnimation(new string[] { "Pyro/RPS/PyroRPS_Start" }, new string[] { "Pyro/RPS/PyroRPS_Loop" }, new string[] { "RPS_Pyro_Initiate" }, "RPS", new JoinSpot[] { new JoinSpot("RPSJoinSpot", new Vector3(0, 0, 1.5f)) });
            RPS_Start_Emotes.Add(emote);

            emote = AddHiddenAnimation(new string[] { "Pyro/RPS/PyroRPS_RWin" }, new string[] { "RPS_Pyro_WinRock" }, "");
            RPS_Win_Emotes.Add(emote);
            emote = AddHiddenAnimation(new string[] { "Pyro/RPS/PyroRPS_RLose" }, new string[] { "RPS_Pyro_Loss" }, "");
            RPS_Loss_Emotes.Add(emote);

            emote = AddHiddenAnimation(new string[] { "Pyro/RPS/PyroRPS_PWin" }, new string[] { "RPS_Pyro_WinPaper" }, "");
            RPS_Win_Emotes.Add(emote);
            emote = AddHiddenAnimation(new string[] { "Pyro/RPS/PyroRPS_PLose" }, new string[] { "RPS_Pyro_Loss" }, "");
            RPS_Loss_Emotes.Add(emote);

            emote = AddHiddenAnimation(new string[] { "Pyro/RPS/PyroRPS_SWin" }, new string[] { "RPS_Pyro_WinScissors" }, "");
            RPS_Win_Emotes.Add(emote);
            emote = AddHiddenAnimation(new string[] { "Pyro/RPS/PyroRPS_SLose" }, new string[] { "RPS_Pyro_Loss" }, "");
            RPS_Loss_Emotes.Add(emote);


            emote = AddHiddenAnimation(new string[] { "Scout/RPS/ScoutRPS_Start" }, new string[] { "Scout/RPS/ScoutRPS_Loop" }, new string[] { "RPS_Scout_Initiate" }, "RPS", new JoinSpot[] { new JoinSpot("RPSJoinSpot", new Vector3(0, 0, 1.5f)) });
            RPS_Start_Emotes.Add(emote);

            emote = AddHiddenAnimation(new string[] { "Scout/RPS/ScoutRPS_RWin" }, new string[] { "RPS_Scout_WinRock" }, "");
            RPS_Win_Emotes.Add(emote);
            emote = AddHiddenAnimation(new string[] { "Scout/RPS/ScoutRPS_RLose" }, new string[] { "RPS_Scout_LossRock" }, "");
            RPS_Loss_Emotes.Add(emote);

            emote = AddHiddenAnimation(new string[] { "Scout/RPS/ScoutRPS_PWin" }, new string[] { "RPS_Scout_WinPaper" }, "");
            RPS_Win_Emotes.Add(emote);
            emote = AddHiddenAnimation(new string[] { "Scout/RPS/ScoutRPS_PLose" }, new string[] { "RPS_Scout_Loss" }, "");
            RPS_Loss_Emotes.Add(emote);

            emote = AddHiddenAnimation(new string[] { "Scout/RPS/ScoutRPS_SWin" }, new string[] { "RPS_Scout_WinScissors" }, "");
            RPS_Win_Emotes.Add(emote);
            emote = AddHiddenAnimation(new string[] { "Scout/RPS/ScoutRPS_SLose" }, new string[] { "RPS_Scout_Loss" }, "");
            RPS_Loss_Emotes.Add(emote);


            emote = AddHiddenAnimation(new string[] { "Sniper/RPS/SniperRPS_Start" }, new string[] { "Sniper/RPS/SniperRPS_Loop" }, new string[] { "RPS_Sniper_Initiate" }, "RPS", new JoinSpot[] { new JoinSpot("RPSJoinSpot", new Vector3(0, 0, 1.5f)) });
            RPS_Start_Emotes.Add(emote);

            emote = AddHiddenAnimation(new string[] { "Sniper/RPS/SniperRPS_RWin" }, new string[] { "RPS_Sniper_WinRock" }, "");
            RPS_Win_Emotes.Add(emote);
            emote = AddHiddenAnimation(new string[] { "Sniper/RPS/SniperRPS_RLose" }, new string[] { "RPS_Sniper_Loss" }, "");
            RPS_Loss_Emotes.Add(emote);

            emote = AddHiddenAnimation(new string[] { "Sniper/RPS/SniperRPS_PWin" }, new string[] { "RPS_Sniper_WinPaper" }, "");
            RPS_Win_Emotes.Add(emote);
            emote = AddHiddenAnimation(new string[] { "Sniper/RPS/SniperRPS_PLose" }, new string[] { "RPS_Sniper_Loss" }, "");
            RPS_Loss_Emotes.Add(emote);

            emote = AddHiddenAnimation(new string[] { "Sniper/RPS/SniperRPS_SWin" }, new string[] { "RPS_Sniper_WinScissors" }, "");
            RPS_Win_Emotes.Add(emote);
            emote = AddHiddenAnimation(new string[] { "Sniper/RPS/SniperRPS_SLose" }, new string[] { "RPS_Sniper_Loss" }, "");
            RPS_Loss_Emotes.Add(emote);


            emote = AddHiddenAnimation(new string[] { "Spy/RPS/SpyRPS_Start" }, new string[] { "Spy/RPS/SpyRPS_Loop" }, new string[] { "RPS_Spy_Initiate" }, "RPS", new JoinSpot[] { new JoinSpot("RPSJoinSpot", new Vector3(0, 0, 1.5f)) });
            RPS_Start_Emotes.Add(emote);

            emote = AddHiddenAnimation(new string[] { "Spy/RPS/SpyRPS_RWin" }, new string[] { "RPS_Spy_WinRock" }, "");
            RPS_Win_Emotes.Add(emote);
            emote = AddHiddenAnimation(new string[] { "Spy/RPS/SpyRPS_RLose" }, new string[] { "RPS_Spy_LossRock" }, "");
            RPS_Loss_Emotes.Add(emote);

            emote = AddHiddenAnimation(new string[] { "Spy/RPS/SpyRPS_PWin" }, new string[] { "RPS_Spy_WinPaper" }, "");
            RPS_Win_Emotes.Add(emote);
            emote = AddHiddenAnimation(new string[] { "Spy/RPS/SpyRPS_PLose" }, new string[] { "RPS_Spy_LossPaper" }, "");
            RPS_Loss_Emotes.Add(emote);

            emote = AddHiddenAnimation(new string[] { "Spy/RPS/SpyRPS_SWin" }, new string[] { "RPS_Spy_WinScissors" }, "");
            RPS_Win_Emotes.Add(emote);
            emote = AddHiddenAnimation(new string[] { "Spy/RPS/SpyRPS_SLose" }, new string[] { "RPS_Spy_LossScissors" }, "");
            RPS_Loss_Emotes.Add(emote);
        }
        public void KazotskyKick()
        {
            CustomEmotesAPI.AddNonAnimatingEmote("Kazotsky Kick");
            string emote;
            emote = AddHiddenAnimation(new string[] { "KazotskyKick/Kazotsky_Demo_Start" }, new string[] { "Kazotsky" }, "Kazotsky", true, new string[] { "KazotskyKick/Kazotsky_Demo_Loop" }); //names are wrong, should be Kazotsky_Sniper_Loop
            KazotskyKick_Emotes.Add(emote);
            int syncpos = BoneMapper.animClips[emote].syncPos;
            emote = AddHiddenAnimation(new string[] { "KazotskyKick/Kazotsky_Engi_Start" }, new string[] { "Kazotsky" }, "Kazotsky", true, new string[] { "KazotskyKick/Kazotsky_Engi_Loop" });
            KazotskyKick_Emotes.Add(emote);
            BoneMapper.animClips[emote].syncPos = syncpos;
            emote = AddHiddenAnimation(new string[] { "KazotskyKick/Kazotsky_Heavy_Start" }, new string[] { "Kazotsky" }, "Kazotsky", true, new string[] { "KazotskyKick/Kazotsky_Heavy_Loop" });
            KazotskyKick_Emotes.Add(emote);
            BoneMapper.animClips[emote].syncPos = syncpos;
            emote = AddHiddenAnimation(new string[] { "KazotskyKick/Kazotsky_Medic_Start" }, new string[] { "Kazotsky" }, "Kazotsky", true, new string[] { "KazotskyKick/Kazotsky_Medic_Loop" });
            KazotskyKick_Emotes.Add(emote);
            BoneMapper.animClips[emote].syncPos = syncpos;
            emote = AddHiddenAnimation(new string[] { "KazotskyKick/Kazotsky_Pyro_Start" }, new string[] { "Kazotsky" }, "Kazotsky", true, new string[] { "KazotskyKick/Kazotsky_Pyro_Loop" });
            KazotskyKick_Emotes.Add(emote);
            BoneMapper.animClips[emote].syncPos = syncpos;
            emote = AddHiddenAnimation(new string[] { "KazotskyKick/Kazotsky_Scout_Start" }, new string[] { "Kazotsky" }, "Kazotsky", true, new string[] { "KazotskyKick/Kazotsky_Scout_Loop" });
            KazotskyKick_Emotes.Add(emote);
            BoneMapper.animClips[emote].syncPos = syncpos;
            emote = AddHiddenAnimation(new string[] { "KazotskyKick/Kazotsky_Sniper_Start" }, new string[] { "Kazotsky" }, "Kazotsky", true, new string[] { "KazotskyKick/Kazotsky_Sniper_Loop" });
            KazotskyKick_Emotes.Add(emote);
            BoneMapper.animClips[emote].syncPos = syncpos;
            emote = AddHiddenAnimation(new string[] { "KazotskyKick/Kazotsky_Soldier_Start" }, new string[] { "Kazotsky" }, "Kazotsky", true, new string[] { "KazotskyKick/Kazotsky_Soldier_Loop" });
            KazotskyKick_Emotes.Add(emote);
            BoneMapper.animClips[emote].syncPos = syncpos;
            emote = AddHiddenAnimation(new string[] { "KazotskyKick/Kazotsky_Spy_Start" }, new string[] { "Kazotsky" }, "Kazotsky", true, new string[] { "KazotskyKick/Kazotsky_Spy_Loop" });
            KazotskyKick_Emotes.Add(emote);
            BoneMapper.animClips[emote].syncPos = syncpos;
        }
        public void Conga()
        {
            CustomEmotesAPI.AddNonAnimatingEmote("Conga");
            string emote;
            emote = AddHiddenAnimation(new string[] { "Conga/Demo_Conga" }, new string[] { "Conga" }, "Conga", true);
            Conga_Emotes.Add(emote);
            int syncpos = BoneMapper.animClips[emote].syncPos;
            emote = AddHiddenAnimation(new string[] { "Conga/Engi_Conga" }, new string[] { "Conga" }, "Conga", true);
            Conga_Emotes.Add(emote);
            BoneMapper.animClips[emote].syncPos = syncpos;
            emote = AddHiddenAnimation(new string[] { "Conga/Heavy_Conga" }, new string[] { "Conga" }, "Conga", true);
            Conga_Emotes.Add(emote);
            BoneMapper.animClips[emote].syncPos = syncpos;
            emote = AddHiddenAnimation(new string[] { "Conga/Medic_Conga" }, new string[] { "Conga" }, "Conga", true);
            Conga_Emotes.Add(emote);
            BoneMapper.animClips[emote].syncPos = syncpos;
            emote = AddHiddenAnimation(new string[] { "Conga/Pyro_Conga" }, new string[] { "Conga" }, "Conga", true);
            Conga_Emotes.Add(emote);
            BoneMapper.animClips[emote].syncPos = syncpos;
            emote = AddHiddenAnimation(new string[] { "Conga/Scout_Conga" }, new string[] { "Conga" }, "Conga", true);
            Conga_Emotes.Add(emote);
            BoneMapper.animClips[emote].syncPos = syncpos;
            emote = AddHiddenAnimation(new string[] { "Conga/Sniper_Conga" }, new string[] { "Conga" }, "Conga", true);
            Conga_Emotes.Add(emote);
            BoneMapper.animClips[emote].syncPos = syncpos;
            emote = AddHiddenAnimation(new string[] { "Conga/Soldier_Conga" }, new string[] { "Conga" }, "Conga", true);
            Conga_Emotes.Add(emote);
            BoneMapper.animClips[emote].syncPos = syncpos;
            emote = AddHiddenAnimation(new string[] { "Conga/Spy_Conga" }, new string[] { "Conga" }, "Conga", true);
            Conga_Emotes.Add(emote);
            BoneMapper.animClips[emote].syncPos = syncpos;
        }

        private void CustomEmotesAPI_emoteSpotJoined_Body(GameObject emoteSpot, BoneMapper joiner, BoneMapper host)
        {
            string emoteSpotName = emoteSpot.name;
            if (emoteSpotName == "RPSJoinSpot")
            {
                if (NetworkServer.active)
                {
                    int winner = Random.Range(0, 2);
                    int hostSpot = Random.Range(0, 3);
                    int clientSpot;
                    if (winner == 0)
                    {
                        clientSpot = hostSpot - 1;
                    }
                    else
                    {
                        clientSpot = hostSpot + 1;
                    }
                    if (clientSpot > 2)
                    {
                        clientSpot -= 3;
                    }
                    if (clientSpot < 0)
                    {
                        clientSpot += 3;
                    }

                    hostSpot += host.props[0].GetComponent<RockPaperScissors>().charType * 3;
                    clientSpot += Random.Range(0, RPS_Start_Emotes.Count) * 3;

                    if (winner == 0)
                    {
                        new SyncRandomEmoteToHost(host.transform.parent.GetComponent<CharacterModel>().body.GetComponent<NetworkIdentity>().netId, "RPS_Win", hostSpot, host.transform.parent.GetComponent<CharacterModel>().body.GetComponent<NetworkIdentity>().netId).Send(NetworkDestination.Server);
                        new SyncRandomEmoteToHost(joiner.transform.parent.GetComponent<CharacterModel>().body.GetComponent<NetworkIdentity>().netId, "RPS_Loss", clientSpot, host.transform.parent.GetComponent<CharacterModel>().body.GetComponent<NetworkIdentity>().netId).Send(NetworkDestination.Server);
                    }
                    else
                    {
                        new SyncRandomEmoteToHost(host.transform.parent.GetComponent<CharacterModel>().body.GetComponent<NetworkIdentity>().netId, "RPS_Loss", hostSpot, host.transform.parent.GetComponent<CharacterModel>().body.GetComponent<NetworkIdentity>().netId).Send(NetworkDestination.Server);
                        new SyncRandomEmoteToHost(joiner.transform.parent.GetComponent<CharacterModel>().body.GetComponent<NetworkIdentity>().netId, "RPS_Win", clientSpot, host.transform.parent.GetComponent<CharacterModel>().body.GetComponent<NetworkIdentity>().netId).Send(NetworkDestination.Server);
                    }
                }
            }
            else if (emoteSpotName == "FlipJoinSpot")
            {
                if (NetworkServer.active)
                {
                    int hostSpot = host.props[0].GetComponent<Flip>().charType;
                    int clientSpot = Random.Range(0, Flip_Flip_Emotes.Count);
                    new SyncRandomEmoteToHost(host.transform.parent.GetComponent<CharacterModel>().body.GetComponent<NetworkIdentity>().netId, "Flip_Throw", hostSpot, host.transform.parent.GetComponent<CharacterModel>().body.GetComponent<NetworkIdentity>().netId).Send(NetworkDestination.Server);
                    new SyncRandomEmoteToHost(joiner.transform.parent.GetComponent<CharacterModel>().body.GetComponent<NetworkIdentity>().netId, "Flip_Flip", clientSpot, host.transform.parent.GetComponent<CharacterModel>().body.GetComponent<NetworkIdentity>().netId).Send(NetworkDestination.Server);
                }
            }
        }

        private void CustomEmotesAPI_animChanged(string newAnimation, BoneMapper mapper)
        {
            if (!mapper.gameObject.GetComponent<TF2EmoteTracker>())
            {
                mapper.gameObject.AddComponent<TF2EmoteTracker>();
            }
            if (newAnimation == "RanchoRelaxo")
            {
                GameObject g = GameObject.Instantiate(Assets.Load<GameObject>("@BadAssEmotes_badassemotes:Assets/Engi/Rancho/RanchoRelaxo.prefab"));
                mapper.props.Add(g);
                g.transform.SetParent(mapper.transform.parent);
                g.transform.localEulerAngles = Vector3.one;
                g.transform.localPosition = Vector3.zero;
                g.transform.localScale = new Vector3(1.175f, 1.175f, 1.175f);
                mapper.ScaleProps();
                mapper.props.RemoveAt(mapper.props.Count - 1);
                ChairHandler chair = g.AddComponent<ChairHandler>();
                chair.pos = g.transform.position;
                chair.rot = g.transform.eulerAngles;
                chair.scal = g.transform.lossyScale;
                chair.mapper = mapper;

                g = new GameObject();
                g.name = "RanchoRelaxoProp";
                mapper.props.Add(g);
                g.transform.localPosition = mapper.transform.position;
                g.transform.localEulerAngles = mapper.transform.eulerAngles;
                g.transform.localScale = Vector3.one;
                mapper.AssignParentGameObject(g, true, true, true, false, false);
                chair.chair = g;
            }
            //if (newAnimation == "engiRanchoBurp" || newAnimation == "engiRanchoBigDrink" || newAnimation == "engiRanchoQuickDrink")
            //{
            //    mapper.AssignParentGameObject(mapper.props[0], true, true, true, false, false);
            //}
            if (newAnimation == "Rock Paper Scissors")
            {
                if (NetworkServer.active)
                {
                    new SyncRandomEmoteToHost(mapper.transform.parent.GetComponent<CharacterModel>().body.GetComponent<NetworkIdentity>().netId, "RPS_Start", Random.Range(0, RPS_Start_Emotes.Count), mapper.transform.parent.GetComponent<CharacterModel>().body.GetComponent<NetworkIdentity>().netId).Send(NetworkDestination.Server);
                }
            }
            if (newAnimation == "Flippin' Awesome")
            {
                if (NetworkServer.active)
                {
                    new SyncRandomEmoteToHost(mapper.transform.parent.GetComponent<CharacterModel>().body.GetComponent<NetworkIdentity>().netId, "Flip_Wait", Random.Range(0, Flip_Wait_Emotes.Count), mapper.transform.parent.GetComponent<CharacterModel>().body.GetComponent<NetworkIdentity>().netId).Send(NetworkDestination.Server);
                }
            }
            if (newAnimation == "Conga")
            {
                if (NetworkServer.active)
                {
                    mapper.gameObject.GetComponent<TF2EmoteTracker>().currentAnimation = "Medic_Conga";
                    new SyncRandomEmoteToHost(mapper.transform.parent.GetComponent<CharacterModel>().body.GetComponent<NetworkIdentity>().netId, "Conga_Start", Random.Range(0, Conga_Emotes.Count), mapper.transform.parent.GetComponent<CharacterModel>().body.GetComponent<NetworkIdentity>().netId).Send(NetworkDestination.Server);
                }
            }
            if (newAnimation == "Kazotsky Kick")
            {
                if (NetworkServer.active)
                {
                    mapper.gameObject.GetComponent<TF2EmoteTracker>().currentAnimation = "Medic_Kazotsky";
                    new SyncRandomEmoteToHost(mapper.transform.parent.GetComponent<CharacterModel>().body.GetComponent<NetworkIdentity>().netId, "Kazotsky_Start", Random.Range(0, KazotskyKick_Emotes.Count), mapper.transform.parent.GetComponent<CharacterModel>().body.GetComponent<NetworkIdentity>().netId).Send(NetworkDestination.Server);
                }
            }
            if (newAnimation.EndsWith("_Conga") && !mapper.gameObject.GetComponent<TF2EmoteTracker>().currentAnimation.EndsWith("_Conga"))
            {
                if (NetworkServer.active)
                {
                    mapper.gameObject.GetComponent<TF2EmoteTracker>().currentAnimation = "Medic_Conga";
                    new SyncRandomEmoteToHost(mapper.transform.parent.GetComponent<CharacterModel>().body.GetComponent<NetworkIdentity>().netId, "Conga_Start", Random.Range(0, Conga_Emotes.Count), mapper.transform.parent.GetComponent<CharacterModel>().body.GetComponent<NetworkIdentity>().netId).Send(NetworkDestination.Server);
                }
            }
            if (newAnimation.StartsWith("Kazotsky_") && !mapper.gameObject.GetComponent<TF2EmoteTracker>().currentAnimation.EndsWith("_Kazotsky"))
            {
                if (NetworkServer.active)
                {
                    mapper.gameObject.GetComponent<TF2EmoteTracker>().currentAnimation = "Medic_Kazotsky";
                    new SyncRandomEmoteToHost(mapper.transform.parent.GetComponent<CharacterModel>().body.GetComponent<NetworkIdentity>().netId, "Kazotsky_Start", Random.Range(0, KazotskyKick_Emotes.Count), mapper.transform.parent.GetComponent<CharacterModel>().body.GetComponent<NetworkIdentity>().netId).Send(NetworkDestination.Server);
                }
            }

            mapper.gameObject.GetComponent<TF2EmoteTracker>().currentAnimation = newAnimation;
            DEBUGHANDLE(mapper, newAnimation);
        }

        void Update()
        {

        }

        internal void AddAnimation(string AnimClip, string wwise, bool looping, bool dimAudio, bool sync)
        {
            CustomEmotesAPI.AddCustomAnimation(Assets.Load<AnimationClip>($"@ExampleEmotePlugin_badassemotes:assets/{AnimClip}.anim"), looping, $"Play_{wwise}", $"Stop_{wwise}", dimWhenClose: dimAudio, syncAnim: sync, syncAudio: sync);
        }

        internal void AddAnimation(string AnimClip, string wwise, string AnimClip2ElectricBoogaloo, bool dimAudio, bool sync)
        {
            CustomEmotesAPI.AddCustomAnimation(Assets.Load<AnimationClip>($"@ExampleEmotePlugin_badassemotes:assets/{AnimClip}.anim"), true, $"Play_{wwise}", $"Stop_{wwise}", secondaryAnimation: Assets.Load<AnimationClip>($"@ExampleEmotePlugin_badassemotes:assets/{AnimClip2ElectricBoogaloo}.anim"), dimWhenClose: dimAudio, syncAnim: sync, syncAudio: sync);
        }
        internal void AddAnimation(string AnimClip, string wwise, string AnimClip2ElectricBoogaloo, bool dimAudio, bool sync, bool visibility)
        {
            CustomEmotesAPI.AddCustomAnimation(Assets.Load<AnimationClip>($"@ExampleEmotePlugin_badassemotes:assets/{AnimClip}.anim"), true, $"Play_{wwise}", $"Stop_{wwise}", secondaryAnimation: Assets.Load<AnimationClip>($"@ExampleEmotePlugin_badassemotes:assets/{AnimClip2ElectricBoogaloo}.anim"), dimWhenClose: dimAudio, syncAnim: sync, syncAudio: sync, visible: visibility);
        }
        internal string AddHiddenAnimation(string[] AnimClip, string[] AnimClip2ElectricBoogaloo, string[] wwise, string stopWwise, JoinSpot[] joinSpots)
        {
            List<string> stopwwise = new List<string>();
            foreach (var item in wwise)
            {
                stopwwise.Add($"Stop_{stopWwise}");
            }
            List<AnimationClip> primary = new List<AnimationClip>();
            foreach (var item in AnimClip)
            {
                primary.Add(Assets.Load<AnimationClip>($"@ExampleEmotePlugin_badassemotes:assets/{item}.anim"));
            }
            List<AnimationClip> secondary = new List<AnimationClip>();
            foreach (var item in AnimClip2ElectricBoogaloo)
            {
                secondary.Add(Assets.Load<AnimationClip>($"@ExampleEmotePlugin_badassemotes:assets/{item}.anim"));
            }
            string emote = AnimClip[0].Split('/')[AnimClip[0].Split('/').Length - 1]; ;
            CustomEmotesAPI.AddCustomAnimation(primary.ToArray(), true, wwise, stopwwise.ToArray(), secondaryAnimation: secondary.ToArray(), joinSpots: joinSpots, visible: false);
            return emote;
        }
        internal string AddHiddenAnimation(string[] AnimClip, string[] wwise, string stopWwise)
        {
            List<string> stopwwise = new List<string>();
            foreach (var item in wwise)
            {
                stopwwise.Add($"Stop_{stopWwise}");
            }
            List<AnimationClip> primary = new List<AnimationClip>();
            foreach (var item in AnimClip)
            {
                primary.Add(Assets.Load<AnimationClip>($"@ExampleEmotePlugin_badassemotes:assets/{item}.anim"));
            }
            string emote = AnimClip[0].Split('/')[AnimClip[0].Split('/').Length - 1]; ;
            CustomEmotesAPI.AddCustomAnimation(primary.ToArray(), false, wwise, stopwwise.ToArray(), visible: false);
            return emote;
        }
        internal string AddHiddenAnimation(string[] AnimClip, string[] wwise, string stopWwise, bool sync)
        {
            List<string> stopwwise = new List<string>();
            foreach (var item in wwise)
            {
                stopwwise.Add($"Stop_{stopWwise}");
            }
            List<AnimationClip> primary = new List<AnimationClip>();
            foreach (var item in AnimClip)
            {
                primary.Add(Assets.Load<AnimationClip>($"@ExampleEmotePlugin_badassemotes:assets/{item}.anim"));
            }
            string emote = AnimClip[0].Split('/')[AnimClip[0].Split('/').Length - 1]; ;
            CustomEmotesAPI.AddCustomAnimation(primary.ToArray(), true, wwise, stopwwise.ToArray(), visible: false, syncAudio: sync);
            return emote;
        }
        internal string AddHiddenAnimation(string[] AnimClip, string[] wwise, string stopWwise, bool sync, string[] AnimClip2)
        {
            List<string> stopwwise = new List<string>();
            foreach (var item in wwise)
            {
                stopwwise.Add($"Stop_{stopWwise}");
            }
            List<AnimationClip> primary = new List<AnimationClip>();
            foreach (var item in AnimClip)
            {
                primary.Add(Assets.Load<AnimationClip>($"@ExampleEmotePlugin_badassemotes:assets/{item}.anim"));
            }
            List<AnimationClip> secondary = new List<AnimationClip>();
            foreach (var item in AnimClip2)
            {
                secondary.Add(Assets.Load<AnimationClip>($"@ExampleEmotePlugin_badassemotes:assets/{item}.anim"));
            }
            string emote = AnimClip[0].Split('/')[AnimClip[0].Split('/').Length - 1]; ;
            CustomEmotesAPI.AddCustomAnimation(primary.ToArray(), false, wwise, stopwwise.ToArray(), visible: false, syncAudio: sync, secondaryAnimation: secondary.ToArray());
            return emote;
        }
        void DEBUG()
        {
            CustomEmotesAPI.AddNonAnimatingEmote("SpawnEnemies");
            CustomEmotesAPI.BlackListEmote("SpawnEnemies");
            CustomEmotesAPI.AddNonAnimatingEmote("EnemyHonda");
            CustomEmotesAPI.BlackListEmote("EnemyHonda");
            CustomEmotesAPI.AddNonAnimatingEmote("EnemyCum");
            CustomEmotesAPI.BlackListEmote("EnemyCum");
            CustomEmotesAPI.AddNonAnimatingEmote("EnemyDodge");
            CustomEmotesAPI.BlackListEmote("EnemyDodge");
            CustomEmotesAPI.AddNonAnimatingEmote("EnemyStand");
            CustomEmotesAPI.BlackListEmote("EnemyStand");
            CustomEmotesAPI.AddNonAnimatingEmote("EnemyJoin");
            CustomEmotesAPI.BlackListEmote("EnemyJoin");
            CustomEmotesAPI.AddNonAnimatingEmote("EnemyRPS");
            CustomEmotesAPI.BlackListEmote("EnemyRPS");
            CustomEmotesAPI.AddNonAnimatingEmote("EnemyConga");
            CustomEmotesAPI.BlackListEmote("EnemyConga");
            CustomEmotesAPI.AddNonAnimatingEmote("EnemyNone");
            CustomEmotesAPI.BlackListEmote("EnemyNone");
            CustomEmotesAPI.AddNonAnimatingEmote("EnemyGetDown");
            CustomEmotesAPI.BlackListEmote("EnemyGetDown");
            CustomEmotesAPI.AddNonAnimatingEmote("EnemyBreak");
            CustomEmotesAPI.BlackListEmote("EnemyBreak");
            CustomEmotesAPI.AddNonAnimatingEmote("SpawnBody");
            CustomEmotesAPI.BlackListEmote("SpawnBody");
            CustomEmotesAPI.AddNonAnimatingEmote("EnemyKazotsky");
            CustomEmotesAPI.BlackListEmote("EnemyKazotsky");
        }
        void DEBUGHANDLE(BoneMapper mapper, string newAnimation)
        {
            if (mapper.worldProp)
            {
                return;
            }
            if (newAnimation == "SpawnEnemies")
            {
                StartCoroutine(SpawnEnemies());
            }
            if (newAnimation == "EnemyHonda")
            {
                foreach (var item in CustomEmotesAPI.GetAllBoneMappers())
                {
                    if (item != CustomEmotesAPI.localMapper)
                    {
                        CustomEmotesAPI.PlayAnimation("HondaStep", item);
                    }
                }
            }
            if (newAnimation == "EnemyKazotsky")
            {
                foreach (var item in CustomEmotesAPI.GetAllBoneMappers())
                {
                    if (item != CustomEmotesAPI.localMapper)
                    {
                        CustomEmotesAPI.PlayAnimation("Kazotsky Kick", item);
                    }
                }
            }
            if (newAnimation == "EnemyNone")
            {
                foreach (var item in CustomEmotesAPI.GetAllBoneMappers())
                {
                    if (item != CustomEmotesAPI.localMapper)
                    {
                        CustomEmotesAPI.PlayAnimation("none", item);
                    }
                }
            }
            if (newAnimation == "EnemyConga")
            {
                foreach (var item in CustomEmotesAPI.GetAllBoneMappers())
                {
                    if (item != CustomEmotesAPI.localMapper)
                    {
                        CustomEmotesAPI.PlayAnimation("Conga", item);
                    }
                }
            }
            if (newAnimation == "EnemyRPS")
            {
                foreach (var item in CustomEmotesAPI.GetAllBoneMappers())
                {
                    if (item != CustomEmotesAPI.localMapper)
                    {
                        CustomEmotesAPI.PlayAnimation("Rock Paper Scissors", item);
                    }
                }
            }
            if (newAnimation == "EnemyJoin")
            {
                foreach (var item in CustomEmotesAPI.GetAllBoneMappers())
                {
                    if (item != CustomEmotesAPI.localMapper && item.currentEmoteSpot)
                    {
                        item.JoinEmoteSpot();
                    }
                }
            }
            if (newAnimation == "EnemyStand")
            {
                foreach (var item in CustomEmotesAPI.GetAllBoneMappers())
                {
                    if (item != CustomEmotesAPI.localMapper)
                    {
                        CustomEmotesAPI.PlayAnimation("StoodHere", item);
                    }
                }
            }
            if (newAnimation == "EnemyBreak")
            {
                foreach (var item in CustomEmotesAPI.GetAllBoneMappers())
                {
                    if (item != CustomEmotesAPI.localMapper)
                    {
                        CustomEmotesAPI.PlayAnimation("Breakneck", item);
                    }
                }
            }
            if (newAnimation == "EnemyGetDown")
            {
                foreach (var item in CustomEmotesAPI.GetAllBoneMappers())
                {
                    if (item != CustomEmotesAPI.localMapper)
                    {
                        CustomEmotesAPI.PlayAnimation("GetDown", item);
                    }
                }
            }
            if (newAnimation == "EnemyDodge")
            {
                foreach (var item in CustomEmotesAPI.GetAllBoneMappers())
                {
                    if (item != CustomEmotesAPI.localMapper)
                    {
                        CustomEmotesAPI.PlayAnimation("DuckThisOneIdle", item);
                    }
                }
            }
            if (newAnimation == "EnemyCum")
            {
                foreach (var item in CustomEmotesAPI.GetAllBoneMappers())
                {
                    if (item != CustomEmotesAPI.localMapper)
                    {
                        CustomEmotesAPI.PlayAnimation("Cum Throne", item);
                    }
                }
            }
            if (newAnimation == "SpawnBody")
            {
                switch (Random.Range(0, 12))
                {
                    case 0:
                        RoR2.Console.instance.SubmitCmd(NetworkUser.readOnlyLocalPlayersList[0], $"spawn_body Acrid");
                        break;
                    case 1:
                        RoR2.Console.instance.SubmitCmd(NetworkUser.readOnlyLocalPlayersList[0], $"spawn_body Artificer");
                        break;
                    case 2:
                        RoR2.Console.instance.SubmitCmd(NetworkUser.readOnlyLocalPlayersList[0], $"spawn_body Bandit2");
                        break;
                    case 3:
                        RoR2.Console.instance.SubmitCmd(NetworkUser.readOnlyLocalPlayersList[0], $"spawn_body Captain");
                        break;
                    case 4:
                        RoR2.Console.instance.SubmitCmd(NetworkUser.readOnlyLocalPlayersList[0], $"spawn_body Commando");
                        break;
                    case 5:
                        RoR2.Console.instance.SubmitCmd(NetworkUser.readOnlyLocalPlayersList[0], $"spawn_body Engi");
                        break;
                    case 6:
                        RoR2.Console.instance.SubmitCmd(NetworkUser.readOnlyLocalPlayersList[0], $"spawn_body Huntress");
                        break;
                    case 7:
                        RoR2.Console.instance.SubmitCmd(NetworkUser.readOnlyLocalPlayersList[0], $"spawn_body Loader");
                        break;
                    case 8:
                        RoR2.Console.instance.SubmitCmd(NetworkUser.readOnlyLocalPlayersList[0], $"spawn_body MULT");
                        break;
                    case 9:
                        RoR2.Console.instance.SubmitCmd(NetworkUser.readOnlyLocalPlayersList[0], $"spawn_body Mercenary");
                        break;
                    case 10:
                        RoR2.Console.instance.SubmitCmd(NetworkUser.readOnlyLocalPlayersList[0], $"spawn_body REX");
                        break;
                    case 11:
                        RoR2.Console.instance.SubmitCmd(NetworkUser.readOnlyLocalPlayersList[0], $"spawn_body Railgunner");
                        break;

                    default:
                        break;
                }
            }
        }
        IEnumerator SpawnEnemies()
        {
            RoR2.Console.instance.SubmitCmd(NetworkUser.readOnlyLocalPlayersList[0], $"spawn_body BeetleQueen");
            yield return new WaitForSeconds(.75f);
            RoR2.Console.instance.SubmitCmd(NetworkUser.readOnlyLocalPlayersList[0], $"spawn_body Larva");
            yield return new WaitForSeconds(.75f);
            RoR2.Console.instance.SubmitCmd(NetworkUser.readOnlyLocalPlayersList[0], $"spawn_body BeetleGuard");
            yield return new WaitForSeconds(.75f);
            RoR2.Console.instance.SubmitCmd(NetworkUser.readOnlyLocalPlayersList[0], $"spawn_body Mithrix");
            yield return new WaitForSeconds(.75f);
            RoR2.Console.instance.SubmitCmd(NetworkUser.readOnlyLocalPlayersList[0], $"spawn_body ClayTemplar");
            yield return new WaitForSeconds(.75f);
            RoR2.Console.instance.SubmitCmd(NetworkUser.readOnlyLocalPlayersList[0], $"spawn_body Aurelionite");
        }
    }
}

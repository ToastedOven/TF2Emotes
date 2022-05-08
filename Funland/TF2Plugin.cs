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
        internal static List<string> RPS_Start_Emotes = new List<string>();
        internal static List<string> RPS_Loss_Emotes = new List<string>();
        internal static List<string> RPS_Win_Emotes = new List<string>();
        internal static Shader defaultShader = Addressables.LoadAssetAsync<GameObject>("RoR2/Base/Commando/CommandoBody.prefab").WaitForCompletion().GetComponentInChildren<SkinnedMeshRenderer>().material.shader;
        public void Awake()
        {
            Assets.PopulateAssets();
            Assets.AddSoundBank("tf2.bnk");
            Assets.LoadSoundBanks();
            AddAnimation("Engi/Rancho/RanchoRelaxo", "Rancho", "Engi/Rancho/RanchoLoop", false, false);
            RPS();
            Register();
            CustomEmotesAPI.animChanged += CustomEmotesAPI_animChanged;
            CustomEmotesAPI.emoteSpotJoined_Body += CustomEmotesAPI_emoteSpotJoined_Body;
        }
        void Register()
        {
            NetworkingAPI.RegisterMessageType<SyncRandomEmoteToClient>();
            NetworkingAPI.RegisterMessageType<SyncRandomEmoteToHost>();
        }
        public void RPS()
        {
            CustomEmotesAPI.AddNonAnimatingEmote("Rock Paper Scissors");
            CustomEmotesAPI.AddNonAnimatingEmote("Rock", false);
            CustomEmotesAPI.AddNonAnimatingEmote("Paper", false);
            CustomEmotesAPI.AddNonAnimatingEmote("Scissors", false);
            string emote = AddHiddenAnimation(new string[] { "Engi/RPS/EngiRPSStart" }, new string[] { "Engi/RPS/EngiRPSLoop" }, new string[] { "" }, "", new JoinSpot[] { new JoinSpot("RPSJoinSpot", new Vector3(0, 0, 1.5f)) });
            RPS_Start_Emotes.Add(emote);

            emote = AddHiddenAnimation(new string[] { "Engi/RPS/EngiRPS_RWin" }, new string[] { "" }, "");
            RPS_Win_Emotes.Add(emote);
            emote = AddHiddenAnimation(new string[] { "Engi/RPS/EngiRPS_RLose" }, new string[] { "" }, "");
            RPS_Loss_Emotes.Add(emote);

            emote = AddHiddenAnimation(new string[] { "Engi/RPS/EngiRPS_PWin" }, new string[] { "" }, "");
            RPS_Win_Emotes.Add(emote);
            emote = AddHiddenAnimation(new string[] { "Engi/RPS/EngiRPS_PLose" }, new string[] { "" }, "");
            RPS_Loss_Emotes.Add(emote);

            emote = AddHiddenAnimation(new string[] { "Engi/RPS/EngiRPS_SWin" }, new string[] { "" }, "");
            RPS_Win_Emotes.Add(emote);
            emote = AddHiddenAnimation(new string[] { "Engi/RPS/EngiRPS_SLose" }, new string[] { "" }, "");
            RPS_Loss_Emotes.Add(emote);

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
        }

        private void CustomEmotesAPI_animChanged(string newAnimation, BoneMapper mapper)
        {
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
                ChairDissasembler chair = g.AddComponent<ChairDissasembler>();
                chair.pos = g.transform.position;
                chair.rot = g.transform.eulerAngles;
                chair.scal = g.transform.lossyScale;

                g = new GameObject();
                g.name = "RanchoRelaxoProp";
                mapper.props.Add(g);
                g.transform.localPosition = mapper.transform.position;
                g.transform.localEulerAngles = mapper.transform.eulerAngles;
                g.transform.localScale = Vector3.one;
                mapper.AssignParentGameObject(g, true, true, true, false, false);
                chair.chair = g;
            }
            if (newAnimation == "Rock Paper Scissors")
            {
                if (NetworkServer.active)
                {
                    new SyncRandomEmoteToHost(mapper.transform.parent.GetComponent<CharacterModel>().body.GetComponent<NetworkIdentity>().netId, "RPS_Start", Random.Range(0, RPS_Start_Emotes.Count), mapper.transform.parent.GetComponent<CharacterModel>().body.GetComponent<NetworkIdentity>().netId).Send(NetworkDestination.Server);
                }
            }
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
    }
}

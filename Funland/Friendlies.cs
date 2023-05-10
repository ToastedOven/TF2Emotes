using EmotesAPI;
using RoR2;
using RoR2.CharacterAI;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace TitanFall2Emotes
{
    public class FriendlyComponent : MonoBehaviour
    {
        public HealthComponent healthComponent;
        public BoneMapper boneMapper;
        public CharacterBody body;
        public bool friendly = true;
        void Awake()
        {

        }
        void Update()
        {
            if (boneMapper.currentClipName != "none" && body.GetComponent<TeamComponent>().teamIndex != TeamIndex.Player)
            {
                if (healthComponent.timeSinceLastHit < 5)
                {
                    CustomEmotesAPI.PlayAnimation("none", boneMapper);
                }
            }
        }
        internal static void FriendlySetup()
        {
            On.RoR2.GenericSkill.ExecuteIfReady += GenericSkill_ExecuteIfReady;
            CustomEmotesAPI.boneMapperCreated += CustomEmotesAPI_boneMapperCreated;
        }

        private static bool GenericSkill_ExecuteIfReady(On.RoR2.GenericSkill.orig_ExecuteIfReady orig, GenericSkill self)
        {

            FriendlyComponent friend = self.gameObject.GetComponent<FriendlyComponent>();
            TeamComponent t = self.GetComponent<TeamComponent>();
            if (friend && t && t.teamIndex != TeamIndex.Player)
            {
                if (!friend.friendly)
                {
                    return orig(self);
                }
                if (friend.boneMapper.currentClipName != "none")
                {
                    return false;
                }
                if (Settings.EnemiesEmoteWithYou.Value &&
                    self.gameObject.GetComponent<HealthComponent>().timeSinceLastHit > 5)
                {
                    BaseAI.Target target = self.GetComponent<CharacterBody>().master.GetComponent<BaseAI>().currentEnemy;
                    if (target != null)
                    {
                        string currentEmoteOfTarget = target.characterBody.GetComponent<FriendlyComponent>().boneMapper.currentClipName;
                        if (currentEmoteOfTarget.EndsWith("_Conga"))
                        {
                            CustomEmotesAPI.PlayAnimation("Conga", friend.boneMapper);
                        }
                        else if (currentEmoteOfTarget.StartsWith("Kazotsky"))
                        {
                            CustomEmotesAPI.PlayAnimation("Kazotsky Kick", friend.boneMapper);
                        }
                        else if (currentEmoteOfTarget.EndsWith("RPSStart") || currentEmoteOfTarget.EndsWith("RPS_Start") || currentEmoteOfTarget.EndsWith("Flip_Start") || currentEmoteOfTarget.EndsWith("Flip_Throw") || currentEmoteOfTarget.EndsWith("Flip_Flip") || currentEmoteOfTarget.EndsWith("Lose") || currentEmoteOfTarget.EndsWith("Win"))
                        {

                        }
                        else
                        {
                            return orig(self);
                        }
                        return false;
                    }
                }
            }
            return orig(self);
        }

        private static void CustomEmotesAPI_boneMapperCreated(BoneMapper mapper)
        {
            CharacterBody body = mapper.transform.GetComponentInParent<CharacterModel>().body;
            FriendlyComponent f = body.gameObject.AddComponent<FriendlyComponent>();
            f.healthComponent = body.GetComponent<HealthComponent>();
            f.boneMapper = mapper;
            if (!Settings.PartyCrashers.Value)
            {
                f.friendly = true;
            }
            else
            {
                f.friendly = mapper.name != "elderlemurian" && UnityEngine.Random.Range(0, 100) < 94;
            }
            f.body = body;
            if (Settings.EnemiesEmoteWithYou.Value && UnityEngine.Random.Range(0, 100) > 88 && f.friendly && body.GetComponent<TeamComponent>().teamIndex != TeamIndex.Player)
            {
                switch (UnityEngine.Random.Range(0, 5))
                {
                    case 0:
                        TF2Plugin.Instance.PlayAfterSecondsNotIEnumerator(mapper, "RanchoRelaxo", .5f);
                        break;
                    case 1:
                        TF2Plugin.Instance.PlayAfterSecondsNotIEnumerator(mapper, "Flippin' Awesome", .5f);
                        break;
                    case 2:
                        TF2Plugin.Instance.PlayAfterSecondsNotIEnumerator(mapper, "Rock Paper Scissors", .5f);
                        break;
                    case 3:
                        TF2Plugin.Instance.PlayAfterSecondsNotIEnumerator(mapper, "Kazotsky Kick", .5f);
                        break;
                    case 4:
                        TF2Plugin.Instance.PlayAfterSecondsNotIEnumerator(mapper, "Conga", .5f);
                        break;

                    default:
                        break;
                }
            }
        }
    }
}

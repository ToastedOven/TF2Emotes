﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace TitanFall2Emotes
{
    class ChairHandler : MonoBehaviour
    {
        internal GameObject chair;
        bool check = true;
        internal Vector3 pos;
        internal Vector3 rot;
        internal Vector3 scal;
        bool check2 = false;
        bool check3 = false;
        internal BoneMapper mapper;
        float timer = 0;
        int whenToEmote = 0;

        void Start()
        {
            foreach (var item in GetComponentInChildren<SkinnedMeshRenderer>().materials)
            {
                item.shader = TF2Plugin.defaultShader;
                item.shaderKeywords = new string[] { "DITHER" };
            }
            whenToEmote = UnityEngine.Random.Range(15, 25);
        }
        void Update()
        {
            if (check)
            {
                timer += Time.deltaTime;
            }
            if (timer > whenToEmote)
            {
                timer = 0;
                whenToEmote = UnityEngine.Random.Range(15, 25);
                switch (UnityEngine.Random.Range(0, 3))
                {
                    case 0:
                        mapper.preserveParent = true;
                        mapper.preserveProps = true;
                        DebugClass.Log($"burp");
                        mapper.PlayAnim("engiRanchoBurp", 0);
                        AkSoundEngine.PostEvent("Play_RanchoBurp", mapper.gameObject);
                        break;
                    case 1:
                        mapper.preserveParent = true;
                        mapper.preserveProps = true;
                        DebugClass.Log($"bigDrink");
                        mapper.PlayAnim("engiRanchoBigDrink", 0);
                        AkSoundEngine.PostEvent("Play_RanchoLong", mapper.gameObject);
                        GetComponentInChildren<Animator>().SetBool("BigDrink", true);
                        break;
                    case 2:
                        mapper.preserveParent = true;
                        mapper.preserveProps = true;
                        DebugClass.Log($"quickDrink");
                        mapper.PlayAnim("engiRanchoQuickDrink", 0);
                        AkSoundEngine.PostEvent("Play_RanchoQuick", mapper.gameObject);
                        GetComponentInChildren<Animator>().SetBool("SmallDrink", true);
                        break;
                    default:
                        break;
                }
            }
            else if (timer > 3)
            {
                GetComponentInChildren<Animator>().SetBool("BigDrink", false);
                GetComponentInChildren<Animator>().SetBool("SmallDrink", false);
            }
            if (check && !chair)
            {
                check = false;
                StartCoroutine(spinThenDestroy());
                GetComponentInChildren<Animator>().SetBool("Breaking", true);
                gameObject.transform.SetParent(null);
                gameObject.transform.position = pos;
                gameObject.transform.localEulerAngles = rot;
                gameObject.transform.localScale = scal;
                scal *= 1.333f;
                AkSoundEngine.PostEvent("Stop_Rancho", mapper.gameObject);
                AkSoundEngine.PostEvent("Play_RanchoClose", gameObject);
            }
            if (check3)
            {
                gameObject.transform.localScale = Vector3.Slerp(gameObject.transform.localScale, Vector3.zero, Time.deltaTime * 5);
            }
            else if (check2)
            {
                gameObject.transform.localScale = Vector3.Slerp(gameObject.transform.localScale, scal, Time.deltaTime * 30);
            }
        }

        IEnumerator spinThenDestroy()
        {
            yield return new WaitForSeconds(3.5f);
            check2 = true;
            yield return new WaitForSeconds(.15f);
            check3 = true;
            yield return new WaitForSeconds(1f);
            Destroy(gameObject);
        }
    }
}

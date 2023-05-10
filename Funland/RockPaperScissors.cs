﻿using RoR2;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace TitanFall2Emotes
{
    class RockPaperScissors : MonoBehaviour
    {
        public int charType;
        public static void RPSStart(BoneMapper joinerMapper, int spot)
        {
            joinerMapper.PlayAnim(TF2Plugin.RPS_Start_Emotes[spot], 0);
            joinerMapper.props.Add(new GameObject());
            joinerMapper.props[0].AddComponent<RockPaperScissors>().charType = spot;

            GameObject g = new GameObject();
            g.name = "RPS_StartProp";
            joinerMapper.props.Add(g);
            g.transform.localPosition = joinerMapper.transform.position;
            g.transform.localEulerAngles = joinerMapper.transform.eulerAngles;
            g.transform.localScale = Vector3.one;
            joinerMapper.AssignParentGameObject(g, true, true, false, false, false);
        }
        public static void RPSWin(BoneMapper joinerMapper, int spot, BoneMapper hostJoinerMapper, TeamIndex joinerIndex, TeamIndex hostIndex)
        {
            joinerMapper.PlayAnim(TF2Plugin.RPS_Win_Emotes[spot], 0);

            GameObject g = new GameObject();
            g.name = "RPS_WinProp";
            joinerMapper.props.Add(g);
            if (hostJoinerMapper != joinerMapper)
            {
                g.transform.SetParent(hostJoinerMapper.transform);
                Vector3 scal = hostJoinerMapper.transform.lossyScale;
                g.transform.localPosition = new Vector3(0, 0, 2.5f / scal.z);
                g.transform.localEulerAngles = new Vector3(0, 180, 0);
                g.transform.localScale = Vector3.one;
                joinerMapper.AssignParentGameObject(g, true, true, false, false, true);
            }
            else
            {
                g.transform.localPosition = joinerMapper.transform.position;
                g.transform.localEulerAngles = joinerMapper.transform.eulerAngles;
                g.transform.localScale = Vector3.one;
                joinerMapper.AssignParentGameObject(g, true, true, false, false, false);
            }

            string Team = "Red";
            if (joinerIndex != hostIndex)
            {
                Team = "Blu";
                TF2Plugin.Instance.KillAfterSecondsNotIEnumerator(hostJoinerMapper, 6.5f);
            }
            //0 rock
            //1 paper
            //2 scissors
            int prop1 = joinerMapper.props.Count;
            switch (spot % 3)
            {
                case 0:
                    joinerMapper.props.Add(GameObject.Instantiate(Assets.Load<GameObject>($"@BadAssEmotes_badassemotes:Assets/RPS/{Team}_Rock_Win.prefab")));
                    break;
                case 1:
                    joinerMapper.props.Add(GameObject.Instantiate(Assets.Load<GameObject>($"@BadAssEmotes_badassemotes:Assets/RPS/{Team}_Paper_Win.prefab")));
                    break;
                case 2:
                    joinerMapper.props.Add(GameObject.Instantiate(Assets.Load<GameObject>($"@BadAssEmotes_badassemotes:Assets/RPS/{Team}_Scissors_Win.prefab")));
                    break;
                default:
                    break;
            }
            joinerMapper.props[prop1].transform.SetParent(joinerMapper.parentGameObject.transform);
            joinerMapper.props[prop1].transform.localEulerAngles = Vector3.zero;
            joinerMapper.props[prop1].transform.localPosition = new Vector3(0, 2.5f * joinerMapper.props[prop1].transform.lossyScale.y, 0);
            joinerMapper.ScaleProps();
        }
        public static void RPSLose(BoneMapper joinerMapper, int spot, BoneMapper hostJoinerMapper, TeamIndex joinerIndex, TeamIndex hostIndex)
        {
            joinerMapper.PlayAnim(TF2Plugin.RPS_Loss_Emotes[spot], 0);

            GameObject g = new GameObject();
            g.name = "RPS_LossProp";
            joinerMapper.props.Add(g);
            if (hostJoinerMapper != joinerMapper)
            {
                g.transform.SetParent(hostJoinerMapper.transform);
                Vector3 scal = hostJoinerMapper.transform.lossyScale;
                g.transform.localPosition = new Vector3(0, 0, 2.5f / scal.z);
                g.transform.localEulerAngles = new Vector3(0, 180, 0);
                g.transform.localScale = Vector3.one;
                joinerMapper.AssignParentGameObject(g, true, true, false, false, true);
            }
            else
            {
                g.transform.localPosition = joinerMapper.transform.position;
                g.transform.localEulerAngles = joinerMapper.transform.eulerAngles;
                g.transform.localScale = Vector3.one;
                joinerMapper.AssignParentGameObject(g, true, true, false, false, false);
            }

            string Team2 = "Red";
            if (joinerIndex != hostIndex)
            {
                Team2 = "Blu";
                TF2Plugin.Instance.KillAfterSecondsNotIEnumerator(joinerMapper, 6.5f);
            }
            //0 rock
            //1 paper
            //2 scissors
            int prop1 = joinerMapper.props.Count;
            switch (spot % 3)
            {
                case 0:
                    joinerMapper.props.Add(GameObject.Instantiate(Assets.Load<GameObject>($"@BadAssEmotes_badassemotes:Assets/RPS/{Team2}_Rock_Lose.prefab")));
                    break;
                case 1:
                    joinerMapper.props.Add(GameObject.Instantiate(Assets.Load<GameObject>($"@BadAssEmotes_badassemotes:Assets/RPS/{Team2}_Paper_Lose.prefab")));
                    break;
                case 2:
                    joinerMapper.props.Add(GameObject.Instantiate(Assets.Load<GameObject>($"@BadAssEmotes_badassemotes:Assets/RPS/{Team2}_Scissors_Lose.prefab")));
                    break;
                default:
                    break;
            }
            joinerMapper.props[prop1].transform.SetParent(joinerMapper.parentGameObject.transform);
            joinerMapper.props[prop1].transform.localEulerAngles = Vector3.zero;
            joinerMapper.props[prop1].transform.localPosition = new Vector3(0, 2.5f * joinerMapper.props[prop1].transform.lossyScale.y, 0);
            joinerMapper.ScaleProps();
        }
    }
}

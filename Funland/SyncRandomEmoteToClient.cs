using R2API.Networking.Interfaces;
using RoR2;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

namespace TitanFall2Emotes
{
    class SyncRandomEmoteToClient : INetMessage
    {
        NetworkInstanceId netId;
        string name;
        int spot;
        NetworkInstanceId secondaryNetId;
        public SyncRandomEmoteToClient()
        {

        }

        public SyncRandomEmoteToClient(NetworkInstanceId netId, string name, int spot, NetworkInstanceId secondaryNetId)
        {
            this.netId = netId;
            this.name = name;
            this.spot = spot;
            this.secondaryNetId = secondaryNetId;
        }

        public void Deserialize(NetworkReader reader)
        {
            netId = reader.ReadNetworkId();
            name = reader.ReadString();
            spot = reader.ReadInt32();
            secondaryNetId = reader.ReadNetworkId();
        }

        public void OnReceived()
        {
            GameObject bodyObject = Util.FindNetworkObject(netId);
            BoneMapper joinerMapper = bodyObject.GetComponent<ModelLocator>().modelTransform.GetComponentInChildren<BoneMapper>();
            GameObject hostBodyObject = Util.FindNetworkObject(secondaryNetId);
            BoneMapper hostJoinerMapper = hostBodyObject.GetComponent<ModelLocator>().modelTransform.GetComponentInChildren<BoneMapper>();
            GameObject g;
            switch (name)
            {
                case "RPS_Start":
                    joinerMapper.PlayAnim(TF2Plugin.RPS_Start_Emotes[spot], 0);
                    joinerMapper.props.Add(new GameObject());
                    joinerMapper.props[0].AddComponent<RockPaperScissors>().charType = spot;

                    g = new GameObject();
                    g.name = "RPS_StartProp";
                    joinerMapper.props.Add(g);
                    g.transform.localPosition = joinerMapper.transform.position;
                    g.transform.localEulerAngles = joinerMapper.transform.eulerAngles;
                    g.transform.localScale = Vector3.one;
                    joinerMapper.AssignParentGameObject(g, true, true, true, false, false);
                    break;
                case "RPS_Win":
                    joinerMapper.PlayAnim(TF2Plugin.RPS_Win_Emotes[spot], 0);

                    g = new GameObject();
                    g.name = "RPS_WinProp";
                    joinerMapper.props.Add(g);
                    if (hostJoinerMapper != joinerMapper)
                    {
                        g.transform.SetParent(hostJoinerMapper.transform);
                        g.transform.localPosition = new Vector3(0, 0, 2.1f);
                        g.transform.localEulerAngles = new Vector3(0, 180, 0);
                        g.transform.localScale = Vector3.one;
                        joinerMapper.AssignParentGameObject(g, true, true, true, false, true);
                    }
                    else
                    {
                        g.transform.localPosition = joinerMapper.transform.position;
                        g.transform.localEulerAngles = joinerMapper.transform.eulerAngles;
                        g.transform.localScale = Vector3.one;
                        joinerMapper.AssignParentGameObject(g, true, true, true, false, false);
                    }
                    break;
                case "RPS_Loss":
                    joinerMapper.PlayAnim(TF2Plugin.RPS_Loss_Emotes[spot], 0);

                    g = new GameObject();
                    g.name = "RPS_LossProp";
                    joinerMapper.props.Add(g);
                    if (hostJoinerMapper != joinerMapper)
                    {
                        g.transform.SetParent(hostJoinerMapper.transform);
                        g.transform.localPosition = new Vector3(0, 0, 2.1f);
                        g.transform.localEulerAngles = new Vector3(0, 180, 0);
                        g.transform.localScale = Vector3.one;
                        joinerMapper.AssignParentGameObject(g, true, true, true, false, true);
                    }
                    else
                    {
                        g.transform.localPosition = joinerMapper.transform.position;
                        g.transform.localEulerAngles = joinerMapper.transform.eulerAngles;
                        g.transform.localScale = Vector3.one;
                        joinerMapper.AssignParentGameObject(g, true, true, true, false, false);
                    }
                    break;
                default:
                    break;
            }
        }

        public void Serialize(NetworkWriter writer)
        {
            writer.Write(netId);
            writer.Write(name);
            writer.Write(spot);
            writer.Write(secondaryNetId);
        }
    }
}

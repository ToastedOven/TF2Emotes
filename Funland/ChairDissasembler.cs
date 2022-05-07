using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace TitanFall2Emotes
{
    class ChairDissasembler : MonoBehaviour
    {
        internal GameObject chair;
        bool check = true;
        internal Vector3 pos;
        internal Vector3 rot;
        internal Vector3 scal;
        bool check2 = false;
        bool check3 = false;
        
        void Start()
        {
            foreach (var item in GetComponentInChildren<SkinnedMeshRenderer>().materials)
            {
                item.shader = TF2Plugin.defaultShader;
                item.shaderKeywords = new string[] { "DITHER" };
            }
        }
        void Update()
        {
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

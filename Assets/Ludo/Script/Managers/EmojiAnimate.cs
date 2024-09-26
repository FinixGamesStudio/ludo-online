using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Ludo
{
    public class EmojiAnimate : MonoBehaviour
    {
        [SerializeField] private string id;

        [SerializeField] GameObject emojiPrefab;

        [SerializeField] private List<RuntimeAnimatorController> emojiAnimatiorList;

        RectTransform cloneEmojiParent = null;

        public void EmojiAnimation(int number, string id)
        {
            this.gameObject.SetActive(true);
            GameObject emojiClone;
            // var cloneEmojiParent = id == LudoGameManager.instace.ludoNumbersAcknowledgementHandler.myUserId ? LudoGameManager.instace.emojiParent : LudoGameManager.instace.emojiParentOppo;
            if (id == LudoGameManager.instace.signUpAcknowledgement.userId)
            {
                //cloneEmojiParent = LudoGameManager.instace.emojiParent;
                Debug.Log("cloneEmojiParent => " + cloneEmojiParent);
            }
            else
            {
                for (int i = 0; i < LudoGameManager.instace.allPlayerHomeController.Count; i++)
                {
                    Debug.Log("LudoGameManager.instace.ludoNumbersAcknowledgementHandler.ludoNumberPlayerControl[i].playerInfoData.userId  => " + LudoGameManager.instace.allPlayerHomeController[i].playerInfoData.userId);
                    if (LudoGameManager.instace.allPlayerHomeController[i].playerInfoData.userId == id)
                    {
                        cloneEmojiParent = LudoGameManager.instace.allPlayerHomeController[i].emojiTransform;
                        Debug.Log("cloneEmojiParent 2 => " + cloneEmojiParent);
                    }
                }
            }
            emojiClone = Instantiate(emojiPrefab, cloneEmojiParent);
            emojiClone.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
            Animator anim = emojiClone.GetComponent<Animator>();
            anim.runtimeAnimatorController = emojiAnimatiorList[number];
            SoundManager.instance.EmojiSoundPlay(SoundManager.instance.emojiSoundClip, number);
            Destroy(emojiClone, 2.2f);
        }
    }
}
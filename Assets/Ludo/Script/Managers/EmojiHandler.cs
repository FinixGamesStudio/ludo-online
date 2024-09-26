using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

namespace Ludo
{
    public class EmojiHandler : MonoBehaviour
    {
        [SerializeField] public string senderId;
        [SerializeField] public string tabelId;

        public void InsidePopUpEmojiClick(int emojiNo)
        {
            //LudoGameManager.instace.socketConnection.SendDataToSocket(LudoGameManager.instace.socketEventManager.EmojiRequestData(emojiNo), EmojiAcknowledgement, "EMOJI");
        }
        private void EmojiAcknowledgement(string expectAcknowledgement) => Debug.Log("EmojiAcknowledgement || expectAcknowledgement  " + expectAcknowledgement);
    }
}
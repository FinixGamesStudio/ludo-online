using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Newtonsoft.Json;
using UnityEngine;
using static Ludo.EmojiRequestResponseClass;

namespace Ludo
{
    public class LudoEmojiController : MonoBehaviour
    {
        public LudoGameManager gameManager;

        [SerializeField] private List<LudoEmojiUiController> allEmojies;

        public RectTransform root;
        public RectTransform startPosition;
        public RectTransform targetPosition;

        public EmojiEvent emojiResponse;

        public int selectToSendIndex;

        public void OpenEmojiScreen(int selectToSend)
        {
            selectToSendIndex = selectToSend;
            gameObject.SetActive(true);
            for (int i = 0; i < allEmojies.Count; i++)
                allEmojies[i].indexOfEmoji = i;
            root.transform.position = startPosition.position;
            root.DOMove(targetPosition.position, 0.5f);
        }

        public void CloseEmojiScreen()
        {
            root.DOMove(startPosition.position, 0.5f).OnComplete(() =>
            {
                gameObject.SetActive(false);
            });
        }

        public void OnEmojiResponse(string response)
        {
            root.transform.position = startPosition.position;
            emojiResponse = JsonConvert.DeserializeObject<EmojiEvent>(response);
            EmojiAnimation(emojiResponse.data.fromToSendSeatIndex, emojiResponse.data.toSendSeatIndex, emojiResponse.data.indexOfEmoji);
        }

        public void EmojiAnimation(int from, int to, int emojiIndex)
        {
            this.gameObject.SetActive(true);
            root.transform.position = targetPosition.position;
            LudoHomeController fromHomeController = gameManager.allPlayerHomeController.Find(player => player.playerInfoData.seatIndex == from);
            LudoHomeController toHomeController = gameManager.allPlayerHomeController.Find(player => player.playerInfoData.seatIndex == to);

            LudoEmojiUiController emojiUiController = Instantiate(allEmojies[emojiIndex], transform);
            emojiUiController.AnimationOnUserProfile(fromHomeController.emojiTransform, toHomeController.emojiTransform);
            //SoundManager.instance.EmojiSoundPlay(SoundManager.instance.emojiSoundClip, emojiIndex);
            this.gameObject.SetActive(false);
        }

        public void ClickedOnEmojiIndex(int emojiNo) => gameManager.socketConnection.SendDataToSocket(gameManager.socketEventManager.EmojiRequestData(emojiNo, selectToSendIndex), EmojiAcknowledgement, "EMOJI");
        private void EmojiAcknowledgement(string expectAcknowledgement) => Debug.Log("EmojiAcknowledgement || expectAcknowledgement  " + expectAcknowledgement);

    }
}

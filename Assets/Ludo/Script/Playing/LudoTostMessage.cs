using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms;
using UnityEngine.UI;

namespace Ludo
{
    public enum ToastMessage
    {
        STARTGAME,
        GAMECOUNTDOWN,
        EXTRAMOVE,
        PLUS56,
        EXTRAPLUS5,
        WAITFORPLAYER,
        LASTMOVE,
        FINISHGAME,
        GAMEWILLEND,
        TIMEOUT
    }

    public class LudoTostMessage : MonoBehaviour
    {

        [Header("Toast Message Bottom Object")]
        public Transform toastMessageBottomObject;
        public TMPro.TextMeshProUGUI toastMessageBottomText;
        public string gameWillEnd30Seconds = "GAME WILL END IN 30 SECONDS";
        public string timeOver = "TIME OVER.PLAYERS WILL PLAY THEIR LAST TURN";

        [Header("Toast Message Object")]
        public TMPro.TextMeshProUGUI toastMessageText;
        public Transform toastMessageObject;
        public string waitingForPlayerMessage = "WAITING FOR PLAYERS";
        public string point56 = "+56 POINTS";
        public string extraMove = "EXTRA MOVE";
        public string lastMove = "LAST MOVE";
        public string startGame = "START";
        public string extraPlus5 = "+5 POINTS";


        public void ShowToastMessages(ToastMessage toastMessage, bool isClose)
        {
            toastMessageObject.gameObject.SetActive(true);
            switch (toastMessage)
            {
                case ToastMessage.WAITFORPLAYER:
                    toastMessageText.text = waitingForPlayerMessage;
                    break;
                case ToastMessage.GAMECOUNTDOWN:
                    break;
                case ToastMessage.STARTGAME:
                    toastMessageText.text = startGame;
                    break;
                case ToastMessage.EXTRAMOVE:
                    toastMessageText.text = extraMove;
                    break;
                case ToastMessage.PLUS56:
                    toastMessageText.text = point56;
                    break;
                case ToastMessage.EXTRAPLUS5:
                    toastMessageText.text = extraPlus5;
                    break;
                case ToastMessage.LASTMOVE:
                    toastMessageText.text = lastMove;
                    break;
                case ToastMessage.FINISHGAME:

                    break;
                default:
                    break;
            }

            float animationSpeed = 1f;
            if (toastMessage == ToastMessage.GAMECOUNTDOWN)
                animationSpeed = 0;

            toastMessageObject.DOMoveX(0f, animationSpeed).SetEase(Ease.Linear).OnComplete(() =>
            {
                if (isClose)
                    CloseToastMessage();
            });
        }
        public void UpdateMessageText(string toastMessage) => toastMessageText.text = toastMessage;

        public void CloseToastMessage() => toastMessageObject.DOMoveX(-1200f, 1f).SetEase(Ease.Linear);

        public void ShowBottomToast(ToastMessage toastMessage, bool isClose)
        {
            switch (toastMessage)
            {
                case ToastMessage.GAMEWILLEND:
                    toastMessageBottomText.text = gameWillEnd30Seconds;
                    break;
                case ToastMessage.TIMEOUT:
                    toastMessageBottomText.text = timeOver;
                    break;
                default:
                    break;
            }
            toastMessageBottomObject.DOScale(Vector3.one, 1f).SetEase(Ease.Linear).OnComplete(() =>
            {
                if (isClose)
                    CloseBottomToastMessage();
            });
        }
        public void CloseBottomToastMessage() => toastMessageBottomObject.DOScale(Vector3.zero, 1f).SetEase(Ease.Linear);


        internal void ShowExtraScoreAnim(string userName)
        {
            toastMessageObject.gameObject.SetActive(true);
            toastMessageObject.transform.localScale = Vector3.zero;
            toastMessageText.text = userName + " GETS +5 POINTS";

            toastMessageObject.transform.DOScale(1f, 0.75f).SetEase(Ease.OutBack).OnComplete(() =>
            {
                toastMessageObject.transform.DOScale(0, 0.3f).SetEase(Ease.InBack).SetDelay(4f).OnComplete(() =>
                {
                    toastMessageObject.gameObject.SetActive(false);
                });
            });
        }

        public GameObject tokenHome;

        internal void ShowTokenHomeAnim()
        {
            tokenHome.SetActive(true);
            tokenHome.transform.localScale = Vector3.zero;
            tokenHome.GetComponent<Image>().DOFade(1, 0f);
            tokenHome.transform.DOScale(1f, 0.75f).SetEase(Ease.OutBack).OnComplete(() =>
            {
                tokenHome.GetComponent<Image>().DOFade(0f, 0.75f).SetEase(Ease.OutBack).SetDelay(4f).OnComplete(() =>
                {
                    tokenHome.gameObject.SetActive(false);
                });
            });
        }

    }
}

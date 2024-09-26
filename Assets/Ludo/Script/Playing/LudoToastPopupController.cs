using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace Ludo
{
    public class LudoToastPopupController : MonoBehaviour
    {
        [Space(5)]
        [Header("TOAST CENTER POPUP")]
        public RectTransform toastCenterPopup;
        public RectTransform toastCenterPopupStart;
        public RectTransform toastCenterPopupFianl;

        public TMPro.TextMeshProUGUI toastCenterMessageText;

        [Space(5)]
        [Header("TOAST TOP POPUP")]
        public RectTransform toastTopPopup;
        public RectTransform toastTopPopupStart;
        public RectTransform toastTopPopupFianl;

        public TMPro.TextMeshProUGUI toastTopMessageText;

        [Space(5)]
        [Header("COLOR")]
        public Color redColor;
        public Color greenColor;

        [Header("SERVER POPUP")]
        public RectTransform popup;
        public TMPro.TextMeshProUGUI headerText;
        public TMPro.TextMeshProUGUI descriptionText;
        public Button yesButton;
        public Button noButton;

        public void ShowCenterToastMessage(string message, bool isError, bool isClose)
        {
            try
            {
                Debug.Log("<color><b>ShowToastMessage || </b></color>" + message);
                toastCenterPopup.DOKill();
                CancelInvoke(nameof(CloseToast));

                if (isError)
                    DoARedColorToCenterText();
                else
                    DoAGreenColorToCenterText();

                toastCenterMessageText.text = message;

                toastCenterPopup.position = toastCenterPopupStart.position;
                gameObject.SetActive(true);
                toastCenterPopup.DOMove(toastCenterPopupFianl.position, 0.5f).SetEase(Ease.Linear).OnComplete(() =>
                {
                    Debug.Log("<color> Animation On Complete </color>");
                    if (isClose)
                        Invoke(nameof(CloseCenterToastMessage), 0.5f);
                });
            }
            catch (System.Exception ex)
            {
                Debug.Log("<color=red> Exception || </color>" + ex.ToString());
            }
        }

        public void CloseCenterToastMessage()
        {
            toastCenterPopup.DOMove(toastCenterPopupStart.position, 0.5f).SetEase(Ease.Linear).OnComplete(() =>
            {
                Debug.Log("<color> Animation On Complete </color>");
                //Invoke(nameof(CloseToast), 2f);
                CloseToast();
            });
        }

        public void UpdateCenterToastMessage(string message) => toastCenterMessageText.text = message;

        private void DoARedColorToCenterText() => toastCenterMessageText.color = redColor;
        private void DoAGreenColorToCenterText() => toastCenterMessageText.color = greenColor;

        //TOP TOAST 
        public void ShowTopToastMessage(string message, bool isError, bool isClose)
        {
            try
            {
                Debug.Log("<color><b>ShowToastMessage || </b></color>" + message);
                toastTopPopup.DOKill();
                CancelInvoke(nameof(CloseToast));

                if (isError)
                    DoARedColorToTopText();
                else
                    DoAGreenColorToTopText();

                toastTopMessageText.text = message;

                toastTopPopup.position = toastTopPopupStart.position;
                toastTopPopup.gameObject.SetActive(true);
                gameObject.SetActive(true);
                toastTopPopup.DOMove(toastTopPopupFianl.position, 0.5f).SetEase(Ease.Linear).OnComplete(() =>
                {
                    //Debug.Log("<color> Animation On Complete </color>");
                    if (isClose)
                        Invoke(nameof(CloseTopToastMessage), 0.5f);
                });
            }
            catch (System.Exception ex)
            {
                Debug.Log("<color=red> Exception || </color>" + ex.ToString());
            }
        }

        public void CloseTopToastMessage()
        {
            toastTopPopup.DOMove(toastTopPopupStart.position, 0.5f).SetEase(Ease.Linear).OnComplete(() =>
            {
                //Debug.Log("<color> Animation On Complete </color>");
                //Invoke(nameof(CloseToast), 2f);
                toastTopPopup.gameObject.SetActive(false);
                gameObject.SetActive(false);
                CloseToast();
            });
        }

        public void UpdateTopToastMessage(string message) => toastTopMessageText.text = message;

        private void DoARedColorToTopText() => toastTopMessageText.color = redColor;
        private void DoAGreenColorToTopText() => toastTopMessageText.color = greenColor;

        void CloseToast() => gameObject.SetActive(false);


        public void OpenServerErrorPopUp(int buttonCount, string header, string description)
        {
            gameObject.SetActive(true);
            yesButton.gameObject.SetActive(false);
            noButton.gameObject.SetActive(false);

            yesButton.onClick.RemoveAllListeners();
            noButton.onClick.RemoveAllListeners();
            if (buttonCount == 0)
            {
                Invoke(nameof(CloseServerPopUp), 1f);
            }
            else if (buttonCount == 1)
            {
                yesButton.gameObject.SetActive(true);
                noButton.gameObject.SetActive(false);
                yesButton.onClick.AddListener(CloseServerPopUp);
            }
            else if (buttonCount == 2)
            {
                yesButton.gameObject.SetActive(true);
                noButton.gameObject.SetActive(true);
                yesButton.onClick.AddListener(CloseServerPopUp);
                noButton.onClick.AddListener(CloseServerPopUp);
            }

            headerText.text = header;
            descriptionText.text = description;


            popup.DOScale(Vector3.one, 0.2f).SetEase(Ease.InOutQuad);
        }


        public void CloseServerPopUp()
        {
            popup.DOScale(Vector3.zero, 0.2f).SetEase(Ease.InOutQuad);
        }

        public void IAmBackPopUp()
        {

        }
    }
}

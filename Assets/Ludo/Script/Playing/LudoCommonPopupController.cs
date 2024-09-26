using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;
using Newtonsoft.Json;
using static CommonServerPopupClass;

namespace Ludo
{
    public class LudoCommonPopupController : MonoBehaviour
    {
        public LudoGameManager gameManager;

        #region VARIABLES

        [Header("==========================================")]
        [SerializeField] private Button[] serverCommonPopupButtons;

        [SerializeField] private Text[] serverCommonPopupButtonsText;

        //public GameObject serverTopToast;
        //public GameObject serverCenterToast;
        public GameObject serverCommonPopup;
        public GameObject serverCommonPopupContent;
        public GameObject serverCommonPopupLoader;

        //public Text serverTopToastText;
        //public Text serverCenterToastText;
        public TextMeshProUGUI serverCommonPopupTitle;
        public TextMeshProUGUI serverCommonPopupMessage;

        [Header("==========================================")]

        [Header("Sprite")]
        [SerializeField]
        private Sprite greenBtn;
        [SerializeField] private Sprite RedBtn;
        public Sprite diceValue;

        #endregion

        #region SHOWPOPUP

        public void ShowPopUp(string responseJsonString)
        {
            CommonServerPopupResponse commonServerPopup = JsonConvert.DeserializeObject<CommonServerPopupResponse>(responseJsonString);

            string popupType = commonServerPopup.data.popupType;
            switch (popupType)
            {
                case "topToastPopup":
                    //ShowServer_TopToast(commonServerPopup.data);
                    break;
                case "toastPopup":
                    //ShowServer_CenterToast(commonServerPopup.data);
                    break;
                case "commonPopup":
                    ShowServer_CommonPopup(commonServerPopup.data);
                    break;
                default:
                    break;
            }
        }

        //private void ShowServer_TopToast(CommonServerPopupResponseData popupData, bool isAutoHide = true)
        //{
        //    serverTopToastText.text = popupData.message;
        //    RectTransform rect = serverTopToast.GetComponent<RectTransform>();
        //    rect.localScale = Vector2.one;
        //    serverTopToast.transform.GetComponent<Canvas>().enabled = true;
        //    if (isAutoHide)
        //    {
        //        rect.DOScale(1f, 0.3f).SetEase(Ease.OutBack).OnComplete(() => { this.WaitforTime(6.5f, () => { HideServer_TopToast(0.3f); }); });
        //    }
        //    else
        //        rect.DOScale(1f, 0.3f).SetEase(Ease.OutBack);
        //}

        //public void HideServer_TopToast(float time)
        //{
        //    RectTransform topToastRect = serverTopToast.GetComponent<RectTransform>();
        //    topToastRect.DOScale(0, time).OnComplete(() => serverTopToast.transform.GetComponent<Canvas>().enabled = false);
        //}

        //private void ShowServer_CenterToast(CommonServerPopupResponseData popupData, bool isAutoHide = true)
        //{
        //    serverCenterToastText.text = popupData.message;
        //    RectTransform rect = serverCenterToast.GetComponent<RectTransform>();
        //    HideServer_CenterToast();
        //    serverCenterToast.GetComponent<Canvas>().enabled = true;
        //    if (isAutoHide)
        //    {
        //        rect.DOScale(1, .4f).SetEase(Ease.OutElastic).OnComplete(() => { this.WaitforTime(6f, () => { HideServer_CenterToast(); }); });
        //    }
        //    else
        //        rect.DOScale(1f, .4f).SetEase(Ease.OutElastic);
        //}

        //public void HideServer_CenterToast()
        //{
        //    RectTransform toastRect = serverCenterToast.GetComponent<RectTransform>();
        //    toastRect.DOScale(0, .2f);
        //    serverCenterToast.GetComponent<Canvas>().enabled = false;
        //}

        internal void ShowServer_CommonPopup(CommonServerPopupResponseData popupData)
        {
            try
            {
                string title = popupData.title;
                string message = popupData.message;

                serverCommonPopupTitle.text = title;

                serverCommonPopupMessage.text = message;

                for (int i = 0; i < serverCommonPopupButtons.Length; i++)
                {
                    serverCommonPopupButtons[i].onClick.RemoveAllListeners();
                    serverCommonPopupButtons[i].gameObject.SetActive(false);
                }

                int buttonCount = popupData.buttonCounts;
                Debug.Log("buttonCount:" + popupData.buttonCounts);
                for (int i = 0; i < popupData.buttonCounts; i++)
                {
                    serverCommonPopupButtons[i].gameObject.SetActive(true);
                    serverCommonPopupButtonsText[i].text = popupData.button_text[i];
                }

                if (buttonCount == 1)
                {
                    serverCommonPopupButtons[0].transform.DOLocalMove(new Vector2(0, -163.6f), 0);
                    SetButtonSprite(serverCommonPopupButtons[0].transform.GetComponent<Image>(),
                        popupData.button_color == null ? "red" : popupData.button_color[0]);
                    serverCommonPopupButtons[0].onClick.AddListener(() => PopupButtonClick(popupData.button_methods[0]));
                }

                if (buttonCount == 2)
                {
                    serverCommonPopupButtons[0].transform.DOLocalMove(new Vector2(-150f, -163.6f), 0);
                    SetButtonSprite(serverCommonPopupButtons[0].transform.GetComponent<Image>(),
                        popupData.button_color == null ? "red" : popupData.button_color[0]);
                    serverCommonPopupButtons[0].onClick.AddListener(() => PopupButtonClick(popupData.button_methods[0]));
                    SetButtonSprite(serverCommonPopupButtons[1].transform.GetComponent<Image>(),
                        popupData.button_color == null ? "green" : popupData.button_color[1]);
                    serverCommonPopupButtons[1].onClick.AddListener(() => PopupButtonClick(popupData.button_methods[1]));
                }

                serverCommonPopup.GetComponent<Canvas>().enabled = true;
                RectTransform contentRect = serverCommonPopupContent.GetComponent<RectTransform>();
                contentRect.DOScale(0, 0);
                contentRect.DOScale(1f, 0.5f).SetEase(Ease.OutBack).OnComplete(() => { serverCommonPopupLoader.SetActive(popupData.isPopup); });
            }
            catch (System.Exception e)
            {
                Debug.LogError(e.ToString());
            }

            //ludoUiManager.StopReconntionAnimation();
        }

        void SetButtonSprite(Image Button, string buttonSpriteColor)
        {
            if (buttonSpriteColor == "green")
                Button.sprite = greenBtn;
            else if (buttonSpriteColor == "red")
                Button.sprite = RedBtn;
        }

        private void PopupButtonClick(string method) => Invoke(method, 0);

        #endregion
    }
}
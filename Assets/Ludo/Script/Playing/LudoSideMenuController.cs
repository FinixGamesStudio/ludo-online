using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace Ludo
{
    public class LudoSideMenuController : MonoBehaviour
    {
        [Header("LudoGameManager")]
        public LudoGameManager gameManager;

        public GameObject openSettingButton;
        public GameObject closeSettingButton;

        public Toggle sound;
        public Toggle vibration;

        public RectTransform popup;
        public Image bg;
        public RectTransform startPosition;
        public RectTransform targetPosition;

        public void OpenSideMenu()
        {
            popup.anchoredPosition = startPosition.anchoredPosition;
            popup.DOMove(targetPosition.position, 0.5f).SetEase(Ease.Linear).OnComplete(() =>
            {
                Debug.Log("<color> Animation On Complete </color>");
            });
        }

        public void CloseSideMenu()
        {
            popup.DOMove(startPosition.position, 0.5f).SetEase(Ease.Linear).OnComplete(() =>
            {
                Debug.Log("<color> Animation On Complete </color>");
                gameObject.SetActive(false);
                bg.enabled = false;
            });
        }

        public void OnButtonClicked(string buttonName)
        {
            switch (buttonName)
            {
                case "Open":
                    HideBothButton();
                    gameObject.SetActive(true);
                    bg.enabled = true;
                    closeSettingButton.SetActive(true);
                    OpenSideMenu();
                    break;
                case "Sound":
                    SetSoundOnOff();
                    break;
                case "Vibration":
                    SetVibrationOnOff();
                    break;
                case "Help":
                    OnButtonClicked("Close");
                    gameManager.uiManager.helpController.OpenHelpScreen();
                    break;
                case "GameInfo":

                    break;
                case "Close":
                    HideBothButton();
                    openSettingButton.SetActive(true);
                    CloseSideMenu();
                    break;
                case "Leave":
                    HideBothButton();
                    CloseSideMenu();
                    openSettingButton.SetActive(true);
                    gameManager.uiManager.exitPopUpController.OnButtonClicked("Open");
                    break;
                default:
                    break;
            }
        }





        public void SetVibrationOnOff()
        {
            if (vibration.isOn)
                vibration.isOn = false;
            else
                vibration.isOn = true;
            SaveTheSoundOnOff("Vibration", vibration.isOn);
        }

        public void SaveTheSoundOnOff(string saveKey, bool isSoundOn)
        {
            if (isSoundOn)
                PlayerPrefs.SetInt(saveKey, 1);
            else
                PlayerPrefs.SetInt(saveKey, 0);
        }

        public void SetSoundOnOff()
        {
            if (sound.isOn)
                sound.isOn = false;
            else
                sound.isOn = true;
            SaveTheSoundOnOff("Sound", sound.isOn);
        }

        public void HideBothButton()
        {
            openSettingButton.SetActive(false);
            closeSettingButton.SetActive(false);
        }
    }
}

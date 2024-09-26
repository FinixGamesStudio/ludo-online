using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Ludo
{
    public class DashBoardManager : MonoBehaviour
    {
        public Sprite noButtonSprite;
        public Sprite yesButtonSprite;

        public Image classicButton;
        public Image numberButton;
        public Image diceButton;

        public LudoSocketConnection socketConnection;
        public GameObject dashBordPanal;

        public void OnButtonClicked(string buttonName)
        {
            switch (buttonName)
            {
                case "CLASSIC":
                    ResetButton();
                    classicButton.sprite = yesButtonSprite;
                    break;
                case "NUMBER":
                    ResetButton();
                    numberButton.sprite = yesButtonSprite;
                    break;
                case "DICE":
                    ResetButton();
                    diceButton.sprite = yesButtonSprite;
                    break;
                default:
                    break;
            }
            LudoGameManager.instace.signUpRequestSDKData.gameModeName = buttonName;
            LudoGameManager.instace.signUpRequestSDKData.gameType = buttonName;
        }

        public void ResetButton()
        {
            classicButton.sprite = noButtonSprite;
            numberButton.sprite = noButtonSprite;
            diceButton.sprite = noButtonSprite;
        }

        public void ClickOnPLayButton()
        {
            dashBordPanal.SetActive(false);
            //socketConnection.LudoSocketConnectionStart(socketConnection.socketUrl);// Call socket 
        }
    }
}
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

namespace Ludo
{
    public class LudoExitPopUpController : MonoBehaviour
    {
        public LudoGameManager gameManager;

        public TMPro.TextMeshProUGUI exitMessageText;

        public Transform root;
        public void OpneExitPopUp(string exitMessage)
        {
            exitMessageText.text = exitMessage;
            root.localScale = Vector3.zero;
            gameObject.SetActive(true);
            root.DOScale(Vector3.one, 0.25f).OnComplete(() =>
            {
            });
        }

        public void CloseExitPopUp()
        {
            root.DOScale(Vector3.zero, 0.25f).OnComplete(() =>
            {
                exitMessageText.text = string.Empty;
                gameObject.SetActive(false);
            });
        }

        public void OnButtonClicked(string buttonName)
        {
            switch (buttonName)
            {
                case "Open":
                    OpneExitPopUp("Are you sure,\nyou want to leave?\nYou will lose the game.");
                    break;
                case "Yes":
                    gameManager.socketConnection.SendDataToSocket(gameManager.socketEventManager.SendLeaveTable(), LeaveTableAcknowledgement, LudoNumberEventList.LEAVE_TABLE.ToString());
                    gameManager.uiManager.reconnectionController.ShowAndHideAnimation(true);
                    gameManager.uiManager.reconnectionController.ShowAndHideObject(true);
                    break;
                case "No":
                    CloseExitPopUp();
                    break;

                default:
                    break;
            }
        }
        public void LeaveTableAcknowledgement(string acknowledgement) => Debug.Log($"");
    }
}

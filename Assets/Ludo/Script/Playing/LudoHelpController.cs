using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine;

namespace Ludo
{
    public class LudoHelpController : MonoBehaviour
    {
        public Transform root;
        public void OpenHelpScreen()
        {
            OnButtonClicked("PointsSystem");
            root.localScale = Vector3.zero;
            root.DOScale(Vector3.one, 0.5f);
            gameObject.SetActive(true);
        }

        public void CloseHelpScren()
        {
            root.DOScale(Vector3.zero, 0.5f).OnComplete(() =>
            {
                gameObject.SetActive(false);
            });
        }
        public List<GameObject> allSmallHelpPopUp;

        public void OnButtonClicked(string buttonName)
        {
            foreach (var item in allSmallHelpPopUp)
                item.SetActive(false);

            switch (buttonName)
            {
                case "PointsSystem":
                    allSmallHelpPopUp[0].SetActive(true);
                    break;
                case "ExtraTime":
                    allSmallHelpPopUp[1].SetActive(true);
                    break;
                case "Extraturns":
                    allSmallHelpPopUp[2].SetActive(true);
                    break;
                case "Safezones":
                    allSmallHelpPopUp[3].SetActive(true);
                    break;
                default:
                    break;
            }
        }
    }
}

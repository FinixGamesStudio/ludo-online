using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

namespace Ludo
{
    public class LudoNoInternetController : MonoBehaviour
    {
        public Transform root;

        public TMPro.TextMeshProUGUI noInternetText;

        void NoInternetConnection(string message) => noInternetText.text = message;

        public void HideNoInternetConnection()
        {
            root.DOScale(Vector3.zero, 0.5f).OnComplete(() => { gameObject.SetActive(false); });
        }

        public void ShowNoInternetConnection(string message)
        {
            NoInternetConnection(message);
            gameObject.SetActive(true);
            root.transform.localScale = Vector3.zero;
            root.DOScale(Vector3.one, 0.5f).OnStart(() => { });
        }

    }
}

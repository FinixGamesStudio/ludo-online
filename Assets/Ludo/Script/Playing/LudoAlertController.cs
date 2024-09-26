using DG.Tweening;
using Newtonsoft.Json;
using UnityEngine;
using static Ludo.AlertPopupClass;

namespace Ludo
{
    public class LudoAlertController : MonoBehaviour
    {
        public Transform root;
        public TMPro.TextMeshProUGUI alertText;

        public LudoGameManager gameManager;

        public void OnResponseAlert(string response)
        {
            gameObject.SetActive(true);
            AlertPopupResponse alertPopupResponse = JsonConvert.DeserializeObject<AlertPopupResponse>(response);
            alertText.text = string.Empty;

            root.DOScale(Vector3.one, 0.25f).OnComplete(() =>
            {
                alertText.text = alertPopupResponse.data.message;
            });
        }

        public void QuitButton() => gameManager.OnClickExit();
    }
}

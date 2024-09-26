using UnityEngine;

namespace Ludo
{
    public class LudoReconnectionController : MonoBehaviour
    {
        public Animator animator;
        public TMPro.TextMeshProUGUI reconnectionText;
        public void UpdateText(string message) => reconnectionText.text = message;
        public void ShowAndHideAnimation(bool isActive) => animator.enabled = isActive;
        public void ShowAndHideObject(bool isActive) => gameObject.SetActive(isActive);
    }
}
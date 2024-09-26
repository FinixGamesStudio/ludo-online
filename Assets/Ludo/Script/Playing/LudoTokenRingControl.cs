using UnityEngine;

namespace Ludo
{
    public class LudoTokenRingControl : MonoBehaviour
    {
        public void Update() => gameObject.transform.Rotate(0f, 0f, -7f);
    }

}
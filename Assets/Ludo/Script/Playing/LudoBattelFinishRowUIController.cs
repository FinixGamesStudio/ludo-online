using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

namespace Ludo
{
    public class LudoBattelFinishRowUIController : MonoBehaviour
    {
        public Image profilePic;
        public Text userName, score, winAmount;
        public GameObject crown;
        public GameObject boxImage;

        internal void SetUserProfile(string url)
        {
            LudoGameManager.instace.uiManager.SpriteLoder(profilePic, url);
        }

    }
}
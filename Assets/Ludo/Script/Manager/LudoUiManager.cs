using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Networking;

namespace Ludo
{
    public class LudoUiManager : MonoBehaviour
    {

        [Header("LudoGameManager")]
        public LudoGameManager gameManager;

        [Header("LudoSocketConnection")]
        public LudoSocketConnection socketConnection;

        [Header("LudoReconnectionController")]
        public LudoReconnectionController reconnectionController;

        [Header("LudoSocketEvnetReceiver")]
        public LudoSocketEvnetReceiver socketEvnetReceiver;

        [Header("LudoCommonPopupController")]
        public LudoCommonPopupController commonPopupController;

        [Header("LudoBattelFinishCotroller")]
        public LudoBattelFinishController battelFinishCotroller;

        [Header("LudoBattelFinishCotroller")]
        public LudoTimerController timerController;

        [Header("LudoAlertController")]
        public LudoAlertController alertController;

        [Header("LudoAlertController")]
        public LudoNumberViewController numberViewController;

        [Header("LudoNoInternetController")]
        public LudoNoInternetController noInternetController;

        [Header("LudoExitPopUpController")]
        public LudoExitPopUpController exitPopUpController;

        [Header("LudoHelpController")]
        public LudoHelpController helpController;

        [Header("LudoSideMenuController")]
        public LudoSideMenuController sideMenuController;

        [Header("LudoToastPopupController")]
        public LudoToastPopupController toastPopupController;

        [Header("LudoEmojiController")]
        public LudoEmojiController emojiController;


        [Header("=================================")]
        public List<string> tutorialMessagesOfNumberMode;
        public List<string> tutorialMessagesOfDiceMode;
        [Header("=================================")]

        public FTUEManager FTUEmanager;

        [Header("GameObject")]
        public GameObject FTUEReconnationPanel;


        [Header("profilePicSpriteList")]
        public List<Sprite> profilePicSpriteList;

        [Header("Coroutine")]
        public Coroutine coroutine;

        [Header("SCORE || DICE || SAMLLBOX || MOVESHOW")]
        public List<GameObject> scoreGameObjectList;
        public List<GameObject> diesGameObjectList;
        public List<GameObject> smallBoxGameObjectList;
        public List<GameObject> moveShowGameObjectList;

        private void Start() => Screen.sleepTimeout = SleepTimeout.NeverSleep;

        public void StopReconntionAnimation()
        {
            reconnectionController.ShowAndHideObject(false);
            reconnectionController.ShowAndHideAnimation(false);
            FTUEReconnationPanel.SetActive(false);
        }

        public void ReconntionAnimation()
        {
            reconnectionController.ShowAndHideObject(true);
            reconnectionController.ShowAndHideAnimation(true);
            FTUEReconnationPanel.SetActive(true);
        }

        public void SpriteLoder(Image profileImage, string url)
        {
            Sprite profilePic = gameManager.profilePicSpriteList.Find(profilePic => profilePic.name == url);
            if (profilePic != null)
                profileImage.sprite = profilePic;
            else
                StartCoroutine(GetTexture(profileImage, url));
        }
        IEnumerator GetTexture(Image profile, string imageUrl)
        {
            UnityWebRequest req = UnityWebRequestTexture.GetTexture(imageUrl);
            yield return req.SendWebRequest();
            Texture2D tex = DownloadHandlerTexture.GetContent(req);
            Sprite mySprite = Sprite.Create(tex, new Rect(0.0f, 0.0f, tex.width, tex.height), new Vector2(0.5f, 0.5f), 100.0f);
            profile.sprite = mySprite;
            mySprite.name = imageUrl;
            gameManager.profilePicSpriteList.Add(mySprite);
        }
    }
}
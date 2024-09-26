using DG.Tweening;
using DG.Tweening.Core.Easing;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Networking;
using UnityEngine.UI;
using static Ludo.SignUpAcknowledgementClass;

namespace Ludo
{
    public class LudoHomeController : MonoBehaviour
    {
        [Header(" || LudoGameManager || ")]
        public LudoGameManager gameManager;
        [Header(" || LudoDiceAnimationController || ")]
        public LudoDiceAnimationController diceAnimationController;

        [Header(" || staticSeatIndex || ")]
        public int staticSeatIndex;

        [Header("====Inner-Home====")]
        public GameObject innerHome;
        public GameObject tokenParent;

        [Header("====PlayerInfo====")]
        public PlayerInfoData playerInfoData;
        public List<LudoTokenController> allPlayerToken;
        public Color myColor;
        [Header("============ USERNAME ============")]
        public TextMeshProUGUI userNameText;
        public Image userImage;
        [Header("============ SCORE ============")]
        public TextMeshProUGUI scoreText;
        public GameObject scoreBox;
        [Header("============ DICE AND DICE VALUE ============")]
        public RectTransform diceAndDiceValue;
        public GameObject diceNumber;
        public TextMeshProUGUI diceNumberText;
        [Header("============ DICE ============")]
        public Button diceImage;
        [Header("============ Animator ============")]
        public GameObject playerLeftImage;
        public Animator blinkOnUserTurn;
        public Image blinkOnUserTurnImage;
        [Header("============ lives ============")]
        public List<Image> lives;
        public List<Image> livesBoxes;
        public GameObject turnSkippedBg;
        [Header("============ DiceValueSix-Parent ============")]
        public Transform sixDiceValueParent;
        [Header("============ LudoPlayersWayPoints ============")]
        public LudoPlayersWayPoints ludoNumbersPlayerHome;
        [Header("============ TURN TIMER  ============")]
        public Image turnTimerImage;
        public Image extraTimerImage;
        public float turnTimer;
        public float remainTimer;

        [Header("============ EMOJI ============")]
        public RectTransform emojiTransform;
        public Button emojiButton;
        [Header("=====================")]

        [Header("PLAYER INDEX VARIABLE")] public int playerIndex;

        public int CanvasOrder;

        public LineRenderer line;

        public List<Transform> linePoints = new List<Transform>();

        public void EmojiButtonInteractable(bool isActive) => emojiButton.interactable = isActive;

        public void GameModeSetUp()
        {
            if (playerInfoData.seatIndex == -1)
                innerHome.SetActive(false);

            turnTimerImage.fillAmount = 0;
            HideAndShowExtraTimer(false);
            scoreBox.SetActive(false);
            diceImage.gameObject.SetActive(false);
            diceNumber.SetActive(false);
            EmojiButtonInteractable(false);

            if (this.name == "1" || this.name == "2")
                diceAndDiceValue.anchoredPosition = new Vector2(100f, 100f);
            else if (this.name == "3" || this.name == "4")
                diceAndDiceValue.anchoredPosition = new Vector2(-100f, 100f);

            BlinkOnUserTurnAnimation(false);
            diceAnimationController.SixDiceValueCounterReset(sixDiceValueParent);
            diceImage.interactable = false;
            scoreText.text = "0";
            diceNumberText.text = string.Empty;

            if (gameManager.signUpRequestSDKData.gameModeName.Equals("NUMBER"))
            {
                diceNumber.SetActive(true);
                scoreBox.SetActive(true);
            }
            else if (gameManager.signUpRequestSDKData.gameModeName.Equals("DICE"))
            {
                scoreBox.SetActive(true);
                diceImage.gameObject.SetActive(true);
            }
            else if (gameManager.signUpRequestSDKData.gameModeName.Equals("CLASSIC"))
            {
                diceImage.gameObject.SetActive(true);

                if (this.name == "1" || this.name == "2")
                    diceAndDiceValue.anchoredPosition = new Vector2(100f, 0);
                else if (this.name == "3" || this.name == "4")
                    diceAndDiceValue.anchoredPosition = new Vector2(-100f, 0);
            }
        }

        public void TurnMissedCouter(int countOfMissedTurn)
        {
            for (int i = 0; i < lives.Count; i++)
                lives[i].color = Color.green;

            for (int i = 0; i < countOfMissedTurn; i++)
                lives[i].color = Color.red;

            for (int i = 0; i < livesBoxes.Count; i++)
                livesBoxes[i].sprite = gameManager.allTurnSkippedSprite[i];

            for (int i = 0; i < countOfMissedTurn; i++)
                livesBoxes[i].sprite = gameManager.turnSkippedSprite;

            turnSkippedBg.SetActive(true);
            Invoke(nameof(HideTurnSkippedBg), 1.5f);
        }

        public void UpdateUserScore(int score) => scoreText.text = score.ToString();

        public void HideAndShowExtraTimer(bool isActive) => extraTimerImage.gameObject.SetActive(isActive);

        void HideTurnSkippedBg() => turnSkippedBg.SetActive(false);

        public void DiceAnimation() => gameManager.socketConnection.SendDataToSocket(gameManager.socketEventManager.SendDiceAnimation(), DiceAnimationStartAcknowledgement, "DICE_ANIMATION_STARTED");

        public void DiceAnimationStartAcknowledgement(string acknowledgement) => Debug.Log($" DiceAnimationStartAcknowledgement || {acknowledgement}");

        public void BlinkOnUserTurnAnimation(bool isActive)
        {
            blinkOnUserTurnImage.color = new Color(0, 0, 0, 0);
            blinkOnUserTurn.enabled = isActive;
        }

        public void UpdateUserName() => userNameText.text = playerInfoData.username;

        public void UpdateDiceValue(int diceValue) => diceNumberText.text = diceValue.ToString();

        public void UpdateUserProfile(string profileURL) => SpriteLoder(userImage, profileURL);

        public void HideAllToken()
        {
            for (int i = 0; i < allPlayerToken.Count; i++)
                allPlayerToken[i].gameObject.SetActive(false);
        }

        public void SetActiveAllToken()
        {
            for (int i = 0; i < allPlayerToken.Count; i++)
            {
                allPlayerToken[i].gameObject.SetActive(true);
                allPlayerToken[i].TokenResetAsNormal();
            }
        }

        public void Reapet(float _remainTime, float _turnTime)
        {
            turnTimer = _turnTime;
            remainTimer = _remainTime;

            turnTimerImage.color = Color.green;
            turnTimerImage.fillAmount = 1;

            CancelInvoke(nameof(TurnTimeStart));
            InvokeRepeating(nameof(TurnTimeStart), 0f, 0.02f);
            isLastTimer = false;
        }

        public void TurnDataReset()
        {
            turnTimerImage.fillAmount = 0;
            CancelInvoke(nameof(TurnTimeStart));
        }

        bool isLastTimer = false;
        public void TurnTimeStart()
        {
            turnTimerImage.fillAmount = (remainTimer / turnTimer);
            remainTimer -= 0.02f;

            if (turnTimerImage.fillAmount <= 0.3 && !isLastTimer)
            {
                isLastTimer = true;
                turnTimerImage.color = Color.red;
                if (playerInfoData.seatIndex == gameManager.signUpAcknowledgement.data.thisseatIndex)
                    SoundManager.instance.TimeSound(SoundManager.instance.timerAudio);
            }
            if (turnTimerImage.fillAmount <= 0)
            {
                turnTimerImage.fillAmount = 0;
                if (playerInfoData.seatIndex == gameManager.signUpAcknowledgement.data.thisseatIndex)
                    SoundManager.instance.TimeSoundStop(SoundManager.instance.timerAudio);
                CancelInvoke(nameof(TurnTimeStart));
            }
        }

        internal IEnumerator WinnerLineAnimation()
        {
            yield return new WaitForSeconds(1f);
            line.DOColor(new Color2(myColor, myColor), new Color2(Color.white, Color.white), 0.5f).SetLoops(8).OnComplete(() =>
            {
                line.startColor = myColor;
                line.endColor = myColor;
            });
        }

        public void TieBreakerShowLines(int tokenIndex, int lastBoxValue, bool isWinner) => StartCoroutine(DrawLine(lastBoxValue, tokenIndex, isWinner));

        IEnumerator DrawLine(int lastBoxValue, int tokenIndex, bool isWinner)
        {
            lastBoxValue += 1;
            for (var i = 0; i < lastBoxValue; i++)
            {
                line.positionCount++;
                line.SetPosition(i, linePoints[i].transform.position);
                yield return new WaitForSeconds(0.05f);
            }

            allPlayerToken[tokenIndex].ScoreView(lastBoxValue);

            if (isWinner)
                StartCoroutine(WinnerLineAnimation());
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
            if (!string.IsNullOrEmpty(imageUrl))
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

        public void OpenEmojiScreen() => gameManager.uiManager.emojiController.OpenEmojiScreen(staticSeatIndex);
    }
}

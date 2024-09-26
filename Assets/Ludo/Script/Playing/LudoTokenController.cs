using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Ludo
{
    public class LudoTokenController : MonoBehaviour, IPointerDownHandler
    {

        [Header("|| GAMEMANAGER ||")]
        public LudoGameManager gameManager;
        [Header("WAY POINTS OF TOKEN")]
        public LudoPlayersWayPoints playersWayPoints;
        public LudoHomeController homeController;
        [Header("=====================================")]
        public RectTransform tokenRectTransfrom;
        [Header("=====================================")]
        [SerializeField] internal int myLastBoxIndex;
        [SerializeField] internal int tokenIndex;
        [Header("=====================================")]
        public Canvas tokenCanvas;
        public GameObject ringObject;
        public Button tokenImage;
        public GameObject tokenScoreToolTipLeft;
        public Text tokenScoreToolTipLeftText;
        public GameObject tokenScoreToolTipRight;
        public Text tokenScoreToolTipRightText;

        public void MakeAClickAble(bool isActive) => tokenImage.image.raycastTarget = isActive;

        public void HideMyRing(bool isActive)
        {
            ringObject.SetActive(isActive);
            MakeAClickAble(isActive);
        }

        public void UpdateSortingOrder(int orderLayer) => tokenCanvas.sortingOrder = (orderLayer);

        public void OnTurnTokenHighLight()
        {
            HideMyRing(true);
            transform.localScale = new Vector3(1.1f, 1.1f, 1.1f);
            HideToolTips();
        }

        public void TokenResetAsNormal()
        {
            HideMyRing(false);
            transform.localScale = Vector3.one;
        }

        public void HideToolTips()
        {
            HideLeftTokenScore();
            HideRightTokenScore();
        }

        public void ShowRightTokenScore(int score)
        {
            tokenScoreToolTipRight.SetActive(true);
            tokenScoreToolTipRightText.text = score.ToString();
        }
        public void HideRightTokenScore() => tokenScoreToolTipRight.SetActive(false);
        public void ShowLeftTokenScore(int score)
        {
            tokenScoreToolTipLeft.SetActive(true);
            tokenScoreToolTipLeftText.text = score.ToString();
        }
        public void HideLeftTokenScore() => tokenScoreToolTipLeft.SetActive(false);

        #region ScoreView

        public void ScoreView(int score)
        {
            HideToolTips();
            var isRight = false;
            switch (homeController.gameObject.name)
            {
                case "1":
                    isRight = (score > 8 && score < 14);
                    break;
                case "2":
                    isRight = (score > 47 && score < 52 || score == 0);
                    break;
                case "3":
                    isRight = (score > 34 && score < 40);
                    break;
                case "4":
                    isRight = (score > 21 && score < 27);
                    break;
            }

            if (isRight)
                ShowRightTokenScore(score);
            else
                ShowLeftTokenScore(score);
        }


        #endregion

        [Header(" || Token Parent ||")]
        public GameObject tokenParent;
        [Header("=====================================")]

        #region VARIABLES

        [Range(0f, 1000)]
        public float jumpPower;
        [Range(0f, 1000)]
        public int jumpnumber;
        [Range(0.1f, 1f)]
        public float moveTime;
        [Range(0.1f, 1f)]
        public float movedelay;

        //public List<GameObject> playerCoockie;

        public int cookieStaticIndex;
        public Color myColor;
        public Gradient colorOverSpeed;


        internal bool isPopupShown;
        public bool isRightPopup;
        public Coroutine movememtCoroutine;

        public Renderer playerRenderer;

        #endregion

        private void Awake()
        {
            Debug.LogError($"---------- || {name} || ----------");
            myLastBoxIndex = -1;
            if (!gameManager.signUpRequestSDKData.gameModeName.Equals("CLASSIC"))
            {
                this.myLastBoxIndex++;
                transform.SetParent(playersWayPoints.wayPointsForTokenMove[myLastBoxIndex].transform.GetChild(1));
                transform.DOScale(new Vector3(1.4f, 1.4f, 1.4f), moveTime / 2).SetEase(Ease.Linear).OnComplete(() =>
                {
                    transform.DOScale(new Vector3(1f, 1f, 1f), moveTime / 2).SetEase(Ease.Linear);
                });
                tokenRectTransfrom.DOJumpAnchorPos(new Vector3(0, 30, 0), jumpPower, jumpnumber, moveTime).OnComplete(() =>
                {
                    SoundManager.instance.TimeSoundStop(SoundManager.instance.timerAudio);
                    playersWayPoints.wayPointsForTokenMove[myLastBoxIndex].GetComponent<LudoNumbersBoxProperty>().UpdateMyColor(myColor);
                    SoundManager.instance.SoundPlay(SoundManager.instance.tokenMoveAudio);
                    DoFade(playersWayPoints.wayPointsForTokenMove[myLastBoxIndex].GetComponent<LudoNumbersBoxProperty>().trail);
                    CoockieManage();
                    homeController.tokenParent.SetActive(false);
                });
            }
        }

        public void ResetTokenPosition()
        {
            transform.SetParent(tokenParent.transform, true);
            tokenRectTransfrom.anchoredPosition = new Vector3(0, 30, 0);
        }

        public void OnButtonClicked()
        {
            //Debug.Log("I AH HERE ============================");
            //if (MGPSDK.MGPGameManager.instance.sdkConfig.data.lobbyData.gameModeName.Equals("CLASSIC"))
            //{
            //    MoveToken();
            //}
            //else
            //{
            //    if (gameManager.signUpAcknowledgement.data.thisseatIndex != gameManager.signUpAcknowledgement.data.userTurnDetails.currentTurnSeatIndex)
            //        gameManager.socketConnection.SendDataToSocket(gameManager.ludoNumberEventManager.Score(tokenIndex), ludoNumbersAcknowledgementHandler.ScoreViewAcknowledgement, LudoNumberEventList.SCORE_CHECK.ToString());
            //    else
            //        gameManager.socketConnection.SendDataToSocket(gameManager.ludoNumberEventManager.MoveTokenCoockie(cookieStaticIndex), AcknowledgementMoveToken, LudoNumberEventList.MOVE_TOKEN.ToString());
            //}
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            Debug.Log("I AH HERE ============================");
            if (gameManager.signUpRequestSDKData.gameModeName.Equals("CLASSIC"))
            {
                MoveToken();
            }
            else
            {
                Debug.Log($"I AH HERE  {(gameManager.signUpAcknowledgement.data.thisseatIndex != gameManager.currentTurnSeatIndex)} || {gameManager.currentTurnSeatIndex} || {gameManager.signUpAcknowledgement.data.thisseatIndex}");

                if (gameManager.signUpAcknowledgement.data.thisseatIndex != gameManager.currentTurnSeatIndex)
                    gameManager.socketConnection.SendDataToSocket(gameManager.socketEventManager.Score(tokenIndex), AcknowledgementOfScoreView, LudoNumberEventList.SCORE_CHECK.ToString());
                else
                    gameManager.socketConnection.SendDataToSocket(gameManager.socketEventManager.MoveTokenCoockie(cookieStaticIndex), AcknowledgementOfMoveToken, LudoNumberEventList.MOVE_TOKEN.ToString());
            }
        }

        public void AcknowledgementOfMoveToken(string acknowledgement) => Debug.Log("AcknowledgementTokenMove || => " + acknowledgement);

        public void AcknowledgementOfScoreView(string acknowledgement) => Debug.Log("AcknowledgementOfScoreView || => " + acknowledgement);

        public void MoveToken() => gameManager.socketConnection.SendDataToSocket(gameManager.socketEventManager.MoveTokenCoockie(cookieStaticIndex), AcknowledgementTokenMove, LudoNumberEventList.MOVE_TOKEN.ToString());

        public void AcknowledgementTokenMove(string data) => Debug.Log("AcknowledgementTokenMove || => " + data);

        public void StopMovement()
        {
            if (movememtCoroutine != null)
            {
                transform.DOKill();
                StopCoroutine(movememtCoroutine);
            }
        }

        public void CoockieMove(int movementValue)
        {
            try
            {
                if (movememtCoroutine != null)
                    StopCoroutine(movememtCoroutine);
                movememtCoroutine = StartCoroutine(Movement(movementValue));
            }
            catch (System.Exception ex)
            {
                Debug.Log("CoockieMove || Exception  => " + ex.ToString());
                throw;
            }
        }

        IEnumerator Movement(int movementValue)
        {
            transform.localScale = new Vector3(1.1f, 1.1f, 1.1f);
            for (int i = 0; i < movementValue; i++)
            {
                this.myLastBoxIndex++;
                transform.SetParent(playersWayPoints.wayPointsForTokenMove[myLastBoxIndex].transform.GetChild(1));
                Debug.Log("Parent name => " + transform.parent.transform.parent.name);
                transform.DOScale(new Vector3(1.4f, 1.4f, 1.4f), moveTime / 2).SetEase(Ease.Linear).OnComplete(() =>
                {
                    transform.DOScale(new Vector3(1f, 1f, 1f), moveTime / 2).SetEase(Ease.Linear);
                });
                tokenRectTransfrom.DOJumpAnchorPos(new Vector3(0, 30, 0), jumpPower, jumpnumber, moveTime).OnComplete(() =>
                {
                    SoundManager.instance.TimeSoundStop(SoundManager.instance.timerAudio);
                    playersWayPoints.wayPointsForTokenMove[myLastBoxIndex].GetComponent<LudoNumbersBoxProperty>().UpdateMyColor(myColor);
                    SoundManager.instance.SoundPlay(SoundManager.instance.tokenMoveAudio);
                    DoFade(playersWayPoints.wayPointsForTokenMove[myLastBoxIndex].GetComponent<LudoNumbersBoxProperty>().trail);
                    CoockieManage();
                });
                yield return new WaitForSeconds(movedelay);
            }
            if (playersWayPoints.wayPointsForTokenMove[myLastBoxIndex].GetComponent<LudoNumbersBoxProperty>().boxType == BoxType.Star)
                SoundManager.instance.SoundPlay(SoundManager.instance.tokenEnterSafeZoneAudio);

            gameManager.CheckForIsKillOrNot();

            if (myLastBoxIndex == 56)
            {
                SoundManager.instance.SoundPlay(SoundManager.instance.tokenEnterHomeAudio);
                CoockieManage();
                MakeAClickAble(false);

                gameManager.homeParticleSystem.gameObject.SetActive(true);

                ParticleSystem.ColorBySpeedModule col = gameManager.homeParticleSystem.colorBySpeed;
                ParticleSystem.MinMaxGradient gr = col.color;
                col.color = Color.white;

                ParticleSystem.MainModule main = gameManager.homeParticleSystem.main;
                main.startColor = Color.white;

                playerRenderer.material.color = myColor;

                gameManager.homeParticleSystem.Play();

                if (!gameManager.signUpRequestSDKData.gameModeName.Equals("CLASSIC"))
                {
                }
            }

            if (gameManager.signUpRequestSDKData.gameModeName.Equals("CLASSIC"))
            {
                int sameColorCookieCount = 0;
                List<LudoTokenController> playerCoockies = new List<LudoTokenController>();

                for (int i = 0; i < playersWayPoints.wayPointsForTokenMove[myLastBoxIndex].transform.GetChild(1).childCount; i++)
                    playerCoockies.Add(playersWayPoints.wayPointsForTokenMove[myLastBoxIndex].transform.GetChild(1).GetChild(i).GetComponent<LudoTokenController>());

                var query = playerCoockies.GroupBy(x => x.myColor).Where(g => g.Count() > 1).Select(y => new { coockies = y.Key, counter = y.Count() }).ToList();
                for (int i = 0; i < query.Count; i++)
                {
                    if (query[i].counter > 1)
                        sameColorCookieCount++;
                }

                if (sameColorCookieCount >= 1)
                    SoundManager.instance.SoundPlay(SoundManager.instance.tokenEnterSafeZoneAudio);
            }

        }

        public void CoockieManage()
        {
            for (int i = 0; i < playersWayPoints.wayPointsForTokenMove.Count; i++)
                playersWayPoints.wayPointsForTokenMove[i].GetComponent<LudoNumbersBoxProperty>().CockieManage();
        }

        void DoFade(Image transform) => transform.DOFade(0f, movedelay * 10);

        public void SetMyPositionOnRejoin(int myBoxIndex)
        {
            transform.SetParent(playersWayPoints.wayPointsForTokenMove[myBoxIndex].transform.GetChild(1));
            Debug.Log("Parent name => " + transform.parent.transform.parent.name);
            transform.DOScale(new Vector3(1.4f, 1.4f, 1.4f), moveTime / 2).SetEase(Ease.Linear).OnComplete(() =>
            {
                transform.DOScale(new Vector3(1f, 1f, 1f), moveTime / 2).SetEase(Ease.Linear);
            });
            tokenRectTransfrom.DOJumpAnchorPos(new Vector3(0, 30, 0), jumpPower, jumpnumber, moveTime).OnComplete(() =>
            {
                SoundManager.instance.TimeSoundStop(SoundManager.instance.timerAudio);
                playersWayPoints.wayPointsForTokenMove[myBoxIndex].GetComponent<LudoNumbersBoxProperty>().UpdateMyColor(myColor);
                SoundManager.instance.SoundPlay(SoundManager.instance.tokenMoveAudio);
                DoFade(playersWayPoints.wayPointsForTokenMove[myBoxIndex].GetComponent<LudoNumbersBoxProperty>().trail);
                CoockieManage();
            });

            if (!gameManager.signUpRequestSDKData.gameModeName.Equals("CLASSIC"))
                homeController.tokenParent.SetActive(false);
        }

        public void KillMove()
        {
            try
            {
                StartCoroutine(KillMovement());
            }
            catch (System.Exception ex)
            {
                Debug.LogError(ex.ToString());
            }
        }
        public IEnumerator KillMovement()
        {
            int killMove = myLastBoxIndex;
            for (int i = 0; i < killMove; i++)
            {
                myLastBoxIndex--;
                transform.SetParent(playersWayPoints.wayPointsForTokenMove[myLastBoxIndex].transform.GetChild(1));
                transform.GetComponent<RectTransform>().DOJumpAnchorPos(new Vector3(0, 20, 0), jumpPower, jumpnumber, moveTime).OnComplete(() =>
                {
                    CoockieManage();
                });
                yield return new WaitForSeconds(0.009f);
            }
            if (myLastBoxIndex == 0 && gameManager.signUpRequestSDKData.gameModeName.Equals("CLASSIC"))
            {
                myLastBoxIndex = -1;
                transform.SetParent(tokenParent.transform);
                transform.GetComponent<RectTransform>().anchoredPosition = new Vector3(0, 30, 0);
            }
        }

    }
}
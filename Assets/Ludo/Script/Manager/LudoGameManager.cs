using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UnityEngine;
using UnityEngine.UI;
using static Ludo.JoinTableResponseClass;
using static Ludo.LeaveTableClass;
using static Ludo.MoveTokenResponseClass;
using static Ludo.ScoreViewResponseClass;
using static Ludo.SignUpAcknowledgementClass;
using static Ludo.SignUpClass;
using static Ludo.TieBreakerResponseClass;
using static Ludo.TurnMissedResponseClass;
using static Ludo.UserExtraTimeStartResponseClass;
using static Ludo.UserTurnStartResponseClass;

namespace Ludo
{
    public class LudoGameManager : MonoBehaviour
    {
        [Header("SING UP DATA")]
        public SignUpRequestSDKData signUpRequestSDKData;

        [Header("ServerType")]

        public ServerType serverType;
        public List<string> ALLServerURL = new List<string>();
        public List<int> ALLServerPortNo = new List<int>();

        public List<string> allServerHost = new List<string>();
        public List<int> allServerPortNo = new List<int>();

        public static LudoGameManager instace;

        [Header("SOCKET SCRIPTS")]
        public LudoSocketConnection socketConnection;
        public LudoSocketEvnetReceiver socketEvnetReceiver;
        public LudoSocketEventManager socketEventManager;

        [Header("Ludo Ui Manager")]
        public LudoUiManager uiManager;

        [Space(10)]

        [Header("SPRITE")]
        public List<Sprite> allTurnSkippedSprite;
        public Sprite turnSkippedSprite;

        [Header("NUMBER AND CLASSIC MODE")]
        public GameObject bg;
        public GameObject numberViewScreen;
        public GameObject moveLeft;
        public TMPro.TextMeshProUGUI moveText;
        public GameObject timer;
        public TMPro.TextMeshProUGUI timerText;

        [Header("homePartical")]
        public ParticleSystem homeParticleSystem;

        public List<Sprite> profilePicSpriteList;
        [Header("====================")]

        //public string selfUserID;

        //public string gameModeName;

        public List<LudoHomeController> allPlayerHomeController = new List<LudoHomeController>();

        public int currentTurnSeatIndex;

        private void Awake()
        {
            if (instace == null)
                instace = this;

            Input.multiTouchEnabled = false;

        }


        private void OnApplicationPause(bool pause)
        {
            if (pause)
                StopAllTokenMovement();
        }

        public void StopAllTokenMovement()
        {
            for (int i = 0; i < allPlayerHomeController.Count; i++)
                allPlayerHomeController[i].allPlayerToken.ForEach((cookie) => cookie.StopMovement());
        }

        public void SendSingUpRequest()
        {
            for (int j = 0; j < allPlayerHomeController.Count; j++)
                allPlayerHomeController[j].GameModeSetUp();

            socketConnection.SendDataToSocket(socketEventManager.SignUpRequstData(), OnSignUpAcknowledgement, "SIGNUP");
        }

        public void ResetLudoGamePlay()
        {
            uiManager.battelFinishCotroller.CloseBattle();
            uiManager.helpController.CloseHelpScren();
            uiManager.sideMenuController.CloseSideMenu();
            uiManager.emojiController.CloseEmojiScreen();

            for (int j = 0; j < allPlayerHomeController.Count; j++)
            {
                Debug.LogError(" || staticSeatIndex || " + allPlayerHomeController[j].staticSeatIndex);

                allPlayerHomeController[j].staticSeatIndex = -1;
                allPlayerHomeController[j].playerInfoData = new PlayerInfoData();

                allPlayerHomeController[j].HideAllToken();

                allPlayerHomeController[j].innerHome.SetActive(false);
                allPlayerHomeController[j].UpdateUserProfile("");

                allPlayerHomeController[j].UpdateUserName();
                allPlayerHomeController[j].TurnMissedCouter(0);
                allPlayerHomeController[j].turnSkippedBg.SetActive(false);

                allPlayerHomeController[j].GameModeSetUp();

                for (int k = 0; k < allPlayerHomeController[j].allPlayerToken.Count; k++)
                {
                    allPlayerHomeController[j].allPlayerToken[k].myLastBoxIndex = -1;
                    allPlayerHomeController[j].allPlayerToken[k].ResetTokenPosition();
                    allPlayerHomeController[j].UpdateUserScore(0);
                }
            }
        }

        public SignUpAcknowledgement signUpAcknowledgement;

        public Transform TokenKill;

        public void OnSignUpAcknowledgement(string acknowledgementData)
        {
            try
            {
                ResetLudoGamePlay();

                bg.SetActive(false);
                numberViewScreen.SetActive(false);
                moveLeft.SetActive(false);
                timer.SetActive(false);
                uiManager.numberViewController.gameObject.SetActive(false);

                Debug.Log("<color><b>LudoLudoGameManager || OnSignUpAcknowledgement || acknowledgementData </b></color>" + acknowledgementData);
                JObject jsonObject = JObject.Parse(acknowledgementData);
                JObject dataObject = JObject.Parse(jsonObject.GetValue("data").ToString());
                Debug.Log("<color><b>LudoLudoGameManager || OnSignUpAcknowledgement || acknowledgementData || isAbleToReconnect </b></color>" + dataObject.GetValue("isAbleToReconnect"));

                signUpAcknowledgement = JsonConvert.DeserializeObject<SignUpAcknowledgement>(acknowledgementData);

                SetPlayerSeatIndex(signUpAcknowledgement.data.thisseatIndex);

                boardManager.ChangeBordColor(signUpAcknowledgement.data.thisseatIndex);

                for (int i = 0; i < uiManager.smallBoxGameObjectList.Count; i++)
                    uiManager.smallBoxGameObjectList[i].SetActive(true);

                if (signUpRequestSDKData.gameModeName.Equals("NUMBER"))
                {
                    bg.SetActive(true);
                    numberViewScreen.SetActive(true);
                    moveLeft.SetActive(true);
                    uiManager.numberViewController.GenerateMovesTextObjectes(signUpAcknowledgement.data.playerMoves);
                }
                else if (signUpRequestSDKData.gameModeName.Equals("DICE"))
                {
                    bg.SetActive(true);
                    timer.SetActive(true);
                }
                else
                {
                    Debug.Log("Closed");
                    Debug.Log("Closed");
                }


                if (signUpAcknowledgement.data.isAbleToReconnect)
                {
                    SoundManager.instance.soundAudioSource.Stop();
                    StopAllTokenMovement();
                    switch (signUpAcknowledgement.data.tableState)
                    {
                        case "WAITING_FOR_PLAYERS":

                            break;
                        case "GAME_TIMER_STARTED":

                            break;
                        case "PLAYING":
                            ludoTostMessage.CloseToastMessage();
                            currentTurnSeatIndex = signUpAcknowledgement.data.userTurnDetails.currentTurnSeatIndex;

                            for (int i = 0; i < signUpAcknowledgement.data.playerInfo.Count; i++)
                            {
                                for (int j = 0; j < allPlayerHomeController.Count; j++)
                                {
                                    if (allPlayerHomeController[j].staticSeatIndex == signUpAcknowledgement.data.playerInfo[i].seatIndex)
                                    {
                                        Debug.LogError(" || staticSeatIndex || " + allPlayerHomeController[j].staticSeatIndex);
                                        allPlayerHomeController[j].playerInfoData = signUpAcknowledgement.data.playerInfo[i];
                                        allPlayerHomeController[j].SetActiveAllToken();
                                        allPlayerHomeController[j].innerHome.SetActive(true);
                                        allPlayerHomeController[j].UpdateUserProfile(signUpAcknowledgement.data.playerInfo[i].userProfile);
                                        allPlayerHomeController[j].UpdateUserName();
                                        //allPlayerHomeController[j].GameModeSetUp();
                                        allPlayerHomeController[j].TurnMissedCouter(signUpAcknowledgement.data.playerInfo[i].missedTurnCount);

                                        for (int k = 0; k < allPlayerHomeController[i].allPlayerToken.Count; k++)
                                        {
                                            allPlayerHomeController[j].allPlayerToken[k].myLastBoxIndex = signUpAcknowledgement.data.playerInfo[i].tokenDetails[k];

                                            if (signUpAcknowledgement.data.playerInfo[i].tokenDetails[k] != -1)
                                                allPlayerHomeController[j].allPlayerToken[k].SetMyPositionOnRejoin(allPlayerHomeController[j].allPlayerToken[k].myLastBoxIndex);

                                            allPlayerHomeController[j].UpdateUserScore(signUpAcknowledgement.data.playerInfo[i].score);
                                        }
                                    }
                                }
                            }

                            selfPlayerHomeController = allPlayerHomeController.Find(player => player.playerInfoData.seatIndex == signUpAcknowledgement.data.thisseatIndex);

                            uiManager.StopReconntionAnimation();

                            if (signUpAcknowledgement.data.numberOfPlayers == 4)
                            {
                                List<LudoHomeController> ludoHomes = new List<LudoHomeController>();

                                for (int i = 0; i < signUpAcknowledgement.data.playerInfo.Count; i++)
                                    ludoHomes = allPlayerHomeController.FindAll(item => (item.playerInfoData.seatIndex != signUpAcknowledgement.data.playerInfo[i].seatIndex));

                                for (int i = 0; i < allPlayerHomeController.Count; i++)
                                {
                                    allPlayerHomeController[i].playerLeftImage.SetActive(true);
                                    allPlayerHomeController[i].gameObject.SetActive(false);

                                    allPlayerHomeController[i].allPlayerToken.ForEach((cookie) => cookie.transform.SetParent(TokenKill.transform));
                                    allPlayerHomeController[i].allPlayerToken.ForEach((cookie) => cookie.gameObject.SetActive(false));

                                    allPlayerHomeController[i].scoreBox.SetActive(false);
                                }
                            }

                            for (int i = 0; i < allPlayerHomeController.Count; i++)
                            {
                                allPlayerHomeController[i].TurnDataReset();
                                allPlayerHomeController[i].BlinkOnUserTurnAnimation(false);
                                allPlayerHomeController[i].HideAndShowExtraTimer(false);
                                allPlayerHomeController[i].diceNumberText.text = string.Empty;
                            }

                            for (int i = 0; i < allPlayerHomeController.Count; i++)
                                for (int j = 0; j < allPlayerHomeController[i].allPlayerToken.Count; j++)
                                {
                                    allPlayerHomeController[i].allPlayerToken[j].HideMyRing(false);
                                    allPlayerHomeController[i].allPlayerToken[j].HideToolTips();
                                }

                            for (int i = 0; i < allPlayerHomeController.Count; i++)
                            {
                                if (allPlayerHomeController[i].playerInfoData.seatIndex == currentTurnSeatIndex)
                                {
                                    allPlayerHomeController[i].BlinkOnUserTurnAnimation(true);
                                    allPlayerHomeController[i].Reapet(signUpAcknowledgement.data.userTurnDetails.remainingTimer, signUpAcknowledgement.data.turnTimer);

                                    for (int j = 0; j < allPlayerHomeController[i].allPlayerToken.Count; j++)
                                    {
                                        if (currentTurnSeatIndex == signUpAcknowledgement.data.thisseatIndex)
                                        {
                                            if (signUpRequestSDKData.gameModeName.Equals("NUMBER"))
                                            {
                                                if (allPlayerHomeController[i].allPlayerToken[j].myLastBoxIndex + signUpAcknowledgement.data.playerMoves[24 - signUpAcknowledgement.data.movesLeft] < 56)
                                                    allPlayerHomeController[i].allPlayerToken[j].OnTurnTokenHighLight();
                                                else
                                                    allPlayerHomeController[i].allPlayerToken[j].TokenResetAsNormal();
                                            }
                                            else if (signUpRequestSDKData.gameModeName.Equals("DICE"))
                                                selfPlayerHomeController.diceImage.interactable = true;
                                            else if (signUpRequestSDKData.gameModeName.Equals("CLASSIC"))
                                                selfPlayerHomeController.diceImage.interactable = true;
                                        }

                                        if (signUpRequestSDKData.gameModeName.Equals("NUMBER"))
                                            allPlayerHomeController[i].UpdateDiceValue(signUpAcknowledgement.data.playerMoves[24 - signUpAcknowledgement.data.movesLeft]);
                                    }
                                }
                            }

                            if (signUpRequestSDKData.gameModeName.Equals("NUMBER"))
                            {
                                uiManager.numberViewController.RemoveFirstMove(signUpAcknowledgement.data.movesLeft);
                                if (signUpAcknowledgement.data.thisseatIndex == currentTurnSeatIndex)
                                    selfPlayerHomeController.allPlayerToken.ForEach((coockie) => coockie.MakeAClickAble(true));
                            }

                            if (signUpAcknowledgement.data.movesLeft == 1 && signUpAcknowledgement.data.thisseatIndex == currentTurnSeatIndex)
                                if (!signUpAcknowledgement.data.userTurnDetails.isExtraTurn)
                                    ludoTostMessage.ShowToastMessages(ToastMessage.LASTMOVE, true);



                            if (signUpRequestSDKData.gameModeName == "NUMBER")
                            {
                                uiManager.numberViewController.RemoveFirstMove(signUpAcknowledgement.data.movesLeft);

                                if (signUpAcknowledgement.data.userTurnDetails.isExtraTurn)
                                    moveText.text = (signUpAcknowledgement.data.movesLeft - 1).ToString();
                                else
                                    moveText.text = signUpAcknowledgement.data.movesLeft.ToString();
                            }
                            else if (signUpRequestSDKData.gameModeName.Equals("DICE"))
                                uiManager.timerController.StartMainTimer(signUpAcknowledgement.data.mainGameTimer);

                            break;
                        case "WINNER_DECLARED":

                            break;
                        default:
                            break;
                    }
                }
                else
                {
                    if (signUpAcknowledgement.data.playerInfo.Count != signUpAcknowledgement.data.maxPlayerCount)
                        ludoTostMessage.ShowToastMessages(ToastMessage.WAITFORPLAYER, false);
                }


                uiManager.noInternetController.HideNoInternetConnection();
                LudoInternetController.instance.CheckInternetWithPingpong();
            }
            catch (System.Exception ex)
            {
                Debug.LogError("Exception || " + ex.ToString());
            }

            uiManager.reconnectionController.ShowAndHideAnimation(false);
            uiManager.reconnectionController.ShowAndHideObject(false);
        }


        #region EXTRATIMEREJOIN
        public void OnResponseExtraTimer(string response)
        {
            UserExtraTimeStartResponse extraTimeStartResponse = JsonConvert.DeserializeObject<UserExtraTimeStartResponse>(response);
            ExtraTimer(extraTimeStartResponse.data.startTurnSeatIndex, extraTimeStartResponse.data.remainingTimer, extraTimeStartResponse.data.remainingTimer);
        }

        public void ExtraTimer(int currentTurnSeatIndex, float remainingTimer, float extraTimer)
        {
            for (int i = 0; i < allPlayerHomeController.Count; i++)
            {
                allPlayerHomeController[i].TurnDataReset();
                allPlayerHomeController[i].BlinkOnUserTurnAnimation(false);
                allPlayerHomeController[i].HideAndShowExtraTimer(false);
            }

            SoundManager.instance.TimeSoundStop(SoundManager.instance.timerAudio);
            for (int i = 0; i < allPlayerHomeController.Count; i++)
            {
                allPlayerHomeController[i].TurnDataReset();
                if (allPlayerHomeController[i].playerInfoData.seatIndex == currentTurnSeatIndex && allPlayerHomeController[i].playerInfoData.seatIndex != -1)
                {
                    allPlayerHomeController[i].turnTimerImage.fillAmount = 1;
                    allPlayerHomeController[i].HideAndShowExtraTimer(true);
                    allPlayerHomeController[i].Reapet(remainingTimer, extraTimer);
                }
            }
        }

        #endregion


        #region COOCKIEKILL
        public ParticleSystem killPratical;

        public void CheckForIsKillOrNot()
        {
            Debug.LogError("+++++======================CoockieKill  ");
            for (int i = 0; i < allPlayerHomeController.Count; i++)
                for (int j = 0; j < allPlayerHomeController[i].allPlayerToken.Count; j++)
                    allPlayerHomeController[i].allPlayerToken[j].CoockieManage();

            if (signUpRequestSDKData.gameModeName.Equals("NUMBER"))
                if (signUpAcknowledgement.data.thisseatIndex == userTurnStartResponseData.data.startTurnSeatIndex)
                    //if (signUpAcknowledgement.data.userTurnDetails.isExtraTurn && gameModeName.Equals("NUMBER"))
                    moveText.text = (userTurnStartResponseData.data.movesLeft - 1).ToString();

            if (moveTokenResponse.data.isCapturedToken)
            {
                for (int i = 0; i < allPlayerHomeController.Count; i++)
                {
                    if (allPlayerHomeController[i].playerInfoData.seatIndex == moveTokenResponse.data.capturedSeatIndex)
                    {
                        if (!signUpRequestSDKData.gameModeName.Equals("CLASSIC"))
                        {
                            for (int j = 0; j < allPlayerHomeController[i].allPlayerToken.Count; j++)
                            {
                                if (allPlayerHomeController[i].allPlayerToken[j].GetComponent<LudoTokenController>().cookieStaticIndex == moveTokenResponse.data.capturedTokenIndex)
                                {
                                    allPlayerHomeController[i].allPlayerToken[j].GetComponent<LudoTokenController>().KillMove();

                                    LudoTokenController coockieMovement = allPlayerHomeController[i].allPlayerToken[j].GetComponent<LudoTokenController>();
                                    Vector3 targetPosition = coockieMovement.playersWayPoints.wayPointsForTokenMove[coockieMovement.myLastBoxIndex + 1].transform.GetChild(0).position;
                                    killPratical.transform.position = targetPosition;

                                    killPratical.Play();
                                    SoundManager.instance.soundAudioSource.Stop();
                                    SoundManager.instance.TokenKill(SoundManager.instance.killAudio);
                                    allPlayerHomeController[i].allPlayerToken.ForEach((coockie) => coockie.transform.localScale = Vector3.one);
                                }
                            }
                        }
                        else
                        {
                            for (int j = 0; j < allPlayerHomeController[i].allPlayerToken.Count; j++)
                            {
                                if (allPlayerHomeController[i].allPlayerToken[j].GetComponent<LudoTokenController>().cookieStaticIndex == moveTokenResponse.data.capturedTokenIndex)
                                {
                                    allPlayerHomeController[i].allPlayerToken[j].GetComponent<LudoTokenController>().KillMove();

                                    LudoTokenController coockieMovement = allPlayerHomeController[i].allPlayerToken[j].GetComponent<LudoTokenController>();
                                    Vector3 targetPosition = coockieMovement.playersWayPoints.wayPointsForTokenMove[coockieMovement.myLastBoxIndex + 1].transform.GetChild(0).position;

                                    killPratical.transform.position = targetPosition;
                                    killPratical.Play();
                                    SoundManager.instance.soundAudioSource.Stop();
                                    SoundManager.instance.TokenKill(SoundManager.instance.killAudio);
                                    allPlayerHomeController[i].allPlayerToken.ForEach((coockie) => coockie.transform.localScale = Vector3.one);
                                }
                            }
                        }
                    }
                }
            }
        }

        #endregion

        public BoardManager boardManager;

        internal void SetPlayerSeatIndex(int id)
        {
            Debug.Log("SetPlayerSeatIndex || " + id);
            for (var i = 0; i < allPlayerHomeController.Count; i++)
            {
                var val = id + i;
                if (val >= allPlayerHomeController.Count)
                {
                    val = val - allPlayerHomeController.Count;
                }
                allPlayerHomeController[i].staticSeatIndex = val;
                Debug.Log(" || staticSeatIndex || " + allPlayerHomeController[i].staticSeatIndex);
            }
        }

        public JoinTableResponse joinTableResponse;

        public LudoTostMessage ludoTostMessage;
        public LudoHomeController selfPlayerHomeController;
        public void SetUserDataOnBoard(string jsonDataFromServer)
        {
            Debug.Log("<color><b>LudoLudoGameManager || SetUserDataOnBoard || jsonDataFromServer </b></color>" + jsonDataFromServer);
            joinTableResponse = JsonConvert.DeserializeObject<JoinTableResponse>(jsonDataFromServer);

            for (int i = 0; i < joinTableResponse.data.playerInfo.Count; i++)
            {
                for (int j = 0; j < allPlayerHomeController.Count; j++)
                {
                    if (allPlayerHomeController[j].staticSeatIndex == joinTableResponse.data.playerInfo[i].seatIndex)
                    {
                        Debug.LogError(" || staticSeatIndex || " + allPlayerHomeController[i].staticSeatIndex);
                        allPlayerHomeController[j].playerInfoData = joinTableResponse.data.playerInfo[i];
                        allPlayerHomeController[j].SetActiveAllToken();
                        allPlayerHomeController[j].innerHome.SetActive(true);
                        allPlayerHomeController[j].UpdateUserProfile(joinTableResponse.data.playerInfo[i].userProfile);
                    }
                    allPlayerHomeController[j].UpdateUserName();

                    for (int k = 0; k < allPlayerHomeController[j].allPlayerToken.Count; k++)
                        allPlayerHomeController[j].allPlayerToken[k].TokenResetAsNormal();

                }
            }

            selfPlayerHomeController = allPlayerHomeController.Find(player => player.playerInfoData.seatIndex == signUpAcknowledgement.data.thisseatIndex);

            if (joinTableResponse.data.playerInfo.Count <= (joinTableResponse.data.maxPlayerCount - 1))
                ludoTostMessage.ShowToastMessages(ToastMessage.WAITFORPLAYER, false);

        }


        #region SCORECHECK

        public void ScoreCheck(string responseJsonString)
        {
            ScoreViewResponse scoreView = JsonConvert.DeserializeObject<ScoreViewResponse>(responseJsonString);
            for (int i = 0; i < allPlayerHomeController.Count; i++)
                if (allPlayerHomeController[i].playerInfoData.seatIndex == scoreView.data.seatIndex)
                    for (int j = 0; j < allPlayerHomeController[i].allPlayerToken.Count; j++)
                        if (allPlayerHomeController[i].allPlayerToken[j].cookieStaticIndex == scoreView.data.tokenIndex)
                            allPlayerHomeController[i].allPlayerToken[j].ScoreView(scoreView.data.score);
        }

        #endregion

        public void OnTurnMissed(string jsonDataFromServer)
        {
            TurnMissedResponse turnMissedResponseData = JsonConvert.DeserializeObject<TurnMissedResponse>(jsonDataFromServer);
            for (int i = 0; i < allPlayerHomeController.Count; i++)
            {
                if (turnMissedResponseData.data.seatIndex == allPlayerHomeController[i].staticSeatIndex)
                {
                    Debug.Log("<color><b>TurnMissedResponseData  </b></color>" + turnMissedResponseData.data.totalTurnMissCounter);
                    allPlayerHomeController[i].TurnMissedCouter(turnMissedResponseData.data.totalTurnMissCounter);
                }
            }
        }

        #region TOKENMOVE
        public MoveTokenResponse moveTokenResponse;
        public void TokenMove(string response)
        {
            for (int i = 0; i < allPlayerHomeController.Count; i++)
            {
                allPlayerHomeController[i].TurnDataReset();
                allPlayerHomeController[i].BlinkOnUserTurnAnimation(false);
                allPlayerHomeController[i].HideAndShowExtraTimer(false);
                for (int j = 0; j < allPlayerHomeController[i].allPlayerToken.Count; j++)
                    allPlayerHomeController[i].allPlayerToken[j].HideToolTips();

                allPlayerHomeController[i].allPlayerToken.ForEach((coockie) => coockie.MakeAClickAble(false));
                allPlayerHomeController[i].allPlayerToken.ForEach((coockie) => coockie.TokenResetAsNormal());
            }

            moveTokenResponse = JsonConvert.DeserializeObject<MoveTokenResponse>(response);

            for (int i = 0; i < allPlayerHomeController.Count; i++)
            {
                if (allPlayerHomeController[i].playerInfoData.seatIndex == userTurnStartResponseData.data.startTurnSeatIndex)
                {
                    for (int j = 0; j < allPlayerHomeController[i].allPlayerToken.Count; j++)
                    {
                        if (allPlayerHomeController[i].allPlayerToken[j].cookieStaticIndex == moveTokenResponse.data.tokenMove)
                        {
                            allPlayerHomeController[i].allPlayerToken[j].CoockieManage();
                            if (moveTokenResponse.data.movementValue == 6 && allPlayerHomeController[i].allPlayerToken[j].myLastBoxIndex == -1)
                                allPlayerHomeController[i].allPlayerToken[j].CoockieMove(1);
                            else
                                allPlayerHomeController[i].allPlayerToken[j].CoockieMove(moveTokenResponse.data.movementValue);
                        }
                    }
                    for (int j = 0; j < moveTokenResponse.data.updatedScore.Count; j++)
                    {
                        if (allPlayerHomeController[i].playerInfoData.seatIndex == moveTokenResponse.data.updatedScore[j].seatIndex)
                            allPlayerHomeController[i].scoreText.text = moveTokenResponse.data.updatedScore[j].score.ToString();
                    }
                }
            }
        }
        #endregion


        #region USERTURNSTART
        public List<RectTransform> movePositon;
        public Image emojiBtn;
        public Button settingBtn;
        [Header("Move Number")] public int moveNumber;

        public UserTurnStartResponse userTurnStartResponseData;

        public void SetUserTurnStartData(string responseJsonString)
        {
            try
            {
                userTurnStartResponseData = JsonConvert.DeserializeObject<UserTurnStartResponse>(responseJsonString);
                currentTurnSeatIndex = userTurnStartResponseData.data.startTurnSeatIndex;

                if (signUpAcknowledgement.data.thisseatIndex == userTurnStartResponseData.data.startTurnSeatIndex)
                {
                    uiManager.helpController.CloseHelpScren();
                    uiManager.sideMenuController.CloseSideMenu();
                    uiManager.emojiController.CloseEmojiScreen();
                }

                ludoTostMessage.CloseToastMessage();

                for (int i = 0; i < allPlayerHomeController.Count; i++)
                {
                    allPlayerHomeController[i].TurnDataReset();
                    allPlayerHomeController[i].BlinkOnUserTurnAnimation(false);
                    allPlayerHomeController[i].HideAndShowExtraTimer(false);
                    allPlayerHomeController[i].diceNumberText.text = string.Empty;
                    allPlayerHomeController[i].EmojiButtonInteractable(true);
                }

                for (int i = 0; i < allPlayerHomeController.Count; i++)
                    for (int j = 0; j < allPlayerHomeController[i].allPlayerToken.Count; j++)
                    {
                        allPlayerHomeController[i].allPlayerToken[j].HideMyRing(false);
                        allPlayerHomeController[i].allPlayerToken[j].HideToolTips();
                    }

                for (int i = 0; i < allPlayerHomeController.Count; i++)
                {
                    if (allPlayerHomeController[i].playerInfoData.seatIndex == userTurnStartResponseData.data.startTurnSeatIndex)
                    {
                        allPlayerHomeController[i].BlinkOnUserTurnAnimation(true);
                        allPlayerHomeController[i].Reapet(signUpAcknowledgement.data.turnTimer, signUpAcknowledgement.data.turnTimer);

                        for (int j = 0; j < allPlayerHomeController[i].allPlayerToken.Count; j++)
                        {
                            if (userTurnStartResponseData.data.startTurnSeatIndex == signUpAcknowledgement.data.thisseatIndex)
                            {
                                if (signUpRequestSDKData.gameModeName.Equals("NUMBER"))
                                {
                                    if (allPlayerHomeController[i].allPlayerToken[j].myLastBoxIndex + signUpAcknowledgement.data.playerMoves[24 - userTurnStartResponseData.data.movesLeft] < 56)
                                        allPlayerHomeController[i].allPlayerToken[j].OnTurnTokenHighLight();
                                    else
                                        allPlayerHomeController[i].allPlayerToken[j].TokenResetAsNormal();
                                }
                                else if (signUpRequestSDKData.gameModeName.Equals("DICE"))
                                {
                                    selfPlayerHomeController.diceImage.interactable = true;
                                }
                                else if (signUpRequestSDKData.gameModeName.Equals("CLASSIC"))
                                {
                                    selfPlayerHomeController.diceImage.interactable = true;
                                }
                            }

                            if (signUpRequestSDKData.gameModeName.Equals("NUMBER"))
                                allPlayerHomeController[i].UpdateDiceValue(signUpAcknowledgement.data.playerMoves[24 - userTurnStartResponseData.data.movesLeft]);
                        }
                    }
                }

                if (signUpRequestSDKData.gameModeName.Equals("NUMBER"))
                {
                    uiManager.numberViewController.RemoveFirstMove(userTurnStartResponseData.data.movesLeft);
                    if (signUpAcknowledgement.data.thisseatIndex == userTurnStartResponseData.data.startTurnSeatIndex)
                        selfPlayerHomeController.allPlayerToken.ForEach((coockie) => coockie.MakeAClickAble(true));

                }

                if (userTurnStartResponseData.data.movesLeft == 1 && signUpAcknowledgement.data.thisseatIndex == userTurnStartResponseData.data.startTurnSeatIndex)
                    if (userTurnStartResponseData.data.isExtraTurn == false)
                        ludoTostMessage.ShowToastMessages(ToastMessage.LASTMOVE, true);

            }
            catch (System.Exception ex)
            {
                Debug.LogError("Exception || " + ex.ToString());
            }
        }

        #endregion

        [Header("TieBreakerResponse")]
        public TieBreakerResponse tieBreakerResponse;
        public GameObject tieBreakerBg;
        public void OnTieBreaker(string jsonDataFromServer)
        {
            tieBreakerResponse = JsonConvert.DeserializeObject<TieBreakerResponse>(jsonDataFromServer);
            tieBreakerBg.SetActive(true);
            for (int i = 0; i < tieBreakerResponse.data.userData.Count; i++)
            {
                for (int j = 0; j < allPlayerHomeController.Count; j++)
                {
                    if (allPlayerHomeController[j].staticSeatIndex == tieBreakerResponse.data.userData[i].seatIndex)
                    {
                        if (tieBreakerResponse.data.userData[tieBreakerResponse.data.winnerIndex].seatIndex == allPlayerHomeController[j].staticSeatIndex)
                            allPlayerHomeController[j].TieBreakerShowLines(tieBreakerResponse.data.userData[i].tokenIndex, tieBreakerResponse.data.userData[i].furthestToken, true);
                        else
                            allPlayerHomeController[j].TieBreakerShowLines(tieBreakerResponse.data.userData[i].tokenIndex, tieBreakerResponse.data.userData[i].furthestToken, false);
                    }
                }
            }
        }

        #region LEAVETABLE

        public void LeaveTable(string responseJsonString)
        {
            LeaveTableResponse leaveTableResponse = JsonConvert.DeserializeObject<LeaveTableResponse>(responseJsonString);
            for (int i = 0; i < allPlayerHomeController.Count; i++)
            {
                if (leaveTableResponse.data.playerSeatIndex == allPlayerHomeController[i].playerInfoData.seatIndex)
                {
                    allPlayerHomeController[i].gameObject.SetActive(false);
                    allPlayerHomeController[i].allPlayerToken.ForEach((cookie) => cookie.transform.SetParent(cookie.tokenParent.transform));
                    allPlayerHomeController[i].allPlayerToken.ForEach((cookie) => cookie.gameObject.SetActive(false));
                    allPlayerHomeController[i].playerLeftImage.SetActive(true);
                }

                if (allPlayerHomeController[i].playerInfoData.seatIndex == leaveTableResponse.data.playerSeatIndex && leaveTableResponse.data.playerSeatIndex == signUpAcknowledgement.data.thisseatIndex)
                    OnClickExit();
            }
        }

        #endregion

        public void OnClickExit()
        {

#if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
#endif
#if UNITY_ANDROID
            Application.Quit();
#endif

        }

        internal LudoHomeController ReturnHomeControllerFromSeatIndex(int seatIndex)
        {
            return allPlayerHomeController.Where(homeController => homeController.playerInfoData.seatIndex == seatIndex).Single();
        }
    }
}

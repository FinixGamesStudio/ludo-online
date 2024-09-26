using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening.Core.Easing;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

namespace Ludo
{
    public class LudoInternetController : MonoBehaviour
    {
        #region Variables

        public LudoGameManager gameManager;

        public static LudoInternetController instance;

        [Tooltip("flag if no internet popup is already open or not")]
        public static bool isInternetPopupOpened;

        public static bool isPongReceived = false;
        public bool checkInternetWithAPI, checkInternetWithHeartBeat;

        [SerializeField] internal Image networkIcon;

        [Tooltip(" NetWork Sprite ")]
        [SerializeField]
        internal Sprite goodSprite, normal1Sprite, normal2Sprite, lowSprite, badSprite, noInternetSprite;

        [SerializeField] string pingUrlForInternet = "https://ping.canbeuseful.com/ping.txt?uid=jksdf";


        private int badDelayCounter, errorCounter;

        public long pingTime, requestTime, timeDelay, pongTime;

        public int pingInterval = 1;
        public int firstRequestDelay = 2;
        public int totalErrorCounter = 6;
        public int totalBadDelayCounter = 6;


        private Coroutine pingPongWithAPICoroutine, pingPongWithHeartBeatCoroutine;
        public List<int> pongTimer;

        List<long> timeDelayList = new List<long>();

        #endregion

        #region Unity Callbacks

        private void Awake()
        {
            Debug.Log(" Internet Connection Ping Pong Url " + pingUrlForInternet);
            instance = this;
        }

        #endregion

        #region Show No Internet Alert Popup

        public void ShowNoInternetPopup()
        {
            if (!isInternetPopupOpened)
            {
                Debug.LogError(" NO INTERNET ");

                badDelayCounter = errorCounter = 0;
                networkIcon.sprite = noInternetSprite;
                StopCheckInternet();
                gameManager.socketConnection.ForceFullySocketDisconnet();
                isInternetPopupOpened = true;
                gameManager.uiManager.noInternetController.ShowNoInternetConnection("Please Check your internet connectiona and rejoin the game");
                gameManager.socketConnection.CheckForSocketStateAndInternet();
            }
        }

        #endregion

        #region Hide No Internet Alert Popup

        public void CloseNoInternetPopup()
        {
            isInternetPopupOpened = false;
            gameManager.uiManager.noInternetController.HideNoInternetConnection();
        }

        #endregion

        #region check is Internet Available or not

        bool IsInternetConnection()
        {
            return Application.internetReachability != NetworkReachability.NotReachable;
        }

        #endregion


        public void CheckInternetWithPingpong()
        {
            badDelayCounter = errorCounter = 0;
            StopCheckInternet();

            if (checkInternetWithAPI)
                pingPongWithAPICoroutine = StartCoroutine(GetRequestWithAPI(pingUrlForInternet));
            else if (checkInternetWithHeartBeat)
                pingPongWithHeartBeatCoroutine = StartCoroutine(GetRequestWithHeartBeat());
            else
                Debug.LogError(" Please Select Any METHOD for Internet Checking");
        }

        public void StopCheckInternet()
        {
            Debug.Log("<color=cyan><b> || StopCheckInternet || </b></color>");
            if (pingPongWithAPICoroutine != null)
                StopCoroutine(pingPongWithAPICoroutine);
            if (pingPongWithHeartBeatCoroutine != null)
                StopCoroutine(pingPongWithHeartBeatCoroutine);
        }

        IEnumerator GetRequestWithAPI(string uri)
        {
            yield return new WaitForSecondsRealtime(firstRequestDelay);
        SB:
            if (IsInternetConnection())
            {
                if (badDelayCounter > totalBadDelayCounter || errorCounter > totalErrorCounter)
                {
                    Debug.Log(" Auto Reconnect badDelayCounter || " + badDelayCounter + "|| errorCounter  " + errorCounter);
                    StopCheckInternet();
                    gameManager.uiManager.toastPopupController.ShowTopToastMessage("Slow Internet Connection...!!!", true, false);
                }
                else
                {
                    requestTime = new DateTimeOffset(DateTime.Now).ToUnixTimeMilliseconds();

                    using (UnityWebRequest webRequest = UnityWebRequest.Get(uri))
                    {
                        webRequest.timeout = pingInterval;

                        yield return webRequest.SendWebRequest();

                        string[] pages = uri.Split('/');
                        int page = pages.Length - 1;

                        switch (webRequest.result)
                        {
                            case UnityWebRequest.Result.ConnectionError:
                                Debug.LogError(": Connection Error Error : " + webRequest.error);
                                errorCounter++;
                                SetNetWorkIndicatorOnError(errorCounter);
                                Debug.Log(" <color=RED> NO Internet Connection 1  </color> Time Delay  XXXX  ");
                                break;
                            case UnityWebRequest.Result.DataProcessingError:
                                Debug.LogError(": DataProcessingError: " + webRequest.error);
                                Debug.Log(" <color=RED> NO Internet Connection  2 </color> Time Delay  XXXX  ");
                                break;
                            case UnityWebRequest.Result.ProtocolError:
                                Debug.LogError(": HTTP Error: " + webRequest.error);
                                errorCounter++;
                                break;
                            case UnityWebRequest.Result.Success:

                                errorCounter = (errorCounter > 3) ? errorCounter-- : 0;
                                timeDelay = new DateTimeOffset(DateTime.Now).ToUnixTimeMilliseconds() - requestTime;
                                timeDelayList.Add(timeDelay);
                                break;
                        }

                        InternetStatusShow();
                    }

                    yield return new WaitForSecondsRealtime(pingInterval);

                    goto SB;
                }
            }
            else
                ShowNoInternetPopup();
        }


        #region Check Ping and TimeOut

        IEnumerator GetRequestWithHeartBeat()
        {
            yield return new WaitForSecondsRealtime(firstRequestDelay);
        SB:
            if (IsInternetConnection())
            {
                SendPing();

                gameManager.uiManager.toastPopupController.CloseTopToastMessage();

                CloseNoInternetPopup();

                yield return new WaitForSecondsRealtime(pingInterval);
                if (badDelayCounter > totalBadDelayCounter || errorCounter > totalErrorCounter)
                {
                    Debug.Log(" Auto Reconnect badDelayCounter || " + badDelayCounter + "|| errorCounter  " + errorCounter);
                    StopCheckInternet();
                    gameManager.uiManager.toastPopupController.ShowTopToastMessage("Slow Internet Connection...!!!", true, false);
                }
                else
                {
                    if (isPongReceived)
                    {
                        errorCounter = (errorCounter > 3) ? errorCounter-- : 0;
                    }
                    else
                    {
                        errorCounter++;
                        Debug.Log("<Color=red>Ping server call missing:: </Color>" + errorCounter + "SocketState:: " + gameManager.socketConnection.socketState + " IsInternet Available: " + IsInternetConnection());
                        SetNetWorkIndicatorOnError(errorCounter);
                    }

                    goto SB;
                }
            }
            else
            {
                ShowNoInternetPopup();
            }
        }

        #endregion


        private void SendPing()
        {
            pingTime = new DateTimeOffset(DateTime.Now).ToUnixTimeMilliseconds();
            isPongReceived = false;

            gameManager.socketConnection.SendDataToSocket(gameManager.socketEventManager.RequestHeartBeat(), HeartBeatAcknowledgement, "HEART_BEAT");
        }

        #region Receive HEART_BEAT

        internal void HeartBeatAcknowledgement(string response)
        {
            //Debug.Log("<color><b> HeartBeatAcknowledgement </b></color>" + response);
            isPongReceived = true;
            pongTime = new DateTimeOffset(DateTime.Now).ToUnixTimeMilliseconds();
            timeDelay = pongTime - pingTime;
            timeDelayList.Add(timeDelay);
            InternetStatusShow();
        }

        #endregion


        private void InternetStatusShow()
        {
            int delayListCount = timeDelayList.Count;

            if (delayListCount == 0 || errorCounter > 0 && errorCounter < 5) return;
            if (delayListCount > 2) timeDelayList.RemoveAt(0);

            SetInternetStatus(timeDelayList[timeDelayList.Count - 1]);
        }


        private void SetInternetStatus(long timeDuration)
        {
            if (timeDuration >= pongTimer[0] && timeDuration < pongTimer[1])
            {
                networkIcon.sprite = goodSprite;
                badDelayCounter = 0;
            }
            else if (timeDuration >= pongTimer[1] && timeDuration < pongTimer[2])
            {
                networkIcon.sprite = normal1Sprite;
                badDelayCounter = (badDelayCounter > 0) ? errorCounter-- : 0;
            }
            else if (timeDuration >= pongTimer[2] && timeDuration < pongTimer[3])
            {
                networkIcon.sprite = normal2Sprite;
                badDelayCounter = (badDelayCounter > 0) ? errorCounter-- : 0;
                Debug.Log(" very Avarage Network ");
            }
            else if (timeDuration >= pongTimer[3] && timeDuration < pongTimer[4])
            {
                networkIcon.sprite = lowSprite;
                badDelayCounter++;
                Debug.Log("  Low  Network ");
            }
            else if (timeDuration > pongTimer[4])
            {
                networkIcon.sprite = badSprite;
                badDelayCounter++;
                Debug.Log("  very Low  Network ");
            }
        }

        private void SetNetWorkIndicatorOnError(int netWorkErrorCounter)
        {
            switch (netWorkErrorCounter)
            {
                case 1:
                    networkIcon.sprite = normal1Sprite;
                    break;

                case 2:
                    networkIcon.sprite = normal2Sprite;
                    break;

                case 3:
                    networkIcon.sprite = badSprite;
                    break;

                case 4:
                    networkIcon.sprite = badSprite;
                    break;
                default:
                    Debug.Log(" Internet Error Counter " + netWorkErrorCounter);
                    break;
            }
        }
    }
}

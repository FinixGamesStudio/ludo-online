using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using UnityEngine;
using static Ludo.TieBreakerResponseClass;

namespace Ludo
{
    public class LudoSocketEvnetReceiver : MonoBehaviour
    {
        public int userStartIndex;

        public LudoUiManager uiManager;

        public void ReciveData(string responseJsonString)
        {
            JObject responseJsonData = JObject.Parse(responseJsonString);
            string eventName = responseJsonData.GetValue("en").ToString();

            Debug.Log($"<color><b> TIME ||  {System.DateTime.Now.ToString("hh:mm:ss fff")} </b></color>||<color=blue><b> SEND EVENT </b></color><color><b>{eventName}</b></color>");
            Debug.Log("<color><b> TIME || " + System.DateTime.Now.ToString("hh:mm:ss fff") + " || </b></color><color=blue><b> SendDataWithAcknowledgement :- </b></color>" + responseJsonString);

            switch (eventName)
            {
                case "CONNECTION_SUCCESS":
                    LudoGameManager.instace.SendSingUpRequest();
                    break;
                case "JOIN_TABLE":
                    LudoGameManager.instace.SetUserDataOnBoard(responseJsonString);
                    break;
                case "GAME_TIMER_START":
                    uiManager.timerController.GameTimerStart(responseJsonString);
                    break;
                case "MAIN_GAME_TIMER_START":
                    uiManager.timerController.MainTimer(responseJsonString);
                    break;
                case "USER_TURN_START":
                    LudoGameManager.instace.SetUserTurnStartData(responseJsonString);
                    break;
                case "USER_EXTRA_TIME_START":
                    LudoGameManager.instace.OnResponseExtraTimer(responseJsonString);
                    break;
                case "TURN_MISSED":
                    LudoGameManager.instace.OnTurnMissed(responseJsonString);
                    break;
                case "BATTLE_FINISH":
                    uiManager.battelFinishCotroller.Battle(responseJsonString);
                    break;
                case "ALERT_POPUP":
                    uiManager.alertController.OnResponseAlert(responseJsonString);
                    break;
                case "LEAVE_TABLE":
                    LudoGameManager.instace.LeaveTable(responseJsonString);
                    break;
                case "MOVE_TOKEN":
                    LudoGameManager.instace.TokenMove(responseJsonString);
                    break;
                case "TIE_BREAKER":
                    LudoGameManager.instace.OnTieBreaker(responseJsonString);
                    break;
                case "HEART_BEAT":

                    break;
                case "HEART_BEAT_CLIENT":

                    break;
                case "SCORE_CHECK":
                    LudoGameManager.instace.ScoreCheck(responseJsonString);
                    break;
                case "SHOW_POPUP":
                    uiManager.commonPopupController.ShowPopUp(responseJsonString);
                    break;
                case "EMOJI":
                    uiManager.emojiController.OnEmojiResponse(responseJsonString);
                    break;
                case "DICE_ANIMATION_STARTED":
                    LudoDiceAnimationController.instance.OnDiceAnimationResponse(responseJsonString);
                    break;
                default:
                    break;
            }
        }

        public LudoReconnectionController reconnectionController;

        public GameObject tieBreakerBg;
    }
}
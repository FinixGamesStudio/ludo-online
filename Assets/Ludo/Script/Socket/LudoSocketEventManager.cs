using Newtonsoft.Json;
using UnityEngine;
using static Ludo.DiceAnimationResponseClass;
using static Ludo.EmojiRequestResponseClass;
using static Ludo.HeartBeatRequestClass;
using static Ludo.LeaveTableClass;
using static Ludo.MoveTokenResponseClass;
using static Ludo.ScoreViewResponseClass;
using static Ludo.SignUpClass;

namespace Ludo
{
    public class LudoSocketEventManager : MonoBehaviour
    {
        public LudoGameManager gameManager;

        public string SignUpRequstData()
        {
            SignUpRequestSDK signRequest = new SignUpRequestSDK();
            SignUpRequestSDKData signRequestData = new SignUpRequestSDKData();

            signRequestData = gameManager.signUpRequestSDKData;
            signRequestData.deviceId = SystemInfo.deviceUniqueIdentifier;
            //signRequestData.userId = SystemInfo.deviceUniqueIdentifier;

            Debug.Log("Sent id => " + signRequestData.userId);
            signRequest.data = signRequestData;

            string singUpJsonString = JsonConvert.SerializeObject(signRequest);
            return singUpJsonString;
        }

        public string MoveTokenCoockie(int coockieIndex)
        {
            MoveTokenRequest moveToken = new MoveTokenRequest();
            MoveTokenRequestData moveTokenRequestData = new MoveTokenRequestData();
            moveTokenRequestData.tokenMove = coockieIndex;
            moveToken.data = moveTokenRequestData;
            string json = JsonConvert.SerializeObject(moveToken);
            Debug.Log("Json Of Move Token" + json);
            return json;
        }

        public string SendLeaveTable()
        {
            LevaeTableRequest levaeTable = new LevaeTableRequest();
            Metrics metrics = new Metrics();
            LevaeTableRequestData levaeTableData = new LevaeTableRequestData();
            metrics.uuid = "caf09baf-faea-4849-8ee0-933db032bf18";
            metrics.ctst = "1677497513839";
            metrics.srct = "";
            metrics.srpt = "";
            metrics.crst = "1.2";
            metrics.userId = "";
            metrics.apkVersion = 101;
            metrics.tableId = "";
            levaeTableData.userSelfLeave = true;
            levaeTable.data = levaeTableData;
            levaeTable.metrics = metrics;
            string json = JsonConvert.SerializeObject(levaeTable);
            Debug.Log("Json Of LEAVEn" + json);
            return json;

        }

        public string Score(int seat)
        {
            ScoreViewRequest scoreView = new ScoreViewRequest();
            ScoreViewRequestData scoreViewData = new ScoreViewRequestData();

            scoreViewData.userID = gameManager.signUpAcknowledgement.userId;
            scoreViewData.seatIndex = gameManager.signUpAcknowledgement.data.thisseatIndex;
            scoreViewData.tokenIndex = seat;
            scoreView.data = scoreViewData;
            string json = JsonConvert.SerializeObject(scoreView);
            return json;
        }

        public string EmojiRequestData(int emojiNumber, int toSendSeatIndex)
        {
            EmojiEvent emojiRequest = new EmojiEvent();
            EmojiEventData emojiRequestData = new EmojiEventData();
            emojiRequest.en = "EMOJI";

            emojiRequestData.toSendSeatIndex = toSendSeatIndex;
            emojiRequestData.fromToSendSeatIndex = gameManager.signUpAcknowledgement.data.thisseatIndex;
            emojiRequestData.indexOfEmoji = emojiNumber;
            emojiRequestData.tableId = gameManager.signUpAcknowledgement.tableId;

            emojiRequest.data = emojiRequestData;
            string json = JsonConvert.SerializeObject(emojiRequest);
            return json;
        }

        public string SendDiceAnimation()
        {
            DiceAnimationRequest diceAnimationSend = new DiceAnimationRequest();
            diceAnimationSend.data = "";
            string diceAnimationSendJson = JsonConvert.SerializeObject(diceAnimationSend);
            //Debug.Log("Json DiceAnimationSend || " + diceAnimationSendJson);
            return diceAnimationSendJson;
        }


        public string RequestHeartBeat()
        {
            HeartBeatRequest heartBeat = new HeartBeatRequest();
            heartBeat.en = "HEART_BEAT";
            heartBeat.data = "";
            string requestHeartBeat = JsonConvert.SerializeObject(heartBeat);

            return requestHeartBeat;
        }
    }
}

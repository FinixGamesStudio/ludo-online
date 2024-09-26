using System.Collections.Generic;

namespace Ludo
{
    public class SignUpAcknowledgementClass
    {
        [System.Serializable]
        public class SignUpAcknowledgementData
        {
            public bool isAbleToReconnect;
            public string roomName;
            public int activePlayer;
            public int numberOfPlayers;
            public string tableState;
            public List<int> leftPlayerInfo;
            public List<PlayerInfoData> playerInfo;
            public int movesLeft;
            public int thisseatIndex;
            public List<int> playerMoves;
            public UserTurnDetails userTurnDetails;
            public int turnTimer;
            public int extraTimer;
            public int gameTimer;
            public float mainGameTimer;
            public bool isSix;
            public int sixCount;
            public int userTurnCount;
            public int maxPlayerCount;
        }
        [System.Serializable]
        public class Metrics
        {
            public string uuid;
            public string ctst;
            public string srct;
            public long srpt;
            public string crst;
            public string userId;
            public int apkVersion;
            public string tableId;
        }
        [System.Serializable]
        public class PlayerInfoData
        {
            public int seatIndex;
            public string userId;
            public string username;
            public List<int> tokenDetails;
            public int score;
            public int missedTurnCount;
            public int highestToken;
            public int remainingTimer;
            public string userProfile;
            public PlayerInfoData()
            {
                seatIndex = -1;
            }
        }
        [System.Serializable]
        public class SignUpAcknowledgement
        {
            public SignUpAcknowledgementData data;
            public Metrics metrics;
            public string userId;
            public string tableId;
        }
        [System.Serializable]
        public class UserTurnDetails
        {
            public int currentTurnSeatIndex;
            public bool isExtraTurn;
            public float remainingTimer;
            public bool isExtraTime;
            public int diceValue;
            public bool isDiceAnimated;
        }
    }
}

using System.Collections.Generic;
namespace Ludo
{
    public class UserTurnStartResponseClass
    {
        [System.Serializable]
        // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
        public class UserTurnStartResponseData
        {
            public int startTurnSeatIndex;
            public int diceValue;
            public bool isExtraTurn;
            public int movesLeft;
            public List<TokenPosition> tokenPosition;
            public bool isCurrentTurn;
            public int userTurnCount;
        }

        [System.Serializable]
        public class UserTurnStartResponse
        {
            public string en;
            public UserTurnStartResponseData data;
        }

        [System.Serializable]
        public class TokenPosition
        {
            public int seatIndex;
            public List<int> tokenDetails;
        }
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Ludo
{
    public class MoveTokenResponseClass
    {
        [System.Serializable]
        // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
        public class MoveTokenRequest
        {
            public MoveTokenRequestData data;
        }

        [System.Serializable]
        public class MoveTokenRequestData
        {
            public int tokenMove;
            public bool flashMove;
        }

        [System.Serializable]
        // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
        public class MoveTokenResponseData
        {
            public int tokenMove;
            public int movementValue;
            public bool isCapturedToken;
            public int capturedTokenIndex;
            public int capturedSeatIndex;
            public List<UpdatedScore> updatedScore;
            public bool isExtraScore;
            public int extraScorePlayerIndex;
            public int extraScore;
            public int captureTokenDecScore;
            public int killedTokenHomePosition;
            public List<PlayerFurthestTokenIndex> playerFurthestTokenIndex;
        }

        [System.Serializable]
        public class PlayerFurthestTokenIndex
        {
            public int seatIndex;
            public int highestToken;
        }

        [System.Serializable]
        public class MoveTokenResponse
        {
            public string en;
            public MoveTokenResponseData data;
        }

        [System.Serializable]
        public class UpdatedScore
        {
            public int seatIndex;
            public int score;
        }


    }
}

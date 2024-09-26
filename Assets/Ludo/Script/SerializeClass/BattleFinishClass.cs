using System.Collections.Generic;

namespace Ludo
{
    public class BattleFinishClass
    {
        [System.Serializable]
        // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
        public class BattleFinishResponseData
        {
            public string showResultOnAlert;
            public BattleFinishResponsePayload payload;
        }

        [System.Serializable]
        public class BattleFinishResponsePayload
        {
            public List<BattleFinishResponsePlayer> players;
        }

        [System.Serializable]
        public class BattleFinishResponsePlayer
        {
            public string userId;
            public string username;
            public int seatIndex;
            public bool isPlaying;
            public string avatar;
            public int score;
            public int winAmount;
            public string winType;
        }

        [System.Serializable]
        public class BattleFinishResponse
        {
            public string en;
            public BattleFinishResponseData data;
        }


    }
}

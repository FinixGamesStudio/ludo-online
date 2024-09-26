using System.Collections.Generic;
using static Ludo.SignUpAcknowledgementClass;

namespace Ludo
{
    public class JoinTableResponseClass
    {
        // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
        public class JoinTableResponseData
        {
            public List<PlayerInfoData> playerInfo;
            public int thisseatIndex;
            public string thisPlayerUserId;
            public int turnTimer;
            public int extraTimer;
            public List<int> playerMoves;
            public string tableId;
            public string queueKey;
            public int maxPlayerCount;
        }

        public class JoinTableResponse
        {
            public string en;
            public JoinTableResponseData data;
        }


    }
}

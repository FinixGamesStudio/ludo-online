
namespace Ludo
{
    public class ScoreViewResponseClass
    {
        [System.Serializable]
        public class ScoreViewRequestData
        {
            public string userID;
            public int seatIndex;
            public int tokenIndex;
        }
        [System.Serializable]
        public class ScoreViewRequestMetrics
        {
            public string uuid;
            public string ctst;
            public string srct;
            public string srpt;
            public string crst;
            public string userId;
            public int apkVersion;
            public string tableId;
        }

        [System.Serializable]
        public class ScoreViewRequest
        {
            public ScoreViewRequestMetrics metrics;
            public ScoreViewRequestData data;
        }

        [System.Serializable]
        public class ScoreViewResponseData
        {
            public int score;
            public int tokenIndex;
            public int seatIndex;
        }
        [System.Serializable]
        public class ScoreViewResponse
        {
            public string en;
            public ScoreViewResponseData data;
        }
    }
}
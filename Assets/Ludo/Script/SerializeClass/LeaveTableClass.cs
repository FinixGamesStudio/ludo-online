namespace Ludo
{
    public class LeaveTableClass
    {
        // LeaveTableResponse myDeserializedClass = JsonConvert.DeserializeObject<LeaveTableResponse>(myJsonResponse);
        public class LeaveTableResponseData
        {
            public int playerSeatIndex;
            public bool userSelfLeave;
        }

        public class LeaveTableResponse
        {
            public string en;
            public LeaveTableResponseData data;
        }

        public class LevaeTableRequestData
        {
            public bool userSelfLeave;
        }
        [System.Serializable]
        public class Metrics
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
        public class LevaeTableRequest
        {
            public Metrics metrics;
            public LevaeTableRequestData data;
        }
    }
}

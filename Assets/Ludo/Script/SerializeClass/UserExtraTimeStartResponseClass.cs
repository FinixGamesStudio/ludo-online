namespace Ludo
{
    public class UserExtraTimeStartResponseClass
    {
        [System.Serializable]
        public class UserExtraTimeStartResponse
        {
            public string en;
            public UserExtraTimeStartResponseData data;
        }
        [System.Serializable]
        // UserExtraTimeStartResponse myDeserializedClass = JsonConvert.DeserializeObject<UserExtraTimeStartResponse>(myJsonResponse);
        public class UserExtraTimeStartResponseData
        {
            public int startTurnSeatIndex;
            public int diceValue;
            public long resumeTimeStamp;
            public int remainingTimer;
        }
    }
}

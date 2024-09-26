namespace Ludo
{
    public class TurnMissedResponseClass
    {
        [System.Serializable]
        public class TurnMissedResponse
        {
            public string en;
            public TurnMissedResponseData data;
        }
        [System.Serializable]
        // TurnMissedResponseData turnMissedResponseData = JsonConvert.DeserializeObject<TurnMissedResponseData>(myJsonResponse);
        public class TurnMissedResponseData
        {
            public int seatIndex;
            public int totalTurnMissCounter;
            public int remainingTimer;
        }
    }
}
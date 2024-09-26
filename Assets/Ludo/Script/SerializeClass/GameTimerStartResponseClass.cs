namespace Ludo
{
    public class GameTimerStartResponseClass
    {
        [System.Serializable]
        // GameTimerStartData gameTimerStartData = JsonConvert.DeserializeObject<GameTimerStartData>(myJsonResponse);
        public class GameTimerStartResponse
        {
            public string en;
            public GameTimerStartResponseData data;
        }
        [System.Serializable]
        // GameTimerStartData gameTimerStartData = JsonConvert.DeserializeObject<GameTimerStartData>(myJsonResponse);
        public class GameTimerStartResponseData
        {
            public int waitingTimer;
        }
    }
}

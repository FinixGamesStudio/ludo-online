namespace Ludo
{
    public class GameMainTimerResponseClass
    {
        [System.Serializable]
        // GameTimerStartData gameTimerStartData = JsonConvert.DeserializeObject<GameTimerStartData>(myJsonResponse);
        public class GameMainTimerResponse
        {
            public string en;
            public GameMainTimerResponseData data;
        }
        [System.Serializable]
        // GameTimerStartData gameTimerStartData = JsonConvert.DeserializeObject<GameTimerStartData>(myJsonResponse);
        public class GameMainTimerResponseData
        {
            public float waitingTimer;
        }
    }
}

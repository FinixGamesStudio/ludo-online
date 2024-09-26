namespace Ludo
{
    public class AlertPopupClass
    {
        [System.Serializable]
        // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
        public class AlertPopupResponse
        {
            public string en;
            public AlertPopupResponseData data;
        }
        [System.Serializable]
        public class AlertPopupResponseData
        {
            public string message;
            public string errorCode;
        }
    }
}

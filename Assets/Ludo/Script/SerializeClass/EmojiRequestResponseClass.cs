using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ludo
{
    public class EmojiRequestResponseClass
    {
        // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
        [System.Serializable]
        public class EmojiEventData
        {
            public int indexOfEmoji;
            public int toSendSeatIndex;
            public int fromToSendSeatIndex;
            public string tableId;
        }

        [System.Serializable]
        public class EmojiEvent
        {
            public string en;
            public EmojiEventData data;
        }
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ludo
{
    public class DiceAnimationResponseClass
    {
        [System.Serializable]
        public class DiceAnimationResponseData
        {
            public int startTurnSeatIndex;
            public int diceValue;
            public bool autoMove;
            public bool isExtraTurn;
            public int autoMoveToken;
            public bool isSix;
            public int sixCount;
        }
        [System.Serializable]
        public class DiceAnimationResponse
        {
            public string en;
            public DiceAnimationResponseData data;
        }

        [System.Serializable]
        public class DiceAnimationRequest
        {
            public string en;
            public string data;
        }
    }
}
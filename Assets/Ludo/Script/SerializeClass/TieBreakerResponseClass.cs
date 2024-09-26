using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Ludo
{
    public class TieBreakerResponseClass
    {
        public class TieBreakerResponseData
        {
            public int winnerIndex;
            public bool isCancelToken;
            public List<UserData> userData;
        }

        public class UserData
        {
            public int seatIndex;
            public int furthestToken;
            public int tokenIndex;
        }

        [System.Serializable]
        public class TieBreakerResponse
        {
            public string en;
            public TieBreakerResponseData data;
        }
    }
}

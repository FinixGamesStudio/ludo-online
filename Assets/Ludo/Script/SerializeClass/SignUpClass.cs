using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ludo
{
    public class SignUpClass
    {
        // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
        [System.Serializable]
        public class SignUpRequestData
        {
            public string lobbyId;
            public int winning_amount;
            public string username;
            public string userId;
            public int maxPlayer;
            public string userProfile;
            public int entryFee;
            public string gameType;
            //public bool isUseBot;
            //public string roomName;
            //public int minPlayer;
            //public string gameId;
            //public string deviceId;
            //public string gameModeId;
            //public string gameModeName;
        }

        [System.Serializable]
        public class SignUpRequest
        {
            public SignUpRequestData data;
        }
        [System.Serializable]
        public class SignUpRequestSDKData
        {
            public string acessToken;
            public int minPlayer;
            public int maxPlayer;
            public string lobbyId;
            public string gameId;
            public string userId;
            public string username;
            public string userProfile;
            public string entryFee;
            public string winningAmount;
            public bool isUseBot;
            public bool isFTUE;
            public string deviceId;
            public string gameType;
            public string projectType;
            public string gameModeId;
            public string gameModeName;
        }

        [System.Serializable]
        public class SignUpRequestSDK
        {
            public SignUpRequestSDKData data;
        }
    }
}


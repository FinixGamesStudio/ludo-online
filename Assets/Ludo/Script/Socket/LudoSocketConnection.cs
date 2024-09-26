using UnityEngine;
using BestHTTP.SocketIO3;
using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;

namespace Ludo
{
    public class LudoSocketConnection : MonoBehaviour
    {
        public string socketUrl = "";

        public LudoSocketEvnetReceiver socketNumberEvnetReceiver;

        private static readonly byte[] ENCIV = new byte[16];

        private static byte[] ENCKEY = Encoding.ASCII.GetBytes("DFK8s58uWFCF4Vs8NCrgTxfMLwjL9WUy");

        [Header("SOCKET STATES")]
        public SocketState socketState;

        public int socketTimeout;

        public LudoGameManager gameManager;

        public SocketManager socketManager;

        private void Start()
        {
            Application.runInBackground = false;
            socketUrl = gameManager.ALLServerURL[(int)gameManager.serverType] + ":" + gameManager.ALLServerPortNo[(int)gameManager.serverType];

            LudoSocketConnectionStart(socketUrl);
        }

        void OnApplicationPause(bool pause)
        {
            if (pause)
            {
                ForceFullySocketDisconnet();
            }
            else
            {
                //ludoUiManager.ReconntionAnimation();
                //LudoSocketConnectionStart(socketUrl);// Call socket
                gameManager.socketConnection.CheckForSocketStateAndInternet();
            }
        }


        public void ForceFullySocketDisconnet()
        {
            if (socketManager != null)
            {
                Debug.Log("<color=red><b> || ForceFullySocketDisconnet || </b></color>");
                socketManager.Socket.Disconnect();
                socketState = SocketState.Disconnect;
                LudoInternetController.instance.StopCheckInternet();
            }
        }

        internal bool IsInternetConnectedCheck()
        {
            bool userInternetStatus = (Application.internetReachability != NetworkReachability.NotReachable);
            Debug.Log("Checking Internet Status : " + userInternetStatus);
            return userInternetStatus;
        }


        public SocketOptions SetSocketOption()
        {
            SocketOptions socketOptions = new SocketOptions();
            socketOptions.ConnectWith = BestHTTP.SocketIO3.Transports.TransportTypes.WebSocket;
            socketOptions.Reconnection = false;
            socketOptions.ReconnectionAttempts = int.MaxValue;
            socketOptions.ReconnectionDelay = TimeSpan.FromMilliseconds(1000);
            socketOptions.ReconnectionDelayMax = TimeSpan.FromMilliseconds(5000);
            socketOptions.RandomizationFactor = 0.5f;
            socketOptions.Timeout = TimeSpan.FromMilliseconds(10000);
            socketOptions.AutoConnect = true;
            socketOptions.QueryParamsOnlyForHandshake = true;
            socketOptions.Auth = (manager, socket) => new { token = gameManager.signUpRequestSDKData.acessToken };
            Debug.Log("Auth Token  || Recived from backend " + gameManager.signUpRequestSDKData.acessToken);
            return socketOptions;
        }

        public void LudoSocketConnectionStart(string socketURL)
        {
            LudoGameManager.instace.uiManager.reconnectionController.ShowAndHideAnimation(true);
            LudoGameManager.instace.uiManager.reconnectionController.ShowAndHideObject(true);
            if (socketManager != null)
            {
                if (socketManager.Socket.IsOpen)
                    socketManager.Socket.Disconnect();

            }
            if (!socketUrl.Contains("socket.io"))
                socketUrl = socketURL + "/socket.io/";
            else
                socketUrl = socketURL;

            Debug.Log("<color=blue> LudoSocketConnection || SocketContionStart || Connection URL -> </color>" + socketUrl);
            socketManager = null;
            socketManager = new SocketManager(new Uri(socketUrl), SetSocketOption());
            socketManager.Socket.On(SocketIOEventTypes.Connect, SocketConnected);
            socketManager.Socket.On(SocketIOEventTypes.Disconnect, SocketDisconnect);
            socketManager.Socket.On<Error>(SocketIOEventTypes.Error, SocketError);

            var events = Enum.GetValues(typeof(LudoNumberEventList)) as LudoNumberEventList[];

            for (int i = 0; i < events.Length; i++)
            {
                socketManager.Socket.On<string>(events[i].ToString(), (res) =>
                {
                    var data = res;
                    if (data == null) return;
                    socketState = SocketState.Running;
                    JObject jsonObj = JObject.Parse(res.ToString());
                    string playLoad = jsonObj.GetValue("data").ToString();
                    socketNumberEvnetReceiver.ReciveData(playLoad);
                });
            }
        }
        private void SocketError(Error error)
        {
            Debug.Log("<Color=blue> <-- SocketError :: SocketError ---> </color>" + error.message);
            socketState = SocketState.Error;
        }
        private void SocketDisconnect()
        {
            Debug.Log("<Color=red> <-- SocketDisconnect :: SocketDisconnect ---> </color>");
            socketState = SocketState.Disconnect;
        }

        public void SocketConnected()
        {
            Debug.Log(" <color=green>   Socket Connect Succed </color>");
            Debug.Log("<Color=yellow> <-- Socket_Connection :: Connected ---> </color>");
        }

        public void SendDataToSocket(string jsonDataToString, Action<string> onComplete, string eventName)
        {
            if (eventName != "HEART_BEAT")
            {
                Debug.Log($"<color><b> TIME ||  {System.DateTime.Now.ToString("hh:mm:ss fff")} </b></color>||<color=red><b> SEND EVENT </b></color><color><b>{eventName}</b></color>");
                Debug.Log("<color><b> TIME || " + System.DateTime.Now.ToString("hh:mm:ss fff") + " || </b></color><color=red><b> SendDataWithAcknowledgement :- </b></color>" + jsonDataToString);
            }
            socketManager.Socket.ExpectAcknowledgement<string>(onComplete).Volatile().Emit(eventName.ToString(), jsonDataToString.ToString());
        }

        public void CheckForSocketStateAndInternet()
        {
            CancelInvoke(nameof(IsInterConnectionThere));
            InvokeRepeating(nameof(IsInterConnectionThere), 1, 1);
        }

        public bool IsInternetConnection()
        {
            return Application.internetReachability != NetworkReachability.NotReachable;
        }

        void IsInterConnectionThere()
        {
            if (IsInternetConnection())
            {
                if (socketState == SocketState.Error || socketState == SocketState.Disconnect)
                {
                    CancelInvoke(nameof(IsInterConnectionThere));
                    LudoSocketConnectionStart(socketUrl);
                }
            }
            else
            {
                Debug.Log("<color><b> IsInterConnectionThere || Force Socket Close </b></color>");
                ForceFullySocketDisconnet();
                Debug.Log("You seem to be offline. Please check your internet connection");
            }
        }

        #region AESDecrypt
        public static string AESDecrypt(string cipherText, byte[] Key, byte[] IV)
        {
            byte[] cipherTextBytes;
            if (cipherText == null || cipherText.Length <= 0)
                throw new ArgumentNullException("cipherText");
            else
                cipherTextBytes = Convert.FromBase64String(cipherText);

            if (Key == null || Key.Length <= 0)
                throw new ArgumentNullException("Key");
            if (IV == null || IV.Length <= 0)
                throw new ArgumentNullException("Key");

            string plaintext = null;
            using (AesManaged aesAlg = new AesManaged())
            {
                aesAlg.Key = Key;
                aesAlg.IV = IV;
                ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);
                using (MemoryStream msDecrypt = new MemoryStream(cipherTextBytes))
                {
                    using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                    {
                        using (StreamReader srDecrypt = new StreamReader(csDecrypt))
                        {
                            plaintext = srDecrypt.ReadToEnd();
                        }
                    }
                }
            }
            return plaintext;
        }
        #endregion  

        #region AESEncrypt
        public static string AESEncrypt(string plainText, byte[] Key, byte[] IV)
        {
            // Check arguments.
            if (plainText == null || plainText.Length <= 0)
                throw new ArgumentNullException("plainText");
            if (Key == null || Key.Length <= 0)
                throw new ArgumentNullException("Key");
            if (IV == null || IV.Length <= 0)
                throw new ArgumentNullException("Key");


            byte[] encrypted;

            using (AesManaged aesAlg = new AesManaged())
            {
                aesAlg.Key = Key;
                aesAlg.IV = IV;
                ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);
                using (MemoryStream msEncrypt = new MemoryStream())
                {
                    using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                        {
                            swEncrypt.Write(plainText);
                        }
                        encrypted = msEncrypt.ToArray();
                    }
                }
            }
            return Convert.ToBase64String(encrypted);

        }
        #endregion


    }
    public enum SocketState
    {
        None,
        Close,
        Connect,
        Open,
        Running,
        Error,
        Disconnect
    }
    public enum ServerType
    {
        Live = 0,
        Dev = 1,
        Local = 2,
    }
}

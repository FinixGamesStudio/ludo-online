using System.Collections;
using Newtonsoft.Json;
using TMPro;
using UnityEngine;
using static Ludo.GameMainTimerResponseClass;
using static Ludo.GameTimerStartResponseClass;

namespace Ludo
{
    public class LudoTimerController : MonoBehaviour
    {
        public LudoTostMessage ludoTostMessage;

        public TextMeshProUGUI mainGameTimerText;//Dice Mode

        #region GAMETIMERSTART
        public GameTimerStartResponse gameTimeStart;
        public void GameTimerStart(string responseJsonString)
        {
            gameTimeStart = JsonConvert.DeserializeObject<GameTimerStartResponse>(responseJsonString);
            CountDownStart(gameTimeStart.data.waitingTimer);
        }

        [Header("GameMainTimer")]
        public GameMainTimerResponse gameMainTimer;

        public void MainTimer(string responseJsonString)
        {
            gameMainTimer = JsonConvert.DeserializeObject<GameMainTimerResponse>(responseJsonString);
            StartMainTimer(gameMainTimer.data.waitingTimer);
        }

        public void StartMainTimer(float timer)
        {
            gameMainTimer.data.waitingTimer = timer;
            CancleMainTimer();
            InvokeRepeating(nameof(GameMainTimer), 0.1f, 1f);
        }
        public void CancleMainTimer() => CancelInvoke(nameof(GameMainTimer));

        public void GameMainTimer()
        {
            gameMainTimer.data.waitingTimer -= 1;
            float minutes = Mathf.FloorToInt(gameMainTimer.data.waitingTimer / 60);
            float second = Mathf.FloorToInt(gameMainTimer.data.waitingTimer - minutes * 60f);
            string textTime = string.Format("{00:00}:{01:00}", minutes, second);
            mainGameTimerText.text = textTime;
            if (gameMainTimer.data.waitingTimer <= 0)
                CancelInvoke(nameof(GameMainTimer));
        }

        public int waitingTimer;
        public Coroutine coroutine;

        public void CountDownStart(int waitingTime)
        {
            waitingTimer = waitingTime;
            if (waitingTimer <= 0)
            {
                if (coroutine != null)
                    StopCoroutine(coroutine);

                CancelInvoke(nameof(DecreaseCounter));
            }
            else
                InvokeRepeating(nameof(DecreaseCounter), 1, 1);
        }

        private void DecreaseCounter()
        {
            waitingTimer--;
            ludoTostMessage.ShowToastMessages(ToastMessage.GAMECOUNTDOWN, false);
            ludoTostMessage.UpdateMessageText("NEW GAME BEGNING IN <color=#FFBA00>" + waitingTimer + "</color> SECONDS.");
            if (waitingTimer <= 0)
            {
                StopDecreaseCounter();
                coroutine = StartCoroutine(Time());
                return;
            }
        }

        IEnumerator Time()
        {
            yield return new WaitForSeconds(0.5f);
            if (waitingTimer == 0)
            {
                ludoTostMessage.UpdateMessageText("START");
            }
        }

        public void StopDecreaseCounter() => CancelInvoke(nameof(DecreaseCounter));

        #endregion
    }

}
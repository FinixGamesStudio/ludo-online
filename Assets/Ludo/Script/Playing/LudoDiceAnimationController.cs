using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.UI;
using static Ludo.DiceAnimationResponseClass;

namespace Ludo
{

    public class LudoDiceAnimationController : MonoBehaviour
    {
        public static LudoDiceAnimationController instance;

        public LudoGameManager gameManager;

        public List<Sprite> diceAnimationSprite;

        public List<Sprite> diceNumberSprite;

        private void Awake() => instance = this;

        public DiceAnimationResponse diceAnimationResponse;

        public void OnDiceAnimationResponse(string responseJsonString)
        {
            gameManager.selfPlayerHomeController.diceImage.interactable = false;
            diceAnimationResponse = JsonConvert.DeserializeObject<DiceAnimationResponse>(responseJsonString);

            for (int i = 0; i < gameManager.allPlayerHomeController.Count; i++)
            {
                if (!gameManager.signUpRequestSDKData.gameModeName.Equals("NUMBER"))
                    for (int j = 0; j < gameManager.allPlayerHomeController[i].allPlayerToken.Count; j++)
                        gameManager.allPlayerHomeController[i].allPlayerToken[j].MakeAClickAble(false);

                if (diceAnimationResponse.data.startTurnSeatIndex == gameManager.allPlayerHomeController[i].playerInfoData.seatIndex)
                    DiceAnimationStart(gameManager.allPlayerHomeController[i].diceImage.image, diceAnimationResponse);

                //DiceAnimationStart();
            }
        }

        public void SixDiceValueCounter(Transform sixDiceValueParent, int countOfSix)
        {
            SixDiceValueCounterReset(sixDiceValueParent);
            for (int i = 0; i < countOfSix; i++)
                sixDiceValueParent.GetChild(i).gameObject.SetActive(true);
        }

        public void SixDiceValueCounterReset(Transform sixDiceValueParent)
        {
            for (int i = 0; i < sixDiceValueParent.childCount; i++)
                sixDiceValueParent.GetChild(i).gameObject.SetActive(false);
        }
        public Coroutine diceCoroutine;

        public void DiceAnimationStart(Image diceImage, DiceAnimationResponse diceAnimationResponse)
        {
            diceImage.transform.DOScale(1f, 0.1f).OnComplete(() =>
            {
                diceImage.gameObject.GetComponent<RectTransform>().sizeDelta = new Vector3(-10, -10);
                SoundManager.instance.SoundPlay(SoundManager.instance.diceAnimationAudio);

                if (diceCoroutine != null)
                    StopCoroutine(diceCoroutine);
                diceCoroutine = StartCoroutine(DiceRoll(diceImage, () =>
                {
                    diceImage.transform.DOScale(1f, 0.1f);
                    SoundManager.instance.soundAudioSource.Stop();

                    if (diceAnimationResponse.data.diceValue - 1 >= 0)
                    {
                        diceImage.sprite = diceNumberSprite[diceAnimationResponse.data.diceValue - 1];
                        diceImage.gameObject.GetComponent<RectTransform>().sizeDelta = new Vector3(-30, -30);

                        for (int i = 0; i < gameManager.allPlayerHomeController.Count; i++)
                        {
                            if (gameManager.allPlayerHomeController[i].playerInfoData.seatIndex == diceAnimationResponse.data.startTurnSeatIndex
                            && gameManager.allPlayerHomeController[i].playerInfoData.seatIndex == gameManager.signUpAcknowledgement.data.thisseatIndex)
                            {
                                for (int j = 0; j < gameManager.allPlayerHomeController[i].allPlayerToken.Count; j++)
                                {
                                    Debug.Log("Number => " + gameManager.allPlayerHomeController[i].allPlayerToken[j].myLastBoxIndex);
                                    Debug.Log("Dice value => " + diceAnimationResponse.data.diceValue);

                                    if (gameManager.allPlayerHomeController[i].allPlayerToken[j].myLastBoxIndex == -1 && diceAnimationResponse.data.diceValue == 6)
                                    {
                                        gameManager.allPlayerHomeController[i].allPlayerToken[j].OnTurnTokenHighLight();
                                        gameManager.allPlayerHomeController[i].allPlayerToken[j].MakeAClickAble(true);
                                    }
                                    else if (gameManager.allPlayerHomeController[i].allPlayerToken[j].myLastBoxIndex != -1 && gameManager.allPlayerHomeController[i].allPlayerToken[j].myLastBoxIndex + diceAnimationResponse.data.diceValue <= 56)
                                    {
                                        gameManager.allPlayerHomeController[i].allPlayerToken[j].OnTurnTokenHighLight();
                                        gameManager.allPlayerHomeController[i].allPlayerToken[j].MakeAClickAble(true);
                                    }
                                    else
                                    {
                                        gameManager.allPlayerHomeController[i].allPlayerToken[j].TokenResetAsNormal();
                                        gameManager.allPlayerHomeController[i].allPlayerToken[j].MakeAClickAble(false);
                                    }
                                }
                            }
                            gameManager.allPlayerHomeController[i].allPlayerToken.ForEach((coockie) => coockie.UpdateSortingOrder(310));
                        }
                        if (gameManager.selfPlayerHomeController.playerInfoData.seatIndex == diceAnimationResponse.data.startTurnSeatIndex)
                        {
                            if (diceAnimationResponse.data.diceValue == 6)
                            {
                                SixDiceValueCounterReset(gameManager.selfPlayerHomeController.sixDiceValueParent);
                                SixDiceValueCounter(gameManager.selfPlayerHomeController.sixDiceValueParent, diceAnimationResponse.data.sixCount);
                                if (diceAnimationResponse.data.sixCount == 2)
                                    Invoke(nameof(SixDiceValueCounterReset), 1.5f);
                            }
                        }
                    }
                    else
                    {
                        Debug.LogError("I AM  Move =>" + (diceAnimationResponse.data.diceValue - 1));
                    }

                }));
            });
        }

        public IEnumerator DiceRoll(Image diceImage, System.Action callback)
        {
            for (int i = 0; i < 23; i++)
            {
                yield return new WaitForSeconds(0.01f);
                diceImage.sprite = diceAnimationSprite[i];
            }
            callback();
        }
    }
}
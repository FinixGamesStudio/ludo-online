using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace Ludo
{


    public class FTUEManager : MonoBehaviour
    {
        #region VARIABLES

        public static FTUEManager Instance;
        //internal LudoNumbers_SessionInfo sessionConfig = new LudoNumbers_SessionInfo();

        public GameObject FTUE_Panel;
        public GameObject display;
        public List<string> TutorialMessages;
        public List<string> TutorialMessagesForNumberMode;

        [Header("Tutorial Messages")]
        public Text TutorialTextMessage;
        public int TutorialIndex;
        public GameObject tutorialTextDisplay;

        [Header("Score")]
        public Text Score_Txt;
        public Text OpponentScore_Txt;

        [Header("Next Button Variables")]
        public Button NextButton;

        [Header("Step-UIs")]
        public GameObject Step1UI;
        public GameObject Step2UI, Step5UI, Step6UI, Step8UI;
        public GameObject tieBreakerUI;
        public GameObject tieNextButton;
        public GameObject tieArrow;
        public Text tieText;

        [Header("Step-1")]
        public GameObject numnerviewPanal;
        public GameObject dice;
        public GameObject num;
        public GameObject step1Arrow;

        [Header("Step-2")]
        public GameObject Step_2_Token;
        public List<GameObject> Step2_Token_Rings;
        public List<Transform> Step2_Way_Points;
        [SerializeField]
        internal Animator Step2_Token_ScaleAnimator;
        bool step2TokenMoved;
        public GameObject dice2;
        public GameObject num2;

        [Header("Step-4")]
        public GameObject arrowStep4;
        public GameObject logoStpe4;
        public GameObject scoreStep4;
        public GameObject score2Step4;

        [Header("Step-5")]
        public GameObject Step_5_Token;
        public List<GameObject> Step5_Token_Rings;
        public List<Transform> Step5_Way_Points;
        [SerializeField]
        internal Animator Step5_Token_ScaleAnimator;
        public GameObject Step_5_OpponentToken;
        public List<Transform> Step5_Opponent_Way_Points;
        [SerializeField]
        internal Animator Step5_OpponentToken_ScaleAnimator;
        bool step5TokenMoved;
        public GameObject dice5;
        public GameObject num5;
        public GameObject numberViewPanalStep5;

        [Header("Step-6")]
        public GameObject Step_6_Token;
        public List<GameObject> Step6_Token_Rings;
        public List<Transform> Step6_Way_Points;
        [SerializeField]
        internal Animator Step6_Token_ScaleAnimator;
        bool step6TokenMoved;
        [SerializeField]
        private Image Plus56PointsMessage;
        public GameObject dice6;
        public GameObject num6;
        public GameObject arrow6;
        public GameObject numberViewPanalStep6;

        [Header("Arrow Indicator")]
        public GameObject NextArrow;
        public GameObject Step_2_Arrow;
        public GameObject Step_5_Arrow;
        public GameObject Step_6_Arrow;

        public GameObject Moves_Left_BG, CaptureStep, CaptureStepHighLight, CaptureStepHighLightSecond, Moves_Left_BG_ForNumberMode;

        public LineRenderer line1;
        public LineRenderer line2;

        public GameObject TokenForStep2, NumberView, Step7LetsPlayBtn;

        public Text pawnScoreText;
        public Text captureScoreText;
        public Text homeScoreText;
        public Text captureLossText;

        public List<Transform> redPath = new List<Transform>();
        public List<Transform> yellowPath = new List<Transform>();

        public LudoUiManager ludoUiManager;
        public GameObject arrowOnHighLightScoreOnFTUE;

        public ParticleSystem killPartical;
        public ParticleSystem tokenAtHomePartical;
        public AudioSource tokenMoveSound;
        public AudioSource tokenKillSound;
        public AudioSource tokenAtHomeSound;
        public AudioSource nextBtnSound;
        public Color32 greenColor;



        public LudoSocketConnection socketConnection;
        #endregion

        #region DEFAULT UNITY METHODS

        void Awake()
        {
            Instance = this;
        }

        void OnDestroy()
        {
            Instance = null;
        }

        #endregion

        #region FTUE

        #region SET TUTORIAL MESSAGES

        private void Set_TutorialMessage()
        {
            ResetStepUIs();
            if (LudoGameManager.instace.signUpRequestSDKData.gameModeName.Equals("NUMBER"))
            {
                TutorialTextMessage.text = TutorialMessagesForNumberMode[TutorialIndex];
            }
            else
            {
                TutorialTextMessage.text = TutorialMessages[TutorialIndex];
            }
        }

        #endregion

        #region RESET STEPS UI
        public GameObject blueKillToken;
        void ResetStepUIs()
        {
            Step1UI.SetActive(false);
            Step2UI.SetActive(false);
            tokens.SetActive(false);
            TokenForStep2.SetActive(false);
            Step5UI.SetActive(false);
            greenKillType.SetActive(false);
            blueKillToken.SetActive(false);
            Step6UI.SetActive(false);
            greenInHome.SetActive(false);
            Step8UI.SetActive(false);
            tieBreakerUI.SetActive(false);
            tutorialTextDisplay.SetActive(true);
        }

        #endregion

        public void Next_FTUE()
        {
            /* if (!ludoUiManager.socketConnection.IsInternetConnectedCheck())
                 return;*/
            TutorialIndex++;
            Debug.Log(TutorialIndex);
            switch (TutorialIndex)
            {
                case 1:
                    Debug.Log("Step 1");
                    SetStep1();
                    nextBtnSound.Play();
                    break;
                case 2:
                    Debug.Log("Step 2");
                    SetStep2();
                    nextBtnSound.Play();
                    break;
                case 3:
                    Debug.Log("Step 3");
                    // NumberView.SetActive(true);
                    if (LudoGameManager.instace.signUpRequestSDKData.gameModeName.Equals("NUMBER"))
                    {
                        Moves_Left_BG_ForNumberMode.SetActive(true);
                        Moves_Left_BG_ForNumberMode.transform.SetAsLastSibling();
                    }
                    else
                    {
                        Moves_Left_BG.SetActive(true);
                        Moves_Left_BG.transform.SetAsLastSibling();
                    }
                    Set_TutorialMessage();
                    break;
                case 4:
                    Debug.Log("Step 4");
                    // NumberView.SetActive(false);
                    if (LudoGameManager.instace.signUpRequestSDKData.gameModeName.Equals("NUMBER"))
                    {
                        Moves_Left_BG_ForNumberMode.SetActive(false);
                        numnerviewPanal.SetActive(false);
                        scoreStep4.SetActive(true);
                        score2Step4.SetActive(true);
                        arrowStep4.SetActive(false);
                        logoStpe4.SetActive(false);
                    }
                    else
                    {
                        NextArrow.SetActive(false);
                        NextButton.interactable = false;
                        Moves_Left_BG.SetActive(false);
                        scoreStep4.SetActive(false);
                        score2Step4.SetActive(false);
                        arrowStep4.SetActive(true);
                        logoStpe4.SetActive(true);
                    }

                    CaptureStepHighLight.SetActive(true);
                    // Moves_Left_BG.transform.SetSiblingIndex();
                    CaptureStep.transform.SetAsLastSibling();



                    Set_TutorialMessage();
                    nextBtnSound.Play();
                    break;
                case 5:
                    Debug.Log("Step 5");
                    CaptureStepHighLight.SetActive(false);
                    Step_5_Arrow.SetActive(true);
                    //CaptureStep.transform.SetSiblingIndex();
                    SetStep5();
                    nextBtnSound.Play();
                    break;
                case 6:
                    Debug.Log("Step 6");
                    SetStep6();
                    break;
                case 7:
                    Debug.Log("Step 7");
                    //NumberView.SetActive(true);
                    if (LudoGameManager.instace.signUpRequestSDKData.gameModeName.Equals("NUMBER"))
                    {
                        Moves_Left_BG_ForNumberMode.SetActive(true);
                        numnerviewPanal.SetActive(false);
                        Moves_Left_BG_ForNumberMode.transform.SetAsLastSibling();
                    }
                    else
                    {
                        Moves_Left_BG.SetActive(true);
                        Moves_Left_BG.transform.SetAsLastSibling();
                    }
                    CaptureStepHighLightSecond.SetActive(true);
                    arrowOnHighLightScoreOnFTUE.SetActive(true);

                    Set_TutorialMessage();
                    break;
                case 8:
                    Debug.Log("Step 8");
                    //NumberView.SetActive(false);
                    nextBtnSound.Play();
                    arrowOnHighLightScoreOnFTUE.SetActive(false);
                    Moves_Left_BG.SetActive(false);
                    CaptureStepHighLightSecond.SetActive(false);
                    Moves_Left_BG.transform.SetSiblingIndex(1);
                    ShowTieBreaker();
                    break;
                case 9:
                    break;
            }
        }

        #region TOKEN MOVEMENT

        private IEnumerator MoveToken(GameObject token, List<Transform> way_Points, Animator token_ScaleAnimator)
        {
            var val = 0;
            // bool scaleUp = true;
            Tweener movetweener = null;
            token.transform.localScale = Vector3.one * 1.5f;

            //for (int i = 0; i < way_Points.Count; i++)
            //{
            //    token.transform.SetParent(way_Points[i].transform.GetChild(1));
            //    token.transform.GetComponent<RectTransform>().DOJumpAnchorPos(way_Points[i].transform.position, 15, 1, 0.18f).OnComplete(() =>
            //    {
            //        //token.transform.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
            //        SoundManager.instance.TimeSoundStop(SoundManager.instance.timerAudio);
            //        way_Points[i].GetComponent<LudoNumbersBoxProperty>().UpdateMyColor(Color.green);
            //        SoundManager.instance.SoundPlay(SoundManager.instance.tokenMoveAudio);
            //        DoFade(way_Points[i].GetComponent<LudoNumbersBoxProperty>().trail);
            //    });
            //    token.transform.DOScale(new Vector3(1.3f, 1.3f, 1.3f), 0.18f).OnComplete(() =>
            //    {
            //        token.transform.GetComponent<RectTransform>().localScale = new Vector3(1f, 1f, 1f);
            //    });

            //    yield return new WaitForSeconds(0.18f);
            //}



            while (val < way_Points.Count)
            {
                Vector3 target = way_Points[val].localPosition;
                Debug.Log("Postion => " + target + "Way Ponit Name =>" + way_Points[val].name);

                //  Debug.Log("Postion => " + a);
                var trail = way_Points[val].GetComponent<LudoNumbersBoxProperty>().trail;
                if (movetweener == null)
                {
                    token.transform.SetParent(way_Points[val].transform.GetChild(1));
                    token.transform.DOLocalMoveX(0, 0);
                    movetweener = token.transform.DOLocalMoveY(0, 0.2f).OnComplete(() =>
                         {
                             Debug.Log("Token Move sound play");
                             tokenMoveSound.Play();
                             movetweener = null;

                         });
                }

                if (val == way_Points.Count)
                    movetweener.Kill(true);


                token_ScaleAnimator.Play("scaleup");
                yield return new WaitForSeconds(0.21f);

                try
                {
                    //Debug.LogError("trail Name => " + trail);
                    //Debug.LogError("trail Parent Name => " + trail.transform.parent.name);

                    trail.color = greenColor;
                    trail.DOFade(0, 1f).SetEase(Ease.OutSine);
                }
                catch (System.Exception)
                {
                    // Debug.LogError(ex.ToString());
                }
                val++;
            }
        }
        private IEnumerator MoveTokenA(GameObject token, List<Transform> way_Points, Animator token_ScaleAnimator)
        {
            var val = 0;
            // bool scaleUp = true;
            Tweener movetweener = null;
            token.transform.localScale = Vector3.one * 1.5f;


            while (val < way_Points.Count)
            {
                Vector3 target = way_Points[val].localPosition;
                Debug.Log("Postion => " + target + "Way Ponit Name =>" + way_Points[val].name);


                //  Debug.Log("Postion => " + a);
                var trail = way_Points[val].GetComponent<LudoNumbersBoxProperty>().trail;
                if (movetweener == null)
                {
                    token.transform.SetParent(way_Points[val].transform.GetChild(1));
                    token.transform.DOLocalMoveY(0, 0);
                    movetweener = token.transform.DOLocalMoveX(0, 0.2f).OnComplete(() =>
                    {
                        tokenMoveSound.Play();
                        movetweener = null;

                    });
                }

                if (val == way_Points.Count)
                    movetweener.Kill(true);


                token_ScaleAnimator.Play("scaleup");
                yield return new WaitForSeconds(0.21f);

                try
                {
                    //Debug.LogError("trail Name => " + trail);
                    //Debug.LogError("trail Parent Name => " + trail.transform.parent.name);

                    trail.color = greenColor;
                    trail.DOFade(0, 1f).SetEase(Ease.OutSine);
                }
                catch (System.Exception)
                {
                    // Debug.LogError(ex.ToString());
                }
                val++;
            }
        }

        bool isKillSoundPlayed;
        IEnumerator MoveOpponentToken(GameObject token, List<Transform> way_Points, Animator token_ScaleAnimator)
        {
            var val = 0;
            // bool scaleUp = true;
            Tweener movetweener = null;

            while (val < way_Points.Count)
            {
                Vector3 target = way_Points[val].localPosition;
                if (movetweener == null)
                {
                    token.transform.SetParent(way_Points[val].transform.GetChild(1));
                    token.transform.DOLocalMoveX(0, 0.005f);
                    movetweener = token.transform.DOLocalMoveY(0, 0.005f).OnComplete(() =>
                    {
                        tokenMoveSound.Stop();
                        tokenKillSound.Play();
                        killPartical.Play();
                        movetweener = null;
                    });
                    token_ScaleAnimator.Play("scaleup");
                }

                if (val == way_Points.Count)
                {
                    movetweener.Kill(true);
                }
                else
                {
                    if (!isKillSoundPlayed)
                    {
                        isKillSoundPlayed = true;
                        //LudoNumbers_Store.Instance.MatchController.PlayAudioSound(LudoNumbers_Store.Instance.sfx_killtoken);
                    }
                }

                val++;
                yield return new WaitForSeconds(0.010f);
            }
        }

        #endregion

        #region Step-0
        /*internal void Step0()
        {
            //sessionConfig = Session.Instance.GetSessionInfo<LudoNumbers_SessionInfo>();
            FTUE_Panel.SetActive(true);
            TutorialIndex = 0;
            Set_TutorialMessage();
            NextArrow.SetActive(true);
        }*/

        #endregion

        #region Step-1

        private void SetStep1()
        {

            Set_TutorialMessage();
            Step1UI.SetActive(true);
            if (LudoGameManager.instace.signUpRequestSDKData.gameModeName.Equals("NUMBER"))
            {
                numnerviewPanal.SetActive(true);
                num.SetActive(true);
                dice.SetActive(false);
                step1Arrow.SetActive(false);
            }
            else
            {
                NextArrow.SetActive(false);
                NextButton.interactable = false;
                numnerviewPanal.SetActive(false);
                num.SetActive(false);
                dice.SetActive(true);
                step1Arrow.SetActive(true);
            }
        }


        #endregion

        #region Step-2

        public GameObject tokenParent;
        public GameObject tokens;
        private void SetStep2()
        {
            Set_TutorialMessage();
            Step2UI.SetActive(true);
            NextButton.interactable = false;
            NextArrow.SetActive(false);
            tokens.SetActive(true);
            TokenForStep2.SetActive(true);
            Step_2_Arrow.SetActive(true);


            tokenParent.SetActive(true);
            if (LudoGameManager.instace.signUpRequestSDKData.gameModeName.Equals("NUMBER"))
            {
                dice2.SetActive(false);
                num2.SetActive(true);
            }
            else
            {
                dice2.SetActive(true);
                num2.SetActive(false);
            }

        }

        public void Step2TokenClick()
        {
            if (!step2TokenMoved)
            {
                TokenForStep2.transform.DOLocalMoveX(-67.9f, 0);
                step2TokenMoved = true;
                for (var i = 0; i < Step2_Token_Rings.Count; i++)
                    Step2_Token_Rings[i].SetActive(false);
                StartCoroutine(MoveTokenStep2());
            }
        }

        private IEnumerator MoveTokenStep2()
        {
            Step_2_Arrow.SetActive(false);
            StartCoroutine(MoveToken(Step_2_Token, Step2_Way_Points, Step2_Token_ScaleAnimator));
            yield return new WaitForSeconds(0.6f);
            Score_Txt.text = "3";
            //
            NextArrow.SetActive(true);
            yield return new WaitForSeconds(0.3f);
            Next_FTUE();
            NextButton.interactable = true;
        }

        #endregion

        #region Step-5
        public GameObject greenKillType;
        private void SetStep5()
        {
            if (LudoGameManager.instace.signUpRequestSDKData.gameModeName.Equals("NUMBER"))
            {
                numberViewPanalStep5.SetActive(true);
                num5.SetActive(true);
                dice5.SetActive(false);
            }
            else
            {
                num5.SetActive(false);
                dice5.SetActive(true);
            }
            Score_Txt.text = "40";
            OpponentScore_Txt.text = "46";
            Set_TutorialMessage();
            ResetStepUIs();
            Step5UI.SetActive(true);
            blueKillToken.SetActive(true);
            //Step_5_Arrow.SetActive(true);
            greenKillType.SetActive(true);
            NextButton.interactable = false;
            NextArrow.SetActive(false);
        }

        public void Step5TokenClick()
        {
            if (!step5TokenMoved && ludoUiManager.socketConnection.IsInternetConnectedCheck())
            {
                step5TokenMoved = true;
                for (var i = 0; i < Step5_Token_Rings.Count; i++)
                    Step5_Token_Rings[i].SetActive(false);
                StartCoroutine(MoveTokenStep5());
            }
        }

        private IEnumerator MoveTokenStep5()
        {
            Step_5_Arrow.SetActive(false);
            StartCoroutine(MoveTokenA(Step_5_Token, Step5_Way_Points, Step5_Token_ScaleAnimator));
            yield return new WaitForSeconds(0.8f);
            tokenMoveSound.Play();
            Score_Txt.text = "49";
            OpponentScore_Txt.text = "37";
            Score_Txt.text = "49";
            StartCoroutine(MoveOpponentToken(Step_5_OpponentToken, Step5_Opponent_Way_Points, Step5_OpponentToken_ScaleAnimator));
            yield return new WaitForSeconds(1.8f);

            //NextButton.interactable = true;
            yield return new WaitForSeconds(0.2f);
            Next_FTUE();
        }

        #endregion

        #region Step-6
        /// <summary> Step 6 : Player Reaching Home </summary>
        public GameObject greenInHome;
        private void SetStep6()
        {

            if (LudoGameManager.instace.signUpRequestSDKData.gameModeName.Equals("NUMBER"))
            {
                num6.SetActive(true);
                dice6.SetActive(false);
                NextButton.interactable = false;
                NextArrow.SetActive(false);
                arrow6.SetActive(false);
                Step_6_Arrow.SetActive(true);
                numberViewPanalStep6.SetActive(true);
            }
            else
            {
                num6.SetActive(false);
                dice6.SetActive(true);
                NextButton.interactable = false;
                NextArrow.SetActive(false);
                arrow6.SetActive(true);
                Step_6_Arrow.SetActive(false);
            }


            Set_TutorialMessage();
            Score_Txt.text = "60";
            OpponentScore_Txt.text = "46";
            Step6UI.SetActive(true);
            greenInHome.SetActive(true);
            //Step_6_Arrow.SetActive(true);

        }

        public void Step6TokenClick()
        {
            if (!step6TokenMoved && ludoUiManager.socketConnection.IsInternetConnectedCheck())
            {
                step6TokenMoved = true;
                for (var i = 0; i < Step6_Token_Rings.Count; i++)
                    Step6_Token_Rings[i].SetActive(false);
                StartCoroutine(MoveTokenStep6());
            }
        }

        private IEnumerator MoveTokenStep6()
        {
            Step_6_Arrow.SetActive(false);
            StartCoroutine(MoveToken(Step_6_Token, Step6_Way_Points, Step6_Token_ScaleAnimator));
            yield return new WaitForSeconds(1f);


            tokenMoveSound.Stop();
            tokenKillSound.Stop();
            tokenAtHomePartical.Play();
            tokenAtHomeSound.Play();

            Plus56PointsMessage.gameObject.SetActive(true);
            Plus56PointsMessage.transform.localScale = Vector3.zero;
            Plus56PointsMessage.DOFade(1, 0f);
            Plus56PointsMessage.transform.DOScale(1f, 0.75f).SetEase(Ease.InOutBack).OnComplete(() =>
            {
                Plus56PointsMessage.DOFade(0f, 0.75f).SetDelay(0.5f).OnComplete(() =>
                {
                    Plus56PointsMessage.gameObject.SetActive(false);
                });
            });

            yield return new WaitForSeconds(1f);

            Score_Txt.text = "116";

            //NextButton.interactable = true;
            //yield return new WaitForSeconds(0.2f);
            Next_FTUE();
            NextButton.interactable = true;
            //NextArrow.SetActive(true);
        }

        #endregion

        #region Step-8

        /*  private void SetStep8()
          {
              ResetStepUIs();
              NextArrow.SetActive(false);
              tieArrow.SetActive(false);
              Step8UI.SetActive(true);
              line1.gameObject.SetActive(false);
              line2.gameObject.SetActive(false);
              try
              {
                  pawnScoreText.text = TutorialMessages[8];
                  captureScoreText.text = TutorialMessages[9];
                  homeScoreText.text = TutorialMessages[10];
                  captureLossText.text = TutorialMessages[11];
              }
              catch (System.Exception)
              {
                  pawnScoreText.text = "Pawn Score -- Earn 1 Point Per Square Moved";
                  captureScoreText.text = "Capture Score -- Earn 2 Points For Capturing An Opponent's Pawn";
                  homeScoreText.text = "Home Score -- Earn 56 Points For Reaching Home";
                  captureLossText.text = "Captured Loss -- Lose 1x Block Moves of Captured Pawn on being captured by an opponent";
              }
          }*/
        #endregion

        private async void ShowTieBreaker()
        {
            ResetStepUIs();
            Score_Txt.text = "100";
            OpponentScore_Txt.text = "100";
            tutorialTextDisplay.SetActive(false);
            tieBreakerUI.SetActive(true);
            NextArrow.SetActive(false);
            tieNextButton.SetActive(true);
            tieArrow.SetActive(true);
            line1.gameObject.SetActive(true);
            line2.gameObject.SetActive(true);
            StartCoroutine(StartAnimateLine(line1, redPath));
            StartCoroutine(StartAnimateLine(line2, yellowPath));
            try
            {
                //tieText.text = TutorialMessages[12];
            }
            catch (System.Exception)
            {
                tieText.text = "In case of ties, the token which is farthest away from the base will win the game.\nPlayer 1 (Green) will win the game as token is the farthest from the base.";
            }

            await Task.Delay(500);
            Step7LetsPlayBtn.SetActive(true);
        }

        private IEnumerator StartAnimateLine(LineRenderer line, List<Transform> path)
        {
            Debug.Log("Line Animation Started For ---> " + gameObject.name);
            var points = path.Count;
            Debug.Log("Line Animation Started For ---> " + points);
            for (var i = 0; i < points; i++)
            {
                line.positionCount++;
                line.SetPosition(i, path[i].position);
                yield return new WaitForSeconds(0.03f);
            }
        }

        #endregion

        public void EndTutorial()
        {
            if (!ludoUiManager.socketConnection.IsInternetConnectedCheck())
                return;
            Debug.Log("FTUE END");
            FTUE_Panel.SetActive(false);

            display.SetActive(true);

            SoundManager.instance.musicAudioSource.Play();

            PlayerPrefs.SetInt("FTUECount", 0);


            LudoGameManager.instace.signUpRequestSDKData.isFTUE = false;
            socketConnection.LudoSocketConnectionStart(socketConnection.socketUrl); // changes FTUE END 31-10-23

        }
    }
}
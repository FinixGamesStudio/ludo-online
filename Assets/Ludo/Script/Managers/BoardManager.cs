using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Ludo
{
    public class BoardManager : MonoBehaviour
    {
        [Header("=========================")]
        public List<LudoTokenController> plyer0Token;
        public List<LudoTokenController> plyer1Token;
        public List<LudoTokenController> plyer2Token;
        public List<LudoTokenController> plyer3Token;
        [Header("=========================")]

        public List<Image> homeBG;
        public List<Image> innerHome;
        public List<Image> innerHome0Child;
        public List<Image> innerHome1Child;
        public List<Image> innerHome2Child;
        public List<Image> innerHome3Child;
        public List<Sprite> homeBGSprite;
        public List<Sprite> innerHomeSprite;
        public List<Sprite> innerHomeChildSprite;


        public List<Sprite> playerTokenSprite;
        public Color green, blue, yellow, red;
        public List<TMPro.TextMeshProUGUI> score;
        public List<TMPro.TextMeshProUGUI> scoreText;
        public List<Image> leaveToken;
        public List<Sprite> leaveTokenSprite;
        public List<Image> tokenAroow;
        public List<Sprite> tokenArrowSprite;
        public List<Image> moveBG;
        public List<Sprite> moveBGSprite;

        public List<Image> plyer0Pathcolorbox;
        public List<Image> plyer1Pathcolorbox;
        public List<Image> plyer2Pathcolorbox;
        public List<Image> plyer3Pathcolorbox;

        public List<Sprite> plyerPathcolorboxSprite;

        public List<Image> winHome;
        public List<Sprite> winHomeSprite;
        public List<Image> leftPlayer;
        public List<Sprite> leftPlayerSprite;
        public Gradient greenGR, blueGR, yellowGR, redGR;
        public Color greenScoreText, blueScoreText, yellowScoreText, redScoreText;

        private void Awake()
        {
            //ChangeBordColor(3);
        }

        public void CheckColor(string color)
        {

            switch (color)
            {
                case "green":
                    ChangeBordColor(0);
                    break;
                case "blue":
                    ChangeBordColor(1);
                    break;
                case "yellow":
                    ChangeBordColor(2);
                    break;
                case "red":
                    ChangeBordColor(3);
                    break;
                default:
                    break;
            }
        }

        public void ChangeBordColor(int colorIndex)
        {
            int rotate = 90 * colorIndex;
            for (int i = 0; i < 4; i++)
            {
                homeBG[i].sprite = homeBGSprite[colorIndex];
                innerHome[i].sprite = innerHomeSprite[colorIndex];
                leaveToken[i].sprite = leaveTokenSprite[colorIndex];
                tokenAroow[i].sprite = tokenArrowSprite[colorIndex];
                moveBG[i].sprite = moveBGSprite[colorIndex];
                winHome[i].sprite = winHomeSprite[colorIndex];
                leftPlayer[i].sprite = leftPlayerSprite[colorIndex];
                tokenAroow[i].gameObject.transform.eulerAngles = new Vector3(0, 0, rotate);
                //winHome[i].gameObject.transform.eulerAngles = new Vector3(0, 0, rotate);
                switch (colorIndex)
                {
                    case 0:
                        score[i].color = greenScoreText;
                        scoreText[i].color = greenScoreText;
                        break;
                    case 1:
                        score[i].color = blueScoreText;
                        scoreText[i].color = blueScoreText;
                        break;
                    case 2:
                        score[i].color = yellowScoreText;
                        scoreText[i].color = yellowScoreText;
                        break;
                    case 3:
                        score[i].color = redScoreText;
                        scoreText[i].color = redScoreText;
                        break;
                }



                switch (i)
                {
                    case 0:
                        for (int j = 0; j < innerHome0Child.Count; j++)
                        {
                            innerHome0Child[j].sprite = innerHomeChildSprite[colorIndex];
                        }
                        for (int j = 0; j < plyer0Token.Count; j++)
                        {
                            plyer0Token[j].tokenImage.image.sprite = playerTokenSprite[colorIndex];
                            plyer0Token[j].myColor = ReturnColor(colorIndex);
                            plyer0Token[j].colorOverSpeed = ReturnGradient(colorIndex);
                        }
                        for (int j = 0; j < plyer0Pathcolorbox.Count; j++)
                        {
                            plyer0Pathcolorbox[j].sprite = plyerPathcolorboxSprite[colorIndex];
                        }
                        break;
                    case 1:
                        for (int j = 0; j < innerHome1Child.Count; j++)
                        {
                            innerHome1Child[j].sprite = innerHomeChildSprite[colorIndex];
                        }
                        for (int j = 0; j < plyer1Token.Count; j++)
                        {
                            plyer1Token[j].tokenImage.image.sprite = playerTokenSprite[colorIndex];
                            plyer1Token[j].myColor = ReturnColor(colorIndex);
                            plyer1Token[j].colorOverSpeed = ReturnGradient(colorIndex);
                        }
                        for (int j = 0; j < plyer1Pathcolorbox.Count; j++)
                        {
                            plyer1Pathcolorbox[j].sprite = plyerPathcolorboxSprite[colorIndex];
                        }
                        break;
                    case 2:
                        for (int j = 0; j < innerHome2Child.Count; j++)
                        {
                            innerHome2Child[j].sprite = innerHomeChildSprite[colorIndex];
                        }
                        for (int j = 0; j < plyer2Token.Count; j++)
                        {
                            plyer2Token[j].tokenImage.image.sprite = playerTokenSprite[colorIndex];
                            plyer2Token[j].myColor = ReturnColor(colorIndex);
                            plyer2Token[j].colorOverSpeed = ReturnGradient(colorIndex);
                        }
                        for (int j = 0; j < plyer2Pathcolorbox.Count; j++)
                        {
                            plyer2Pathcolorbox[j].sprite = plyerPathcolorboxSprite[colorIndex];
                        }
                        break;
                    case 3:
                        for (int j = 0; j < innerHome3Child.Count; j++)
                        {
                            innerHome3Child[j].sprite = innerHomeChildSprite[colorIndex];
                        }
                        for (int j = 0; j < plyer3Token.Count; j++)
                        {
                            plyer3Token[j].tokenImage.image.sprite = playerTokenSprite[colorIndex];
                            plyer3Token[j].myColor = ReturnColor(colorIndex);
                            plyer3Token[j].colorOverSpeed = ReturnGradient(colorIndex);
                        }
                        for (int j = 0; j < plyer3Pathcolorbox.Count; j++)
                        {
                            plyer3Pathcolorbox[j].sprite = plyerPathcolorboxSprite[colorIndex];
                        }
                        break;
                    default:
                        break;
                }
                if (colorIndex == 3)
                    colorIndex = 0;
                else
                    colorIndex++;
            }
        }

        public Color ReturnColor(int colorIndex)
        {
            Color color = green;
            switch (colorIndex)
            {
                case 0:
                    color = green;
                    break;
                case 1:
                    color = blue;
                    break;
                case 2:
                    color = yellow;
                    break;
                case 3:
                    color = red;
                    break;
            }
            return color;
        }

        public Gradient ReturnGradient(int colorIndex)
        {
            Gradient color = greenGR;
            switch (colorIndex)
            {
                case 0:
                    color = greenGR;
                    break;
                case 1:
                    color = blueGR;
                    break;
                case 2:
                    color = yellowGR;
                    break;
                case 3:
                    color = redGR;
                    break;
            }
            return color;
        }
    }
}

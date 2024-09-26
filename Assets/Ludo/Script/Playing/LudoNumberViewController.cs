using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace Ludo
{
    public class LudoNumberViewController : MonoBehaviour
    {
        //public static LudoNumberViewController ludoNumberView;

        public Transform parentTrasnfrom;
        public LudoNumberBoxUI moveTextPrefab;

        public List<LudoNumberBoxUI> allMovesObjects;

        public void GenerateMovesTextObjectes(List<int> playerMoves)
        {
            foreach (Transform child in parentTrasnfrom)
                Destroy(child.gameObject);

            for (int i = 0; i < playerMoves.Count; i++)
            {
                LudoNumberBoxUI cloneMoveText = Instantiate(moveTextPrefab, parentTrasnfrom);
                cloneMoveText.numberText.text = playerMoves[i].ToString();
                allMovesObjects.Add(cloneMoveText);
            }
            gameObject.SetActive(true);
        }

        public Sprite disableMovesSprite;
        public Sprite currentMovesSprite;

        public int disableBox;

        public void RemoveFirstMove(int leftMoves)
        {
            disableBox = allMovesObjects.Count - leftMoves;
            allMovesObjects[disableBox].boxImage.sprite = currentMovesSprite;
            for (int i = 0; i < disableBox; i++)
            {
                allMovesObjects[i].boxImage.sprite = disableMovesSprite;
                allMovesObjects[i].numberText.color = Color.white;
            }
        }
    }
}

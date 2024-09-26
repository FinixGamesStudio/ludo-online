using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ludo
{
    public class LudoPlayersWayPoints : MonoBehaviour
    {
        public int playerIndex;
        public List<RectTransform> way_Point;
        public List<RectTransform> wayPointsForTokenMove;
        public Color color;
    }
}

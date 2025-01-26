using UnityEngine;
using System;
using Unity.VisualScripting;
using UnityEngine.UI;

namespace SakugaEngine.UI
{
    public partial class RoundsCounter : MonoBehaviour
    {
        [SerializeField] private int RoundsLimit = 2;
        public Image[] RoundViews;

        public void ShowRounds(int roundsCount)
        {
            for (int i = 0; i < RoundsLimit; i++)
            {
                if (roundsCount - 1 == i)
                    RoundViews[i].gameObject.SetActive(true);
            }
        }
    }
}

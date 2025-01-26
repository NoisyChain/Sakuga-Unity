using UnityEngine;
using UnityEngine.UI;
using System;

namespace SakugaEngine.UI
{
    public partial class FadeScreen : MonoBehaviour
    {
        [Range(0, 100)] public int FadeIntensity;
        public Image screen;

        public void Update()
        {
            float alpha = FadeIntensity / 100f;
            screen.color = new Color(screen.color.r, screen.color.g, screen.color.b, alpha);
        }
    }
}

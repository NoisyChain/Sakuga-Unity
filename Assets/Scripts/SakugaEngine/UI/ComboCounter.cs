using SakugaEngine;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace SakugaEngine.UI
{
    public partial class ComboCounter : MonoBehaviour
    {
        [SerializeField] private Color InvalidHitColor = new Color(0, 0, 0);
        private Color DefaultColor;
        [SerializeField] private  Slider StunBar;
        [SerializeField] private  TextMeshProUGUI ComboCount;
        public void Awake()
        {
            //StunBar = GetNode<TextureProgressBar>("StunBar");
            //ComboCount = GetNode<Label>("ComboCount");
            //DefaultColor = StunBar.fillRect;
        }

        public void UpdateCounter(int stunValue, CombatTracker tracker)
        {
            StunBar.TintProgress = tracker.invalidHit ? InvalidHitColor : DefaultColor;
            StunBar.value = stunValue;
            ComboCount.text = tracker.HitCombo.ToString();
        }
    }
}

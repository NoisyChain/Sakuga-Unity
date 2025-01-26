using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace SakugaEngine.UI
{
    public partial class HealthHUD : MonoBehaviour
    {    
        [Header("Player 1")]
        [SerializeField] private Image P1Portrait;
        [SerializeField] private Slider P1Health;
        [SerializeField] private Slider P1LostHealth;
        [SerializeField] private RoundsCounter P1Rounds;
        [SerializeField] private ComboCounter P1Combo;
        [SerializeField] private TextMeshProUGUI P1Name;
        
        [Header("Player 2")]
        [SerializeField] private Image P2Portrait;
        [SerializeField] private Slider P2Health;
        [SerializeField] private Slider P2LostHealth;
        [SerializeField] private RoundsCounter P2Rounds;
        [SerializeField] private ComboCounter P2Combo;
        [SerializeField] private TextMeshProUGUI P2Name;

        [Header("Extra")]
        [SerializeField] private TextMeshProUGUI Timer;
        [SerializeField] private TextMeshProUGUI P1Debug;
        [SerializeField] private TextMeshProUGUI P2Debug;

        public void Setup(SakugaFighter[] fighters)
        {
            P1Health.maxValue = fighters[0].Variables.MaxHealth;
            P2Health.maxValue = fighters[1].Variables.MaxHealth;
            
            if (fighters[0].Profile.Portrait != null)
            {
                P1Portrait.sprite = fighters[0].Profile.Portrait;
                P1Name.text = fighters[0].Profile.ShortName;
            }

            if (fighters[1].Profile.Portrait != null)
            {
                P2Portrait.sprite = fighters[1].Profile.Portrait;
                P2Name.text = fighters[1].Profile.ShortName;
            }

            //P1Rounds.Setup();
            //P2Rounds.Setup();
        }

        public void UpdateHealthBars(SakugaFighter[] fighters, GameMonitor monitor)
        {
            P1Health.value = fighters[0].Variables.CurrentHealth;
            P2Health.value = fighters[1].Variables.CurrentHealth;
            P1LostHealth.value = fighters[0].FighterVars.LostHealth;
            P2LostHealth.value = fighters[1].FighterVars.LostHealth;

            UpdateTimer(monitor);

            P1Rounds.ShowRounds(monitor.VictoryCounter[0]);
            P2Rounds.ShowRounds(monitor.VictoryCounter[1]);

            P1Combo.gameObject.SetActive(fighters[1].Tracker.HitCombo > 0);
            P2Combo.gameObject.SetActive(fighters[0].Tracker.HitCombo > 0);

            P1Combo.UpdateCounter((int)fighters[1].HitStun.TimeLeft, fighters[1].Tracker);
            P2Combo.UpdateCounter((int)fighters[0].HitStun.TimeLeft, fighters[0].Tracker);
            UpdateDebug(fighters);
        }

        public void UpdateDebug(SakugaFighter[] fighters)
        {
            P1Debug.text = fighters[0].DebugInfo();
            P2Debug.text = fighters[1].DebugInfo();
        }

        public void UpdateTimer(GameMonitor monitor)
        {
            int time = (monitor.Clock / Global.TicksPerSecond) + 1;
            time = Mathf.Clamp(time, 0, monitor.ClockLimit);
            Timer.text = time.ToString();
        }
    }
}

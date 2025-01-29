using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace SakugaEngine.UI
{
    public partial class MetersHUD : MonoBehaviour
    {
        [SerializeField] private Slider P1Meter;
        [SerializeField] private Slider P2Meter;
        [SerializeField] private TextMeshProUGUI P1TrainingInfo;
        [SerializeField] private TextMeshProUGUI P2TrainingInfo;
        [SerializeField] private InputHistory P1InputHistory;
        [SerializeField] private InputHistory P2InputHistory;

        private int CurrentFrameAdvantage;

        /*public override void _Ready()
        {
            P1Meter = GetNode<TextureProgressBar>("Meters/P1Meter");
            P2Meter = GetNode<TextureProgressBar>("Meters/P2Meter");
            P1TrainingInfo = GetNode<Label>("TrainingInfo/P1Info/Information");
            P2TrainingInfo = GetNode<Label>("TrainingInfo/P2Info/Information");
        }*/

        public void Setup(SakugaFighter[] fighters)
        {
            P1Meter.maxValue = fighters[0].Variables.MaxSuperGauge;
            P2Meter.maxValue = fighters[1].Variables.MaxSuperGauge;
        }

        public void UpdateMeters(SakugaFighter[] fighters)
        {
            P1Meter.value = fighters[0].Variables.CurrentSuperGauge;
            P2Meter.value = fighters[1].Variables.CurrentSuperGauge;

            GetFrameAdvantage(fighters);

            if (P1InputHistory != null) P1InputHistory.SetHistoryList(fighters[0].Inputs);
            if (P2InputHistory != null) P2InputHistory.SetHistoryList(fighters[1].Inputs);

            P1TrainingInfo.text = TrainingInfoText(fighters[0], fighters[1]);
            P2TrainingInfo.text = TrainingInfoText(fighters[1], fighters[0]);
        }

        void GetFrameAdvantage(SakugaFighter[] fighters)
        {
            for (int i = 0; i < fighters.Length; i++)
            {
                if (fighters[i].Tracker.FrameAdvantage != 0)
                    CurrentFrameAdvantage = fighters[i].Tracker.FrameAdvantage;
            }
        }

        private string TrainingInfoText(SakugaFighter owner, SakugaFighter reference)
        {
            string hitTypeText = "";

            switch (reference.Tracker.LastHitType)
            {
                case 0:
                    hitTypeText = "HIGH";
                    break;
                case 1:
                    hitTypeText = "MID";
                    break;
                case 2:
                    hitTypeText = "LOW";
                    break;
                case 3:
                    hitTypeText = "UNBLOCKABLE";
                    break;
            }

            int refFrameAdv = CurrentFrameAdvantage;
            int ownFrameAdv = -CurrentFrameAdvantage;

            int finalFrameAdv = (int)owner.Tracker.FrameAdvantage != 0 ? ownFrameAdv : refFrameAdv;

            string frameAdvantageInfo = finalFrameAdv >= 0 ? 
                    ("+" + finalFrameAdv) : "" + finalFrameAdv;
            
            string frameAdvText = "("+frameAdvantageInfo+")";

            FighterVariables vars = reference.Variables as FighterVariables;

            return reference.Tracker.LastDamage + "\n" +
                    reference.Tracker.CurrentCombo + "\n" +
                    reference.Tracker.HighestCombo + "\n" +
                    hitTypeText + "\n" +
                    vars.CurrentDamageScaling + "\n" +
                    owner.Tracker.FrameData + frameAdvText;
        }
    }
}
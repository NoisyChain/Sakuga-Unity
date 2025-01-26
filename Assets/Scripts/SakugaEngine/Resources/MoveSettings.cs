using UnityEngine;

namespace SakugaEngine.Resources
{
    //[Icon("res://Sprites/Icons/Icon_Motion.png")]
    public partial class MoveSettings : ScriptableObject
    {
        public string MoveName;
        public int MoveState;
        public MotionInputs Inputs;
        public Global.SideChangeMode SideChange;
        public int SuperGaugeRequired = 0;
        public int BuildSuperGauge = 0;
        public Vector2Int HealthThreshold = new Vector2Int(0, 99999);
        public int SpendHealth = 0;
        public Vector2Int DistanceArea = new Vector2Int(0, 999999);
        public int[] IsSequenceFromStates;
        public int[] IgnoreStates;
        public int[] CancelsTo;
        public int[] KaraCancelsTo;
        public ButtonChargeSequence[] buttonChargeSequence;
        public ExtraVariableCondition[] ExtraVariablesRequirement;
        public ExtraVariableChange[] ExtraVariablesChange;
        public Global.MoveEndCondition MoveEnd;
        public int MoveEndState = -1;
        public int ChangeStance = -1;
        public int FrameLimit = -1;
        public int SuperFlash = 0;
        public int Priority = 0;
        public bool CanBeOverrided;
        public bool CanOverrideToSelf;
        public bool IgnoreSamePriority = true;
        public bool InterruptCornerPushback = false;
        public bool PriorityBuffer;
        public bool WaitForNullStates = true;
        public bool UseOnGround, UseOnAir;
    }
}

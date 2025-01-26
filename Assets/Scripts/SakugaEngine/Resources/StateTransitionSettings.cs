using UnityEngine;
using System;

namespace SakugaEngine.Resources
{
    public partial class StateTransitionSettings
    {
        public int StateIndex = -1;
        public Global.TransitionCondition Condition;
        public int AtFrame = -1;
        public MotionInputs Inputs;
        public Vector2Int DistanceArea = new Vector2Int(0, 999999);
    }
}

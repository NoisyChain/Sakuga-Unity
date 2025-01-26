using UnityEngine;

namespace SakugaEngine.Resources
{
    public class FighterState : ScriptableObject
    {
        public string StateName;
        public Global.StateType Type;
        public bool OffTheGround;
        public int Duration;
        public bool Loop;
        public Vector2Int LoopFrames;
        public int TurnState = -1;
        public int HitStunFrameLimit = -1;
        public FrameProperties[] stateProperties;
        public StatePhysics[] statePhysics;
        public HitboxState[] hitboxStates;
        public AnimationEventsList[] animationEvents;
        public ThrowPivot[] throwPivot;
        public StateTransitionSettings[] stateTransitions;
    }
}
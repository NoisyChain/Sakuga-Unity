using UnityEngine;

namespace SakugaEngine.Resources
{
    [System.Serializable]
    public class SpawnObjectAnimationEvent : AnimationEvent
    {
        public Global.ObjectType Object;
        public Vector2Int TargetPosition = Vector2Int.zero;
        public Global.RelativeTo xRelativeTo, yRelativeTo;
        public int Index;
        public bool IsRandom;
        public int Range;
        public int FromExtraVariable;
    }
}
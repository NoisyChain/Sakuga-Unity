using UnityEngine;

namespace SakugaEngine.Resources
{
    public class TeleportAnimationEvent : AnimationEvent
    {
        public int Index;
        public Vector2Int TargetPosition = Vector2Int.zero;
        public Global.RelativeTo xRelativeTo, yRelativeTo;
    }
}

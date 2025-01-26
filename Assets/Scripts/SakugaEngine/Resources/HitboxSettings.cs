using UnityEngine;

namespace SakugaEngine.Resources
{

    public partial class HitboxSettings : ScriptableObject
    {
        public Vector2Int PushboxCenter = Vector2Int.zero;
        public Vector2Int PushboxSize = Vector2Int.zero;
        public HitboxElement[] Hitboxes;
    }
}

using UnityEngine;

namespace SakugaEngine.Resources
{
    [CreateAssetMenu(fileName = "New Hitbox Settings", menuName = "Sakuga Engine/Hitbox Settings", order = 0)]
    public partial class HitboxSettings : ScriptableObject
    {
        public Vector2Int PushboxCenter = Vector2Int.zero;
        public Vector2Int PushboxSize = Vector2Int.zero;
        public HitboxElement[] Hitboxes;
    }
}

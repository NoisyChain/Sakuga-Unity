using UnityEngine;

namespace SakugaEngine.Resources
{
    [CreateAssetMenu(fileName = "New Sounds List", menuName = "Sakuga Engine/Sounds List", order = 0)]
    public partial class SoundsList : ScriptableObject
    {
        public AudioClip[] Sounds;
    }
}

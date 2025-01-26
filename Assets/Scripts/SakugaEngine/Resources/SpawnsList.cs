using UnityEngine;

namespace SakugaEngine.Resources
{
    [CreateAssetMenu(fileName = "New Spawns List", menuName = "Sakuga Engine/Spawns List", order = 0)]
    public partial class SpawnsList : ScriptableObject
    {
        public SpawnObject[] SpawnObjects;
    }
}

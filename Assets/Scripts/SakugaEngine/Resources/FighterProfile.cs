using UnityEngine;

namespace SakugaEngine.Resources
{
    [CreateAssetMenu(fileName = "New Fighter Profile", menuName = "Sakuga Engine/Fighter Profile", order = 0)]
    public class FighterProfile : ScriptableObject
    {
        public string FighterName;
        public string ShortName;
        public Sprite Render;
        public Sprite Portrait;
    }
}
